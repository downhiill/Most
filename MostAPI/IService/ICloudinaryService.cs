namespace MostAPI.IService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task SaveImageUrlToReviewAsync(int reviewId, string imageUrl);
    }
}
