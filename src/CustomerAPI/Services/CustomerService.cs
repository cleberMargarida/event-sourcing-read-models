using CustomerAPI.Data;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Services
{
    public class CustomerService(CustomerContext context) : ICustomerService
    {
        public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            await context.AddAsync(customer);

            await context.SaveChangesAsync(cancellationToken);

            return customer;
        }

        public async Task<IReadOnlyCollection<Customer>> ListCustomersAsync(CancellationToken cancellationToken)
        {
            return await context.Customers.ToListAsync(cancellationToken);
        }
    }
}
