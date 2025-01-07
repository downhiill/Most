using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MostAPI.Data;
using MostAPI.Service;
using System.Collections.Generic;
using System.Linq;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var comparison = new ImageComparison
                {
                    PageId = pageId,
                    Image1 = await GetBytesAsync(request.Image1),
                    Image2 = await GetBytesAsync(request.Image2)
                };
                Console.WriteLine($"Image1 Length: {comparison.Image1.Length}, Image2 Length: {comparison.Image2.Length}");
                await _imageComparisons.InsertOneAsync(comparison);

                return CreatedAtAction(nameof(GetComparison), new { id = comparison.Id.ToString() }, comparison);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        // Получение сравнения по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageComparison>> GetComparison(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Некорректный формат ID.");
            }

            var comparison = await _imageComparisons.Find(c => c.Id == objectId).FirstOrDefaultAsync();
            if (comparison == null)
                return NotFound("Сравнение не найдено.");

            return Ok(comparison);
        }

        // Получение всех сравнений для страницы
        [HttpGet]
        public async Task<IActionResult> GetComparisons([FromQuery] int pageId)
        {
            try
            {
                var comparisons = await _imageComparisons.Find(c => c.PageId == pageId).ToListAsync();

                if (!comparisons.Any())
                    return NotFound("Сравнения не найдены для указанной страницы.");

                // Преобразование изображений в строки Base64
                var beforeImages = comparisons.Select(c => Convert.ToBase64String(c.Image1)).ToList();
                var afterImages = comparisons.Select(c => Convert.ToBase64String(c.Image2)).ToList();

                // Создаем ответ с двумя отдельными массивами изображений
                var response = new
                {
                    BeforeImages = beforeImages.ToArray(),  // Массив строк для изображений до
                    AfterImages = afterImages.ToArray()     // Массив строк для изображений после
                };

                // Возвращаем правильную структуру данных
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }



        // Редактирование существующего сравнения
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComparison(string id, [FromForm] ImageComparisonRequest request)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Некорректный формат ID.");
            }

            var existingComparison = await _imageComparisons.Find(c => c.Id == objectId).FirstOrDefaultAsync();
            if (existingComparison == null)
            {
                return NotFound("Сравнение не найдено.");
            }

            try
            {
                if (request.Image1 != null)
                {
                    existingComparison.Image1 = await GetBytesAsync(request.Image1);
                }

                if (request.Image2 != null)
                {
                    existingComparison.Image2 = await GetBytesAsync(request.Image2);
                }

                var updateResult = await _imageComparisons.ReplaceOneAsync(c => c.Id == objectId, existingComparison);

                if (updateResult.ModifiedCount == 0)
                {
                    return StatusCode(500, "Не удалось обновить сравнение.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        // Удаление сравнения
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComparison(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Некорректный формат ID.");
            }

            try
            {
                var result = await _imageComparisons.DeleteOneAsync(c => c.Id == objectId);
                if (result.DeletedCount == 0)
                    return NotFound("Сравнение не найдено.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        // Метод для получения массива байтов из IFormFile
        private static async Task<byte[]> GetBytesAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
