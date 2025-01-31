using BetAPI.Models;
using ReportAPI.Events;
using Sculptor.Core;
using System.Text.Json.Serialization;

namespace ReportAPI.Models
{
    public partial class CustomerSingleBetsPnlReport : AggregateRoot<Guid>
    {
        public Guid CustomerId { get; protected init; }
        public string Username { get; protected init; }
        public decimal TotalProfit { get; protected set; }
        public decimal TotalLoss { get; protected set; }
        public decimal NetProfit { get; protected set; }
        public int TotalBets { get; protected set; }
        public int TotalWins { get; protected set; }
        public int TotalLosses { get; protected set; }
        public DateTime Timestamp { get; protected set; }

        public CustomerSingleBetsPnlReport(Guid customerId, string username, DateTime timestamp)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Username = username;
            Timestamp = timestamp;
        }

        [JsonConstructor]
        protected CustomerSingleBetsPnlReport(Guid id, Guid customerId, string username, decimal totalProfit, decimal totalLoss, decimal netProfit, int totalBets, int totalWins, int totalLosses, DateTime timestamp)
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

        public CustomerSingleBetsPnlReport Recalculate(Bet bet)
        {
            var other = (CustomerSingleBetsPnlReport)MemberwiseClone();
            other.Id = Guid.NewGuid();

            if (bet.BetTypeId is not BetType.Single)
            {
                return other;
            }
            
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
