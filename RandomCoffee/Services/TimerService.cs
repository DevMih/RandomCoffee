using RandomCoffee.Data;

namespace RandomCoffee.Services
{
    public class TimerService : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private readonly SmtpService _smptService;
        private readonly MatchingService _matchingService;

        public TimerService(SmtpService smptService, MatchingService matchingService)
        {
            _smptService = smptService;
            _matchingService = matchingService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(100));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var matches = _matchingService.GetMatches().ToList();

            for (int i = 0; i < matches.Count; i++)
            {  
                if (i == matches.Count - 1)
                {
                    _smptService.SendMatches(matches[i], null);
                    break;
                }
                _smptService.SendMatches(matches[i], matches[i + 1]);
                _smptService.SendMatches(matches[i + 1], matches[i]);
                i++;
            }
        }

        public Task UpdateTimer(int startTime, int interval)
        {
            _timer?.Change(TimeSpan.FromSeconds(startTime), TimeSpan.FromSeconds(interval));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}