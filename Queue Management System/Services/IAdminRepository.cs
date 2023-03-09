using Queue_Management_System.Models;
using System.Threading.Tasks;

namespace Queue_Management_System.Services
{
    public interface IAdminRepository
    {
        //ServiceProviders
        Task<IEnumerable<ServiceProviderVM>> GetServiceProviders();
        Task<ServiceProviderVM> GetServiceProviderDetails(int id);
        Task CreateServiceProvider(ServiceProviderVM serviceProvider);
        Task UpdateServiceProvider(ServiceProviderVM serviceProvider);
        Task DeleteServiceProvider(int id);


        //ServicePoints
        Task<IEnumerable<ServicePointVM>> GetServicePoints();
        Task<ServicePointVM> GetServicePointDetails(int id);
        Task CreateServicePoint(ServicePointVM servicePoint);
        Task UpdateServicePoint(ServicePointVM servicePoint);
        Task DeleteServicePoint(int id);
    }
}
