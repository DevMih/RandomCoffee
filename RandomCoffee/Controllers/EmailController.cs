using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomCoffee.Services;

namespace RandomCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class EmailController : ControllerBase
    {
        private readonly TimerService _timerService;

        public EmailController(TimerService timerService)
        {
            _timerService = timerService;
        }

        [HttpGet("timer/start")]
        public Task StartTimer()
        {
            return _timerService.StartAsync(CancellationToken.None);
        }

        [HttpPut("timer")]
        public async Task<ActionResult> UpdateTimer(int startTime, int interval)
        {
            _timerService.UpdateTimer(startTime, interval);
            
            return Ok();
        }

        [HttpGet("timer/stop")]
        public Task StopTimer()
        {
            return _timerService.StopAsync(CancellationToken.None);
        }
    }
}
