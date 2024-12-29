using MongoDB.Driver;
using MostAPI.Data;

namespace MostAPI.Service
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Category> _categories;

        public MongoDBService(IConfiguration config)
        {
            // Получаем строку подключения из переменной окружения
            var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGODB_URL");

            // Проверяем, что строка подключения получена, и если нет, выбрасываем исключение
            if (string.IsNullOrEmpty(mongoDbConnectionString))
            {
                throw new InvalidOperationException("MongoDB connection string is not set in environment variables.");
            }

            // Создаем клиента MongoDB и подключаемся к базе данных
            var client = new MongoClient(mongoDbConnectionString);
            var database = client.GetDatabase("Most");
            _categories = database.GetCollection<Category>("Categories");
        }

        // Получить все категории
        public async Task<List<Category>> GetCategoriesAsync()
        {
            // Получаем только id и name категорий, без данных об услугах
            var categories = await _categories
                .Find(category => true)
                .Project(category => new Category
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToListAsync();

            return categories;
        }

        // Добавить новую категорию
        public async Task CreateCategoryAsync(Category category) =>
            await _categories.InsertOneAsync(category);

        // Обновить категорию
        public async Task UpdateCategoryAsync(string id, Category category) =>
            await _categories.ReplaceOneAsync(c => c.Id == id, category);

        // Удалить категорию
        public async Task DeleteCategoryAsync(string id) =>
            await _categories.DeleteOneAsync(c => c.Id == id);

        // Получить услугу по id
        public async Task<Services> GetServiceByIdAsync(string categoryId, string serviceId) =>
            await _categories
                .Find(c => c.Id == categoryId)
                .Project(c => c.Services.FirstOrDefault(s => s.Id == serviceId))
                .FirstOrDefaultAsync();

        // Добавить услугу в категорию
        public async Task AddServiceAsync(string categoryId, Services service)
        {
            var category = await _categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();
            if (category != null)
            {
                category.Services.Add(service);
                await _categories.ReplaceOneAsync(c => c.Id == categoryId, category);
            }
        }

        // Удалить услугу из категории
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

        // Фильтрация услуг по имени
        public async Task<List<Services>> FilterServicesAsync(string categoryId, string nameFilter = null)
        {
            var category = await _categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();

            // Если категория не найдена, возвращаем null или пустой список
            if (category == null)
            {
                return new List<Services>();
            }

            // Если nameFilter не указан, возвращаем все услуги
            if (string.IsNullOrEmpty(nameFilter))
            {
                return category.Services;
            }

            // Если nameFilter задан, фильтруем услуги по имени
            return category.Services
                .Where(s => s.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
