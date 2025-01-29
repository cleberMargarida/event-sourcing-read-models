using BetAPI.Models;
using ReportAPI.Events;
using Sculptor.Core;
using System.Text.Json.Serialization;

namespace ReportAPI.Models
{
    public partial class CustomerPnlReport : AggregateRoot<Guid>
    {
        public Guid CustomerId { get; private init; }
        public string Username { get; private init; }
        public decimal TotalProfit { get; private set; }
        public decimal TotalLoss { get; private set; }
        public decimal NetProfit { get; private set; }
        public int TotalBets { get; private set; }
        public int TotalWins { get; private set; }
        public int TotalLosses { get; private set; }
        public DateTime Timestamp { get; private set; }

        public CustomerPnlReport(Guid customerId, string username, DateTime timestamp)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Username = username;
            Timestamp = timestamp;
            AddEvent(new CustomerPnlReportCreatedEvent(this));
        }

        [JsonConstructor]
        private CustomerPnlReport(Guid id, Guid customerId, string username, decimal totalProfit, decimal totalLoss, decimal netProfit, int totalBets, int totalWins, int totalLosses, DateTime timestamp)
        {
            Id = id;
            CustomerId = customerId;
            Username = username;
            TotalProfit = totalProfit;
            TotalLoss = totalLoss;
            NetProfit = netProfit;
            TotalBets = totalBets;
            TotalWins = totalWins;
            TotalLosses = totalLosses;
            Timestamp = timestamp;
        }

        public CustomerPnlReport Recalculate(Bet bet)
        {
            var other = (CustomerPnlReport)MemberwiseClone();
            other.Id = Guid.NewGuid();

            if (bet.Win)
            {
                other.TotalProfit += bet.MaxReturns - bet.TotalStake;
                other.TotalWins++;
            }
            else
            {
                other.TotalLoss += bet.TotalStake;
                other.TotalLosses++;
            }

            other.NetProfit = other.TotalProfit - other.TotalLoss;
            other.TotalBets++;
            other.Timestamp = bet.Timestamp;

            return other;
        }
    }
}
