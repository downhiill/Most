using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.IService;

namespace MostAPI.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(MongoDBService mongoDBService)
        {
            _categories = mongoDBService.Categories;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categories
                .Find(category => true)
                .Project(category => new Category
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToListAsync();
        }

        public async Task CreateCategoryAsync(Category category) =>
            await _categories.InsertOneAsync(category);

        public async Task UpdateCategoryAsync(string id, Category category)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be null or whitespace.", nameof(id));
            if (category == null) throw new ArgumentNullException(nameof(category));

            var updateDefinitions = new List<UpdateDefinition<Category>>();

            if (!string.IsNullOrWhiteSpace(category.Name))
            {
                updateDefinitions.Add(Builders<Category>.Update.Set(c => c.Name, category.Name));
            }

            if (category.Services != null && category.Services.Any())
            {
                updateDefinitions.Add(Builders<Category>.Update.Set(c => c.Services, category.Services));
            }

            if (!updateDefinitions.Any())
            {
                throw new ArgumentException("No valid fields provided for update.");
            }

            var update = Builders<Category>.Update.Combine(updateDefinitions);

            var result = await _categories.UpdateOneAsync(c => c.Id == id, update);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Category with id '{id}' not found.");
            }
        }


        public async Task DeleteCategoryAsync(string id) =>
            await _categories.DeleteOneAsync(c => c.Id == id);
    }
}
