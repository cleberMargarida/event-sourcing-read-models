using BetAPI.Models;
using CustomerAPI.Models;
using ReportAPI.Models;

namespace ReportAPI.Services
{
    public interface IReportService
    {
        Task<IReadOnlyCollection<CustomerPnlReport>> GetCustomerPnlAsync(CancellationToken cancellationToken);
        Task<CustomerPnlReport?> GetCustomerPnlAsync(Guid customerId, CancellationToken cancellationToken);
        Task GenerateReportAsync(Bet bet, CancellationToken cancellationToken);
        Task GenerateReportAsync(Customer customer, CancellationToken cancellationToken);
    }
}
