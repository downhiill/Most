using MongoDB.Driver;
using MostAPI.Data;

namespace MostAPI.Service
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;

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
            _database = client.GetDatabase("Most");
        }

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Faq> Faqs => _database.GetCollection<Faq>("FAQ");
    }

}
