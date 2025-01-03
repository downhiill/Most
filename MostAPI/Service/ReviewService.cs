using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.IService;

namespace MostAPI.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IMongoCollection<Review> _reviews;

        public ReviewService(MongoDBService mongoDBService)
        {
            _reviews = mongoDBService.Reviews;
        }

        public async Task<List<Review>> GetReviewsAsync() =>
            await _reviews.Find(review => true).ToListAsync();

        public async Task CreateReviewAsync(Review review) =>
            await _reviews.InsertOneAsync(review);

        public async Task UpdateReviewAsync(string id, Review review)
        {
            if (review == null) throw new ArgumentNullException(nameof(review));

            var updateDefinitions = new List<UpdateDefinition<Review>>();

            if (!string.IsNullOrWhiteSpace(review.Text))
                updateDefinitions.Add(Builders<Review>.Update.Set(r => r.Text, review.Text));

            if (review.Images != null && review.Images.Any())
                updateDefinitions.Add(Builders<Review>.Update.Set(r => r.Images, review.Images));

            if (!updateDefinitions.Any())
                throw new ArgumentException("No valid fields provided for update.");

            var update = Builders<Review>.Update.Combine(updateDefinitions);

            var result = await _reviews.UpdateOneAsync(r => r.Id == id, update);

            if (result.MatchedCount == 0)
                throw new KeyNotFoundException($"Review with id '{id}' not found.");
        }

        public async Task DeleteReviewAsync(string id) =>
            await _reviews.DeleteOneAsync(r => r.Id == id);
    }
}
