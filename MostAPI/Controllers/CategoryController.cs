using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;
using MostAPI.Service;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public CategoryController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // Получить все категории
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await _mongoDBService.GetCategoriesAsync();
            return Ok(categories);
        }

        // Добавить новую категорию
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Invalid data.");
                }

                await _mongoDBService.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.Error.WriteLine($"Error creating category: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        // Обновить категорию
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(string id, [FromBody] Category category)
        {
            await _mongoDBService.UpdateCategoryAsync(id, category);
            return NoContent();
        }

        // Удалить категорию
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(string id)
        {
            await _mongoDBService.DeleteCategoryAsync(id);
            return NoContent();
        }

        // Добавить услугу в категорию
        [HttpPost("{categoryId}/services")]
        public async Task<ActionResult> AddService(string categoryId, [FromBody] Services service)
        {
            await _mongoDBService.AddServiceAsync(categoryId, service);
            return CreatedAtAction(nameof(AddService), new { categoryId = categoryId, serviceId = service.Id }, service);
        }

        // Удалить услугу из категории
        [HttpDelete("{categoryId}/services/{serviceId}")]
        public async Task<ActionResult> DeleteService(string categoryId, string serviceId)
        {
            await _mongoDBService.DeleteServiceAsync(categoryId, serviceId);
            return NoContent();
        }

        // Фильтрация услуг по имени
        [HttpGet("{categoryId}/services")]
        public async Task<ActionResult<List<Services>>> FilterServices(string categoryId, [FromQuery] string nameFilter)
        {
            var services = await _mongoDBService.FilterServicesAsync(categoryId, nameFilter);
            return Ok(services);
        }
    }
}
