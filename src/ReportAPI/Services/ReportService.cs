using BetAPI.Models;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Data;
using ReportAPI.Models;

namespace ReportAPI.Services
{
    public class ReportService(ReportContext context) : IReportService
    {
        public async Task GenerateReportAsync(Bet bet, CancellationToken cancellationToken)
        {
            var customerId = bet.CustomerId;

            var report = await context.CustomerPnlReportData
                .Where(r => r.CustomerId == customerId)
                .OrderBy(r => r.Timestamp)
                .LastAsync(cancellationToken);

            var singleBetsReport = await context.CustomerSingleBetsPnlReportData
                .Where(r => r.CustomerId == customerId)
                .OrderBy(r => r.Timestamp)
                .LastAsync(cancellationToken);

            var recalculatedReport = report!.Recalculate(bet);
            var recalculatedSingleBetsReport = singleBetsReport!.Recalculate(bet);

            context.CustomerPnlReportData.Add(recalculatedReport);
            context.CustomerSingleBetsPnlReportData.Add(recalculatedSingleBetsReport);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task GenerateReportAsync(Customer customer, CancellationToken cancellationToken)
        {
            CustomerPnlReport report = new(customer.Id, customer.Username, customer.Timestamp);
            context.CustomerPnlReportData.Add(report);

            CustomerSingleBetsPnlReport singleBetsReport = new(customer.Id, customer.Username, customer.Timestamp);
            context.CustomerSingleBetsPnlReportData.Add(singleBetsReport);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CustomerPnlReport?> GetCustomerPnlAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await context.CustomerPnlReportData
                .Where(r => r.CustomerId == customerId)
                .OrderBy(r => r.Timestamp)
                .LastOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<CustomerPnlReport>> GetCustomerPnlAsync(CancellationToken cancellationToken)
        {
            return await context.CustomerPnlReportData.ToListAsync(cancellationToken);
        }
    }
}
