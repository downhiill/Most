using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.Service;

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
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateImageComparison([FromForm] ImageComparisonRequest request)
        {
            if (request.Image1 == null || request.Image2 == null)
            {
                return BadRequest("Both images are required for comparison.");
            }

            var comparison = new ImageComparison();

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
            return CreatedAtAction(nameof(GetComparison), new { id = comparison.Id }, comparison);
        }

        // Получение сравнения по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageComparison>> GetComparison(string id)
        {
            var comparison = await _imageComparisons.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (comparison == null)
                return NotFound();

            return Ok(comparison);
        }

        // Получение всех сравнений
        [HttpGet]
        public async Task<ActionResult<List<ImageComparison>>> GetAllComparisons()
        {
            var comparisons = await _imageComparisons.Find(Builders<ImageComparison>.Filter.Empty).ToListAsync();
            return Ok(comparisons);
        }

        // Удаление сравнения
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComparison(string id)
        {
            var result = await _imageComparisons.DeleteOneAsync(c => c.Id == id);
            if (result.DeletedCount == 0)
                return NotFound();

            return NoContent();
        }
    }
}
