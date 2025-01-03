namespace MostAPI.IService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task AddReviewImageAsync(string reviewId, string imageUrl);
    }
}
