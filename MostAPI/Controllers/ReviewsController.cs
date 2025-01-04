using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.Service;

namespace MostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMongoCollection<Review> _reviews;

        public ReviewsController(MongoDBService database)
        {
            _reviews = database.Reviews;
        }

        // Создание отзыва
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] Review review, IFormFile photo)
        {
            if (photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    review.Photo = memoryStream.ToArray();
                }
            }

            await _reviews.InsertOneAsync(review);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        // Получение отзыва по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(string id)
        {
            var review = await _reviews.Find(r => r.Id == new ObjectId(id)).FirstOrDefaultAsync();
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        // Обновление отзыва
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(string id, [FromForm] Review review, IFormFile photo)
        {
            if (photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    review.Photo = memoryStream.ToArray();
                }
            }

            var result = await _reviews.ReplaceOneAsync(r => r.Id == new ObjectId(id), review);
            if (result.MatchedCount == 0)
                return NotFound();
            return NoContent();
        }

        // Удаление отзыва
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            var result = await _reviews.DeleteOneAsync(r => r.Id == new ObjectId(id));
            if (result.DeletedCount == 0)
                return NotFound();
            return NoContent();
        }
    }

}
