using CustomerAPI.Models;
using CustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController(ICustomerService service) : ControllerBase
    {
        // GET: Customers
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var customers = await service.ListCustomersAsync(cancellationToken);

            return Ok(customers);
        }

        // POST: Customers
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer, CancellationToken cancellationToken)
        {
            await service.AddAsync(customer, default);

            return Ok(customer);
        }
    }
}
