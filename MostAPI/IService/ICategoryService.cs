using MostAPI.Data;

namespace MostAPI.IService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(int id, Category category);
        Task DeleteCategoryAsync(int id);
    }
}
