using CustomerAPI.Models;
using System.Net.Http.Json;
#nullable disable
namespace Application.FunctionalTests.Clients
{
    public class CustomerApiClient(HttpClient http)
    {
        public async Task<bool> CheckHealthAsync()
        {
            var response = await http.GetAsync("/health");
            return response.IsSuccessStatusCode;
        }

        public async Task<Customer> CreateCustomerAsync(string username, string firstName, string lastName)
        {
            var response = await http.PostAsJsonAsync("/customers", new { username, firstName, lastName });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Customer>();
        }
    }
}