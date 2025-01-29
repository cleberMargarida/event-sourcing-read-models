using CustomerAPI.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sculptor.Core;

namespace CustomerAPI.Data
{
    public class CustomerContext(IServiceProvider services, DbContextOptions<CustomerContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; } = default!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var events = ChangeTracker
                .Entries<IEventSourcing>()
                .SelectMany(x => x.Entity.Events)
                .OfType<object>();

            var publisher = services.GetRequiredService<IBus>();
            await publisher.PublishBatch(events, cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
