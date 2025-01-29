using BetAPI.Models;
using Sculptor.Core;

namespace BetAPI.Events
{
    public class BetSettledEvent(Bet bet) : DomainEvent
    {
        public Bet Bet { get; } = bet;
    }
}
