using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MostAPI.Data;
using MostAPI.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageComparisonsController : ControllerBase
    {
        private readonly IMongoCollection<ImageComparison> _imageComparisons;

        public ImageComparisonsController(MongoDBService database)
        {
            _imageComparisons = database.ImageComparisons;
        }

        // Создание сравнения
        [HttpPost("{pageId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateImageComparison(int pageId, [FromForm] ImageComparisonRequest request)
        {
            if (request.Image1 == null || request.Image2 == null)
            {
                return BadRequest("Both images are required for comparison.");
            }

            var comparison = new ImageComparison
            {
                PageId = pageId
            };

            // Сохраняем первое изображение
            using (var memoryStream1 = new MemoryStream())
            {
                await request.Image1.CopyToAsync(memoryStream1);
                comparison.Image1 = memoryStream1.ToArray();
            }

            // Сохраняем второе изображение
            using (var memoryStream2 = new MemoryStream())
            {
                await request.Image2.CopyToAsync(memoryStream2);
                comparison.Image2 = memoryStream2.ToArray();
            }

            // Сохраняем сравнение в базу данных
            await _imageComparisons.InsertOneAsync(comparison);

            return CreatedAtAction(nameof(GetComparison), new { id = comparison.Id.ToString() }, comparison);
        }

        // Получение сравнения по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageComparison>> GetComparison(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var comparison = await _imageComparisons.Find(c => c.Id == objectId).FirstOrDefaultAsync();
            if (comparison == null)
                return NotFound("Comparison not found.");

            return Ok(comparison);
        }

        // Получение всех сравнений для страницы
        [HttpGet]
        public async Task<IActionResult> GetComparisons([FromQuery] int pageId)
        {
            var comparisons = await _imageComparisons.Find(c => c.PageId == pageId).ToListAsync();

            if (!comparisons.Any())
                return NotFound("No comparisons found for the given page.");

            // Разделяем изображения на "до" и "после"
            var beforeImages = comparisons.Select(c => Convert.ToBase64String(c.Image1)).ToList();
            var afterImages = comparisons.Select(c => Convert.ToBase64String(c.Image2)).ToList();

            return Ok(new
            {
                BeforeImages = beforeImages,
                AfterImages = afterImages
            });
        }

        // Редактирование существующего сравнения
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComparison(string id, [FromForm] ImageComparisonRequest request)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var existingComparison = await _imageComparisons.Find(c => c.Id == objectId).FirstOrDefaultAsync();
            if (existingComparison == null)
            {
                return NotFound("Comparison not found.");
            }

            // Обновляем изображения, если они предоставлены
            if (request.Image1 != null)
            {
                using (var memoryStream1 = new MemoryStream())
                {
                    await request.Image1.CopyToAsync(memoryStream1);
                    existingComparison.Image1 = memoryStream1.ToArray();
                }
            }

            if (request.Image2 != null)
            {
                using (var memoryStream2 = new MemoryStream())
                {
                    await request.Image2.CopyToAsync(memoryStream2);
                    existingComparison.Image2 = memoryStream2.ToArray();
                }
            }

            // Обновляем запись в базе данных
            var updateResult = await _imageComparisons.ReplaceOneAsync(c => c.Id == objectId, existingComparison);

            if (updateResult.ModifiedCount == 0)
            {
                return StatusCode(500, "Unable to update the comparison.");
            }

            return NoContent(); // Успешное обновление
        }

        // Удаление сравнения
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComparison(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var result = await _imageComparisons.DeleteOneAsync(c => c.Id == objectId);
            if (result.DeletedCount == 0)
                return NotFound("Comparison not found.");

            return NoContent();
        }
    }
}
