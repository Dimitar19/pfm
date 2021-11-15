using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pfm.Models;

namespace pfm.Controllers
{
    [ApiController]
    [Route("/spending-analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;
        public AnalyticsController(ILogger<AnalyticsController> logger){
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetSpendingAnalytics([FromQuery] string catcode, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] Directions direction){
            return Ok();
        }
    }
}