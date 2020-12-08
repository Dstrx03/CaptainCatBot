using System;
using System.Linq;
using Cat.Domain;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Cat.Application;
using MediatR;

namespace Cat.Infrastructure
{
    public class FakeBotApiPoller : BotApiPollerBase<FakeBotUpdate>, IDisposable
    {
        private readonly ILogger<FakeBotApiPoller> _logger;
        private readonly FakeBotApiClient _botApiClient;
        private readonly IMediator _mediator;
        private readonly Timer _updatesPollingTimer;

        // todo: ============== move to separate fake component/IOptions<>? ==============
        private TimeSpan _timerInterval = TimeSpan.FromSeconds(10);
        // todo: ==============================================================

        public FakeBotApiPoller(ILogger<FakeBotApiPoller> logger, FakeBotApiClient botApiClient, IMediator mediator)
        {
            _logger = logger;
            _botApiClient = botApiClient;
            _mediator = mediator;
            _updatesPollingTimer = new Timer(HandleUpdatesPollingCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

        public override void RegisterPoller()
        {
            if (_botApiClient.CanConsumeOperationalClient)
            {
                _updatesPollingTimer.Change(0, (int)_timerInterval.TotalMilliseconds);
                ComponentState = BotApiComponentState.CreateRegistered();
                _logger.LogInformation("Fake Bot API Poller registered.");
            }
            else
            {
                var errorMessage = $"Fake Bot API Poller registration failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed.";
                ComponentState = BotApiComponentState.CreateError(errorMessage);
                _logger.LogError(errorMessage);
            }
        }

        public override void UnregisterPoller()
        {
            _updatesPollingTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ComponentState = BotApiComponentState.CreateUnregistered();
            _logger.LogInformation("Fake Bot API Poller unregistered.");
        }

        protected override Task SendUpdateCommand(FakeBotUpdate update) =>
            _mediator.Send(new FakeBotUpdateCommand(update));

        private async void HandleUpdatesPollingCallback(object state) => 
            await PollUpdatesAsync();

        private async Task PollUpdatesAsync()
        {
            try
            {
                var updates = await _botApiClient.ConsumeOperationalClientAsync(_ => _.GetUpdatesAsync(), () =>
                {
                    AbortPollingUpdates();
                    return Task.FromResult(Enumerable.Empty<FakeBotUpdate>());
                });
                foreach (var update in updates)
                    await SendUpdateCommand(update);
            }
            catch (Exception e)
            {
                _logger.LogError($"Fake Bot API Poller the polling operation failed, an exception has occurred.{Environment.NewLine}{e}");
            }
        }

        private void AbortPollingUpdates()
        {
            _updatesPollingTimer.Change(Timeout.Infinite, Timeout.Infinite);
            var errorMessage = $"Fake Bot API Poller updates polling stopped, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed.";
            ComponentState = BotApiComponentState.CreateError(errorMessage);
            _logger.LogError(errorMessage);
        }

        public void Dispose()
        {
            _updatesPollingTimer?.Dispose();
        }
    }
}
