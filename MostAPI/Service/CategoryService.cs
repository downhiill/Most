using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.IService;

namespace MostAPI.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(IMongoCollection<Category> categories)
        {
            _categories = categories;
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

        public async Task UpdateCategoryAsync(string id, Category category) =>
            await _categories.ReplaceOneAsync(c => c.Id == id, category);

        public async Task DeleteCategoryAsync(string id) =>
            await _categories.DeleteOneAsync(c => c.Id == id);
    }
}
