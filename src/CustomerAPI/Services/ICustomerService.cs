
using CustomerAPI.Models;

namespace CustomerAPI.Services
{
    public interface ICustomerService
    {
        Task<IReadOnlyCollection<Customer>> ListCustomersAsync(CancellationToken cancellationToken);
        Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken);
    }
}
