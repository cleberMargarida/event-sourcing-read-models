using BetAPI.Data;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BetAPI.Services
{
    public class BetService(BetContext context) : IBetService
    {
        public async Task<Bet?> FindAsync(Guid id)
        {
            return await context.Bets.FindAsync(id);
        }

        public async Task<IReadOnlyCollection<Bet>> ListBetsAsync(CancellationToken cancellationToken)
        {
            return await context.Bets.ToListAsync(cancellationToken);
        }

        public async Task<Bet> AddAsync(Bet bet, CancellationToken cancellationToken)
        {
            context.Add(bet);

            await context.SaveChangesAsync(cancellationToken);

            return bet;
        }

        public async Task ResultBetAsync(Bet bet, bool win, CancellationToken cancellationToken)
        {
            bet.Result(win);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task SettleBetAsync(Bet bet, CancellationToken cancellationToken)
        {
            bet.Settle();

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
