using MostAPI.Data;

namespace MostAPI.IService
{
    public interface IFaqService
    {
        Task<List<Faq>> GetRandomAsync(int count = 5);
        Task<Faq> GetByIdAsync(int id);
        Task CreateAsync(Faq faq);
        Task UpdateAsync(int id, Faq faq);
        Task DeleteAsync(int id);
    }
}
