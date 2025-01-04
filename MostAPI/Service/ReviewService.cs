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

        public async Task<List<Review>> GetReviewsAsync()
        {
            return await _reviews.Find(_ => true).ToListAsync();
        }

        public async Task CreateReviewAsync(Review review)
        {
            await _reviews.InsertOneAsync(review);
        }

        public async Task UpdateReviewAsync(int id, Review review)
        {
            var result = await _reviews.ReplaceOneAsync(r => r.Id == id, review);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Review with ID '{id}' not found.");
            }
        }

        public async Task DeleteReviewAsync(int id)
        {
            var result = await _reviews.DeleteOneAsync(r => r.Id == id);

            if (result.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Review with ID '{id}' not found.");
            }
        }
    }
}
