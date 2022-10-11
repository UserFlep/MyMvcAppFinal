namespace MyMvcAppFinal.Services
{
    public interface IStatusService
    {
        public string GetStatus();
        public Task StartAsync(CancellationToken stoppingToken);
        public Task StopAsync(CancellationToken stoppingToken);
        public void Dispose();
    }
    public class StatusService : IStatusService, IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer? _timer = null;

        private string Active { get { return "Активно"; } }
        private string Blocked { get { return "Заблокировано"; } }
        private bool Flag { get; set; }

       
        public string GetStatus()
        {
            return Flag ? Active : Blocked;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(3));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);
            Flag = !Flag;
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
