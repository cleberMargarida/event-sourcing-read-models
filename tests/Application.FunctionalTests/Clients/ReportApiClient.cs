using ReportAPI.Models;
using System.Net.Http.Json;
#nullable disable
namespace Application.FunctionalTests.Clients
{
    public class ReportApiClient(HttpClient http)
    {
        public async Task<bool> CheckHealthAsync()
        {
            var response = await http.GetAsync("/health");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CustomerPnlReport>> GetCustomerPnlReportAsync()
        {
            var response = await http.GetAsync($"/customerpnl");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CustomerPnlReport>>();
        }
        
        public async Task<CustomerPnlReport> GetCustomerPnlReportAsync(object customerId)
        {
            var response = await http.GetAsync($"/customerpnl/{customerId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerPnlReport>();
        }
    }
}