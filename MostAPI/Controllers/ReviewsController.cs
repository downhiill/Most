using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;
using MostAPI.IService;

namespace MostAPI.Controllers
{
    

    namespace MostAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class ReviewsController : ControllerBase
        {
            private readonly IReviewService _reviewService;
            private readonly ICloudinaryService _cloudinaryService;

            public ReviewsController(IReviewService reviewService, ICloudinaryService cloudinaryService)
            {
                _reviewService = reviewService;
                _cloudinaryService = cloudinaryService;
            }

            [HttpGet]
            public async Task<IActionResult> GetReviews()
            {
                var reviews = await _reviewService.GetReviewsAsync();
                return Ok(reviews);
            }

            [HttpPost]
            public async Task<IActionResult> CreateReview([FromBody] Review review)
            {
                await _reviewService.CreateReviewAsync(review);
                return Ok("Review created successfully.");
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
            {
                await _reviewService.UpdateReviewAsync(id, review);
                return Ok("Review updated successfully.");
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteReview(int id)
            {
                await _reviewService.DeleteReviewAsync(id);
                return Ok("Review deleted successfully.");
            }

            [HttpPost("{id}/upload-image")]
            public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile file)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(file);
                await _cloudinaryService.SaveImageUrlToReviewAsync(id, imageUrl);
                return Ok(new { Message = "Image uploaded successfully.", ImageUrl = imageUrl });
            }
        }
    }

}
