using BetAPI.Events;
using MassTransit;
using ReportAPI.Services;

namespace ReportAPI.Consumers
{
    public class ReportBetSettledConsumer(IReportService reportService) : IConsumer<BetSettledEvent>
    {
        public async Task Consume(ConsumeContext<BetSettledEvent> context)
        {
            await reportService.GenerateReportAsync(context.Message.Bet, context.CancellationToken);
        }
    }
}
