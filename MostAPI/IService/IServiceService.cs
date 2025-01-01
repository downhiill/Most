using MostAPI.Data;

namespace MostAPI.IService
{
    public interface IServiceService
    {
        Task<Services> GetServiceByIdAsync(int categoryId, int serviceId);
        Task AddServiceAsync(int categoryId, Services service);
        Task DeleteServiceAsync(int categoryId, int serviceId);
        Task<List<Services>> FilterServicesAsync(int categoryId, string nameFilter = null);
    }
}
