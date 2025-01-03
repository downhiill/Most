using MostAPI.Data;

namespace MostAPI.IService
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsAsync();
        Task CreateReviewAsync(Review review);
        Task UpdateReviewAsync(string id, Review review);
        Task DeleteReviewAsync(string id);
    }
}
