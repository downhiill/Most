using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;
using MostAPI.IService;
using MostAPI.Service;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Получить все категории
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
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

                await _categoryService.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.Error.WriteLine($"Error creating category: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            try
            {

                if (category == null)
                {
                    return BadRequest("Invalid data.");
                }

                await _categoryService.UpdateCategoryAsync(id, category);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Console.Error.WriteLine($"Error updating category: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating category: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        // Удалить категорию
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
