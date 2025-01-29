using BetAPI.Data;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BetAPI.Services
{
    public class BetService(BetContext context) : IBetService
    {
        public async Task<Bet?> FindAsync(long id)
        {
            return await context.Bets.FindAsync(id);
        }

        public async Task<IReadOnlyCollection<Bet>> ListBetsAsync(CancellationToken cancellationToken)
        {
            return await context.Bets.ToListAsync(cancellationToken);
        }

        public async Task<Bet> AddAsync(Bet bet, CancellationToken cancellationToken)
        {
            try
            {

                context.Add(bet);

                await context.SaveChangesAsync(cancellationToken);

                return bet;
            }
            catch (Exception)
            {

                throw;
            }

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
