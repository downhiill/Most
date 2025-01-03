using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.IService;
using MostAPI.Service;

namespace MostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMongoCollection<Review> _reviews;
        private readonly ICloudinaryService _cloudinaryService;

        public ReviewsController(MongoDBService mongoDBService, ICloudinaryService cloudinaryService)
        {
            _reviews = mongoDBService.Reviews;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviews.Find(_ => true).ToListAsync();
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromForm] string name, [FromForm] string username, [FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            var imageUrl = await _cloudinaryService.UploadImageAsync(image);

            var review = new Review
            {
                Name = name,
                Username = username,
                ImageUrl = imageUrl
            };

            await _reviews.InsertOneAsync(review);
            return CreatedAtAction(nameof(GetReviews), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review updatedReview)
        {

            var filter = Builders<Review>.Filter.Eq(r => r.Id, id);
            var update = Builders<Review>.Update
                .Set(r => r.Name, updatedReview.Name)
                .Set(r => r.Username, updatedReview.Username)
                .Set(r => r.ImageUrl, updatedReview.ImageUrl);

            var result = await _reviews.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                return NotFound($"Review with ID '{id}' not found.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {

            var filter = Builders<Review>.Filter.Eq(r => r.Id, id);
            var result = await _reviews.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                return NotFound($"Review with ID '{id}' not found.");
            }

            return NoContent();
        }
    }
}
