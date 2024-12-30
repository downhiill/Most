using MostAPI.Data;

namespace MostAPI.IService
{
    public interface IServiceService
    {
        Task<Services> GetServiceByIdAsync(string categoryId, string serviceId);
        Task AddServiceAsync(string categoryId, Services service);
        Task DeleteServiceAsync(string categoryId, string serviceId);
        Task<List<Services>> FilterServicesAsync(string categoryId, string nameFilter = null);
    }
}
