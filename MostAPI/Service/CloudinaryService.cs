using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.IService;

namespace MostAPI.Service
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IMongoCollection<Review> _reviews;

        public CloudinaryService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            // Настраиваем MongoDB
            _reviews = mongoDBService.Reviews;

            // Настраиваем Cloudinary
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            if (string.IsNullOrWhiteSpace(cloudName) ||
                string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(apiSecret))
            {
                throw new InvalidOperationException("Cloudinary configuration is missing or invalid.");
            }

            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret))
            {
                Api = { Secure = true }
            };
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.", nameof(file));
            }

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "clients",
                UseFilename = true,
                UniqueFilename = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new InvalidOperationException($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString();
        }

        public async Task SaveImageUrlToReviewAsync(int reviewId, string imageUrl)
        {

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("Image URL cannot be null or empty.", nameof(imageUrl));
            }

            var update = Builders<Review>.Update.Set(r => r.ImageUrl, imageUrl);
            var result = await _reviews.UpdateOneAsync(r => r.Id == reviewId, update);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Review with ID '{reviewId}' not found.");
            }
        }
    }
}
