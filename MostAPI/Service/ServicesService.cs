using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.IService;

namespace MostAPI.Service
{
    public class ServicesService : IServiceService
    {
        private readonly IMongoCollection<Category> _categories;

        public ServicesService(MongoDBService mongoDBService)
        {
            _categories = mongoDBService.Categories;
        }

        public async Task<Services> GetServiceByIdAsync(string categoryId, string serviceId) =>
            await _categories
                .Find(c => c.Id == categoryId)
                .Project(c => c.Services.FirstOrDefault(s => s.Id == serviceId))
                .FirstOrDefaultAsync();

        public async Task AddServiceAsync(string categoryId, Services service)
        {
            var category = await _categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();
            if (category != null)
            {
                category.Services.Add(service);
                await _categories.ReplaceOneAsync(c => c.Id == categoryId, category);
            }
        }

        public async Task DeleteServiceAsync(string categoryId, string serviceId)
        {
            var category = await _categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();
            if (category != null)
            {
                var service = category.Services.FirstOrDefault(s => s.Id == serviceId);
                if (service != null)
                {
                    category.Services.Remove(service);
                    await _categories.ReplaceOneAsync(c => c.Id == categoryId, category);
                }
            }
        }

        public async Task<List<Services>> FilterServicesAsync(string categoryId, string nameFilter = null)
        {
            var category = await _categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();

            if (category == null)
            {
                return new List<Services>();
            }

            if (string.IsNullOrEmpty(nameFilter))
            {
                return category.Services;
            }

            return category.Services
                .Where(s => s.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
