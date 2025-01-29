using CustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using ReportAPI.Models;
using ReportAPI.Services;

namespace ReportAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerPnlController(IReportService reportService) : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> GetCustomerPnl(CancellationToken cancellationToken)
        {
            var reports = await reportService.GetCustomerPnlAsync(cancellationToken);

            return Ok(reports);
        }
        
        [HttpGet("/{userId}")]
        public async Task<IActionResult> GetCustomerPnl(Guid userId, CancellationToken cancellationToken)
        {
            var reports = await reportService.GetCustomerPnlAsync(userId, cancellationToken);

            return Ok(reports);
        }
    }
}
