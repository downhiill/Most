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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateReview([FromForm] ReviewRequest request)
        {
            var review = new Review
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username
            };

            if (request.UploadedPhoto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.UploadedPhoto.CopyToAsync(memoryStream);
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
            var review = await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        // Получение всех отзывов
        [HttpGet]
        public async Task<ActionResult<List<Review>>> GetAllReviews()
        {
            var reviews = await _reviews.Find(new BsonDocument()).ToListAsync(); // Пустой фильтр для получения всех документов
            return Ok(reviews);
        }

        // Обновление отзыва
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateReview(string id, [FromForm] ReviewRequest request)
        {
            var review = await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();
            if (review == null)
                return NotFound();

            review.FirstName = request.FirstName;
            review.LastName = request.LastName;
            review.Username = request.Username;

            if (request.UploadedPhoto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.UploadedPhoto.CopyToAsync(memoryStream);
                    review.Photo = memoryStream.ToArray();
                }
            }

            var result = await _reviews.ReplaceOneAsync(r => r.Id == id, review);
            return NoContent();
        }

        // Удаление отзыва
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            var result = await _reviews.DeleteOneAsync(r => r.Id == id);
            if (result.DeletedCount == 0)
                return NotFound();
            return NoContent();
        }
    }
}
