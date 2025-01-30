using BetAPI.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sculptor.Core;

namespace BetAPI.Data
{
    public class BetContext(IServiceProvider services, DbContextOptions<BetContext> options) : DbContext(options)
    {
        public DbSet<Bet> Bets { get; set; } = default!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var events = ChangeTracker.Entries<IEventSourcing>()
                                      .SelectMany(x => x.Entity.Events)
                                      .OfType<object>();

            var publisher = services.GetRequiredService<IBus>();
            await publisher.PublishBatch(events, cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
