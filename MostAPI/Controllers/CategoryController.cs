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
            try
            {
                var categories = await _mongoDBService.GetCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching categories: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
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
    }
}
