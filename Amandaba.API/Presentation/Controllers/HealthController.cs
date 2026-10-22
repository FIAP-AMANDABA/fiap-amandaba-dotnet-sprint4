using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Amandaba.Presentation.Controllers
{
    [Route("api/health")]
    [ApiController]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthService;

        public HealthController(HealthCheckService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet("live")]
        public async Task<IActionResult> Live(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(
                r => r.Tags.Contains("live"),
                ct
            );

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    result
                );
        }

        [HttpGet("db")]
        public async Task<IActionResult> Ready(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(
                r => r.Tags.Contains("db"),
                ct
            );

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    result
                );
        }
    }
}