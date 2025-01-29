using BetAPI.Models;
using BetAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BetsController(IBetService service) : ControllerBase
    {
        // GET: Bets
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var bets = await service.ListBetsAsync(cancellationToken);

            return Ok(bets);
        }

        // POST: Bets/Create
        [HttpPost]
        public async Task<IActionResult> Create(Bet bet, CancellationToken cancellationToken)
        {
            await service.AddAsync(bet, cancellationToken);

            return Ok(bet);
        }

        [HttpPost("result/{id}")]
        public async Task<IActionResult> ResultBet(long id, bool win, CancellationToken cancellationToken)
        {
            var bet = await service.FindAsync(id);

            if (bet is null)
            {
                return NotFound();
            }

            await service.ResultBetAsync(bet, win, cancellationToken);

            return Ok();
        }

        [HttpPost("settle/{id}")]
        public async Task<IActionResult> SettleBet(long id, CancellationToken cancellationToken)
        {
            var bet = await service.FindAsync(id);

            if (bet is null)
            {
                return NotFound();
            }

            await service.SettleBetAsync(bet, cancellationToken);

            return Ok();
        }
    }
}
