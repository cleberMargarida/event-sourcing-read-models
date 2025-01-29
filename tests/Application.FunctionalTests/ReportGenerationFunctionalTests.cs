using BetAPI.Models;
using CustomerAPI.Models;

namespace Application.FunctionalTests
{
    public class ReportGenerationFunctionalTests(ApplicationFixture fixture) : IClassFixture<ApplicationFixture>
    {
        [Fact]
        public async Task CanGenerateReport()
        {
            // Arrange
            Customer customer = await fixture.CustomerApi.CreateCustomerAsync(username: "c.margarida", firstName: "Cleber", lastName: "Margarida");

            Bet firstBet = await fixture.BetApi.PlaceBetAsync(customer.Id, betTypeId: BetType.Single, maxReturns: 100, totalOdds: 2, totalStake: 50, inPlay: true);
            await fixture.BetApi.ResultBetAsync(id: firstBet.Id, win: true);
            await fixture.BetApi.SettleBetAsync(id: firstBet.Id);

            Bet secondBet = await fixture.BetApi.PlaceBetAsync(customer.Id, betTypeId: BetType.Double, maxReturns: 200, totalOdds: 4, totalStake: 50, inPlay: true);
            await fixture.BetApi.ResultBetAsync(id: secondBet.Id, win: true);
            await fixture.BetApi.SettleBetAsync(id: secondBet.Id);

            Bet thirdBet = await fixture.BetApi.PlaceBetAsync(customer.Id, betTypeId: BetType.Triple, maxReturns: 150, totalOdds: 3, totalStake: 100, inPlay: false);
            await fixture.BetApi.ResultBetAsync(id: thirdBet.Id, win: true);
            await fixture.BetApi.SettleBetAsync(id: thirdBet.Id);

            // Act
            var report = await fixture.ReportApi.GetCustomerPnlReportAsync(customerId: customer.Id);

            // Assert
            Assert.NotNull(report);
        }
    }
}
