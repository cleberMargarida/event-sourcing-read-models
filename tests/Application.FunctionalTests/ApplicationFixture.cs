#nullable disable
using Application.FunctionalTests.Clients;

namespace Application.FunctionalTests
{
    public class ApplicationFixture : IAsyncLifetime
    {
        private DistributedApplication app;

        public BetApiClient BetApi { get; private set; }
        public CustomerApiClient CustomerApi { get; private set; }
        public ReportApiClient ReportApi { get; private set; }

        public async Task InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Web_AppHost>();

            builder.Services.ConfigureHttpClientDefaults(clientBuilder => clientBuilder.ConfigureHttpClient(builder => builder.Timeout = Timeout.InfiniteTimeSpan));

            app = await builder.BuildAsync();

            await app.StartAsync();

            BetApi = new(app.CreateHttpClient("betapi"));
            CustomerApi = new(app.CreateHttpClient("customerapi"));
            ReportApi = new(app.CreateHttpClient("reportapi"));

            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

            await resourceNotificationService.WaitForResourceAsync("mssql").WaitAsync(TimeSpan.FromSeconds(50));
            await resourceNotificationService.WaitForResourceHealthyAsync("customerapi").WaitAsync(TimeSpan.FromSeconds(50));
            await resourceNotificationService.WaitForResourceHealthyAsync("reportapi").WaitAsync(TimeSpan.FromSeconds(50));

            await BetApi.CheckHealthAsync();
            await CustomerApi.CheckHealthAsync();
            await ReportApi.CheckHealthAsync();
        }

        public async Task DisposeAsync()
        {
            await app.DisposeAsync();
        }
    }
}