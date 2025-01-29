using ReportAPI.Models;
using Sculptor.Core;

namespace ReportAPI.Events
{
    public class CustomerPnlReportCreatedEvent(CustomerPnlReport customerPnlReport) : DomainEvent
    {
        public CustomerPnlReport CustomerPnlReport { get; } = customerPnlReport;
    }
}