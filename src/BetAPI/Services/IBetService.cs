
using BetAPI.Models;

namespace BetAPI.Services
{
    public interface IBetService
    {
        Task<Bet> AddAsync(Bet bet, CancellationToken cancellationToken);
        Task<Bet?> FindAsync(Guid id);
        Task<IReadOnlyCollection<Bet>> ListBetsAsync(CancellationToken cancellationToken);
        Task ResultBetAsync(Bet bet, bool win, CancellationToken cancellationToken);
        Task SettleBetAsync(Bet bet, CancellationToken cancellationToken);
    }
}
