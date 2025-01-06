using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;
using MostAPI.IService;
using MostAPI.Service;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly IServiceService _servicesService;

        public ServiceController(IServiceService servicesService)
        {
            _servicesService = servicesService;
        }
        // Добавить услугу в категорию
        [HttpPost("{categoryId}/services")]
        public async Task<ActionResult> AddService(int categoryId, [FromBody] Services service)
        {
            await _servicesService.AddServiceAsync(categoryId, service);
            return CreatedAtAction(nameof(AddService), new { categoryId = categoryId, serviceId = service.Id }, service);
        }

        // Удалить услугу из категории
        [HttpDelete("{categoryId}/services/{serviceId}")]
        public async Task<ActionResult> DeleteService(int categoryId, int serviceId)
        {
            await _servicesService.DeleteServiceAsync(categoryId, serviceId);
            return NoContent();
        }

        // Фильтрация услуг по имени
        [HttpGet("{categoryId}/services")]
        public async Task<ActionResult<List<Services>>> FilterServices(int categoryId, [FromQuery] string nameFilter = null)
        {
            var services = await _servicesService.FilterServicesAsync(categoryId, nameFilter);
            return Ok(services);
        }
    }
}
