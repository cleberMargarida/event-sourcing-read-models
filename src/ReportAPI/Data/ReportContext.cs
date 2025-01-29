using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Models;
using Sculptor.Core;

namespace ReportAPI.Data
{
    public class ReportContext(IServiceProvider services, DbContextOptions<ReportContext> options) : DbContext(options)
    {
        public DbSet<CustomerPnlReport> CustomerPnlReportData { get; set; } = default!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var events = ChangeTracker.Entries<IEventSourcing>()
                                      .SelectMany(x => x.Entity.Events)
                                      .OfType<object>();

            var publisher = services.GetRequiredService<IBus>();
            await publisher.PublishBatch(events, cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerPnlReport>().HasIndex(x => x.Timestamp);
            modelBuilder.Entity<CustomerPnlReport>().HasIndex(x => x.Username);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
