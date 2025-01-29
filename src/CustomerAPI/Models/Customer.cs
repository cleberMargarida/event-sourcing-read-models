using CustomerAPI.Events;
using Sculptor.Core;

namespace CustomerAPI.Models
{
    public class Customer : AggregateRoot<Guid>
    {
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime Timestamp { get; private set; }

        public Customer(Guid id, string username, string firstName, string lastName, DateTime timestamp)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Timestamp = timestamp;

            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
                Timestamp = DateTime.UtcNow;
                AddEvent(new CustomerCreatedEvent(this));
            }
        }
    }
}
