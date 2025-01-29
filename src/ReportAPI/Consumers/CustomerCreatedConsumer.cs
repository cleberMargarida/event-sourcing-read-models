using CustomerAPI.Events;
using MassTransit;
using ReportAPI.Services;

namespace ReportAPI.Consumers
{
    public class CustomerCreatedConsumer(IReportService reportService) : IConsumer<CustomerCreatedEvent>
    {
        public async Task Consume(ConsumeContext<CustomerCreatedEvent> context)
        {
            await reportService.GenerateReportAsync(context.Message.Customer, context.CancellationToken);
        }
    }
}
