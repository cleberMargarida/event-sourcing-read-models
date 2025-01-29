using BetAPI.Events;
using Sculptor.Core;

namespace BetAPI.Models
{
    public class Bet : AggregateRoot<Guid>
    {
        public Guid CustomerId { get; private init; }
        public bool Win { get; private set; }
        public bool IsSettled { get; private set; }
        public BetType BetTypeId { get; private init; }
        public decimal TotalStake { get; private init; }
        public decimal MaxReturns { get; private init; }
        public decimal TotalOdds { get; private init; }
        public bool InPlay { get; set; }
        public DateTime Timestamp { get; private set; }

        public Bet(Guid id, Guid customerId, bool win, bool isSettled, BetType betTypeId, decimal totalStake, decimal maxReturns, decimal totalOdds, bool inPlay, DateTime timestamp)
        {
            Id = id;
            CustomerId = customerId;
            Win = win;
            IsSettled = isSettled;
            BetTypeId = betTypeId;
            TotalStake = totalStake;
            MaxReturns = maxReturns;
            TotalOdds = totalOdds;
            InPlay = inPlay;
            Timestamp = timestamp;

            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
                Timestamp = DateTime.UtcNow;
                AddEvent(new BetPlacedEvent(this));
            }
        }

        public void Result(bool win)
        {
            Win = win;
            Timestamp = DateTime.UtcNow;
            AddEvent(new BetResultedEvent(this));
        }

        public void Settle()
        {
            IsSettled = true;
            Timestamp = DateTime.UtcNow;
            AddEvent(new BetSettledEvent(this));
        }
    }
}
