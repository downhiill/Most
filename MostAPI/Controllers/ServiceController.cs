using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;
using MostAPI.Service;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public ServiceController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
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
        public async Task<ActionResult<List<Services>>> FilterServices(string categoryId, [FromQuery] string nameFilter = null)
        {
            var services = await _mongoDBService.FilterServicesAsync(categoryId, nameFilter);
            return Ok(services);
        }
    }
}
