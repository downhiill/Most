using MostAPI.Data;

namespace MostAPI.IService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(string id, Category category);
        Task DeleteCategoryAsync(string id);
    }
}
