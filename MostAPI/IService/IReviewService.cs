using MostAPI.Data;

namespace MostAPI.IService
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsAsync();
        Task CreateReviewAsync(Review review);
        Task UpdateReviewAsync(int id, Review review);
        Task DeleteReviewAsync(int id);
    }
}
