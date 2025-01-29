using ReportAPI.Models;
using System.Net.Http.Json;
#nullable disable
namespace Application.FunctionalTests.Clients
{
    public class ReportApiClient(HttpClient http)
    {
        public async Task<List<CustomerPnlReport>> GetCustomerPnlReportAsync(object customerId)
        {
            var response = await http.GetAsync($"customerpnl/{customerId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CustomerPnlReport>>();
        }
    }
}