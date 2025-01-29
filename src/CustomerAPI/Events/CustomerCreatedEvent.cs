using CustomerAPI.Models;
using Sculptor.Core;

namespace CustomerAPI.Events
{
    public class CustomerCreatedEvent(Customer customer) : DomainEvent
    {
        public Customer Customer { get; } = customer;
    }
}
