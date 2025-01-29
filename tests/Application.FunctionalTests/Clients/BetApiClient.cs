﻿using BetAPI.Models;
using System.Net.Http.Json;
#nullable disable
namespace Application.FunctionalTests.Clients
{
    public class BetApiClient(HttpClient http)
    {
        public async Task<Bet> PlaceBetAsync(Guid customerId, BetType betTypeId, int maxReturns, int totalOdds, int totalStake, bool inPlay)
        {
            var bet = new Bet(Guid.Empty, customerId, false, false, betTypeId, maxReturns, totalOdds, totalStake, inPlay, default);
            var response = await http.PostAsJsonAsync("/bets", bet);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Bet>();
        }

        public async Task ResultBetAsync(object id, bool win)
        {
            var response = await http.PostAsync($"/bets/result/{id}?win={win}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task SettleBetAsync(object id)
        {
            var response = await http.PostAsync($"/bets/settle/{id}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}