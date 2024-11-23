using Microsoft.AspNetCore.Mvc;
using web_app_domain;

namespace web_app_performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            var healthStatus = new Health
            {
                Status = "Healthy",
                CheckedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Ok(healthStatus);
        }
    }
}
