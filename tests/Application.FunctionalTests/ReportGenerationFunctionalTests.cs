using BetAPI.Models;
using CustomerAPI.Models;
using ReportAPI.Models;

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

            Bet secondBet = await fixture.BetApi.PlaceBetAsync(customer.Id, betTypeId: BetType.Double, maxReturns: 200, totalOdds: 4, totalStake: 35, inPlay: true);
            await fixture.BetApi.ResultBetAsync(id: secondBet.Id, win: false);
            await fixture.BetApi.SettleBetAsync(id: secondBet.Id);

            Bet thirdBet = await fixture.BetApi.PlaceBetAsync(customer.Id, betTypeId: BetType.Triple, maxReturns: 150, totalOdds: 3, totalStake: 100, inPlay: false);
            await fixture.BetApi.ResultBetAsync(id: thirdBet.Id, win: true);
            await fixture.BetApi.SettleBetAsync(id: thirdBet.Id);

            // Act
            CustomerPnlReport report = await fixture.ReportApi.GetCustomerPnlReportAsync(customerId: customer.Id);

            // Assert
            Assert.NotNull(report);
            Assert.Equal(customer.Id, report.CustomerId);
            Assert.Equal("c.margarida", report.Username);
            Assert.Equal(2, report.TotalWins);
            Assert.Equal(1, report.TotalLosses);
            Assert.Equal(100, report.TotalProfit);
            Assert.Equal(35, report.TotalLoss);
            Assert.Equal(65, report.NetProfit);
            Assert.Equal(3, report.TotalBets);
        }
    }
}
