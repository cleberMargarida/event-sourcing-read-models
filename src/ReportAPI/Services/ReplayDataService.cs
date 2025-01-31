using BetAPI.Events;
using CustomerAPI.Events;
using EventStore.Client;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Data;
using System.Text.Json;

namespace ReportAPI.Services
{
    public class ReplayDataService : BackgroundService
    {
        private readonly IServiceScope _scope;
        private readonly ReportContext _context;
        private readonly IReportService _reportService;
        private readonly EventStoreClient _eventStore;

        public ReplayDataService(IServiceProvider serviceProvider, EventStoreClient eventStore)
        {
            _scope = serviceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ReportContext>();
            _reportService = _scope.ServiceProvider.GetRequiredService<IReportService>();
            _eventStore = eventStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(_scope.Dispose);
            var customerIds = await _context.CustomerPnlReportData.Select(x => x.CustomerId).Distinct().ToListAsync(stoppingToken);

            //clean up database by recreating it. This is a solution just for demontration purposes
            await _context.Database.EnsureDeletedAsync(stoppingToken);
            await _context.Database.EnsureCreatedAsync(stoppingToken);

            foreach (Guid customerId in customerIds)
            {
                await ReplayEventsForCustomer(customerId, stoppingToken);
            }
        }

        private async Task ReplayEventsForCustomer(Guid customerId, CancellationToken stoppingToken)
        {
            var stream = _eventStore.ReadStreamAsync(Direction.Forwards, customerId.ToString(), StreamPosition.Start, cancellationToken: stoppingToken)
            await foreach (var @event in stream)
            {
                if (@event.Event.EventType is "CustomerCreatedEvent")
                {
                    var @event = JsonSerializer.Deserialize<CustomerCreatedEvent>(@event.Event.Data.Span);
                    await _reportService.GenerateReportAsync(@event!.Customer, stoppingToken);
                }
                else if (@event.Event.EventType is "BetSettledEvent")
                {
                    var @event = JsonSerializer.Deserialize<BetSettledEvent>(@event.Event.Data.Span);
                    await _reportService.GenerateReportAsync(@event!.Bet, stoppingToken);
                }
            }
        }
    }
}
