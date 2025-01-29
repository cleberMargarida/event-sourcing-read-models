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
            var report = await GetCustomerPnlAsync(bet.CustomerId, cancellationToken);

            var recalculatedReport = report!.Recalculate(bet);

            context.CustomerPnlReportData.Add(recalculatedReport);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task GenerateReportAsync(Customer customer, CancellationToken cancellationToken)
        {
            CustomerPnlReport report = new(customer.Id, customer.Username, customer.Timestamp);

            context.CustomerPnlReportData.Add(report);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CustomerPnlReport?> GetCustomerPnlAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await context.CustomerPnlReportData.LastOrDefaultAsync(r => r.CustomerId == customerId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<CustomerPnlReport>> GetCustomerPnlAsync(CancellationToken cancellationToken)
        {
            return await context.CustomerPnlReportData.ToListAsync(cancellationToken);
        }
    }
}
