using BetAPI.Models;
using Sculptor.Core;

namespace BetAPI.Events
{
    public class BetResultedEvent(Bet bet) : DomainEvent
    {
        public Bet Bet { get; } = bet;
    }
}
