using Queue_Management_System.Models;

namespace Queue_Management_System.Services
{
    public interface IQueueRepository
    {
        Task<IEnumerable<ServicePointVM>> GetServices();
        Task AddCustomerToQueue(ServicePointVM customer);       
        Task<IEnumerable<QueueVM>> GetCalledCustomers();
        Task<IEnumerable<QueueVM>> GetWaitingCustomers(int servicePointId);        
        Task<QueueVM> MyCurrentServingCustomer(int servicePointId);       
        Task<QueueVM> UpdateOutGoingAndIncomingCustomerStatus(int outgoingCustomerId, int servicePointId);
        Task MarkNumberASNoShow(int outgoingCustomerId, int servicePointId);
        Task MarkNumberASFinished(int outgoingCustomerId, int servicePointId);
        Task TransferNumber(int currentServicePointId, int servicePointIdTranser);
    }
}
