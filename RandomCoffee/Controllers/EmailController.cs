using Microsoft.AspNetCore.Mvc;
using RandomCoffee.Services;

namespace RandomCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly TimerService _timerService;

        public EmailController(TimerService timerService)
        {
            _timerService = timerService;
        }

        [HttpGet("matches")]
        public Task GetMatches()
        {
            return _timerService.StartAsync(CancellationToken.None);
        }

        [HttpPut("timer")]
        public async Task<ActionResult> UpdateTimer(int startTime, int interval)
        {
            _timerService.UpdateTimer(startTime, interval);
            
            return Ok();
        }
    }
}
