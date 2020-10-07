using System;
using Cat.Domain;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Cat.Application;
using MediatR;

namespace Cat.Infrastructure
{
    public class FakeBotApiPoller : BotApiComponentBase, IBotApiPoller
    {
        private readonly ILogger<FakeBotApiPoller> _logger;
        private readonly IMediator _mediator;
        private readonly FakeBotApiClient _botApiClient;
        private readonly Timer _timer;

        // todo: ============== move to separate fake component/IOptions<>? ==============
        private TimeSpan _timerInterval = TimeSpan.FromSeconds(10);
        // todo: ==============================================================

        public FakeBotApiPoller(ILogger<FakeBotApiPoller> logger, IMediator mediator, FakeBotApiClient botApiClient)
        {
            _logger = logger;
            _mediator = mediator;
            _botApiClient = botApiClient;
            _timer = new Timer(async s => await GetUpdatesAsync(), null, Timeout.Infinite, Timeout.Infinite);
        }

        public void RegisterPoller()
        {
            if (BotApiComponentState.IsRegistered(_botApiClient))
            {
                _timer.Change(0, (int)_timerInterval.TotalMilliseconds);
                ComponentState = BotApiComponentState.CreateRegistered();
                _logger.LogDebug("TODO log registered"); // todo: apply single text format convention for all Fake Bot API components log messages
            }
            else
            {
                // todo: apply single text format convention for all Fake Bot API components log messages
                var message = $"TODO log error: client state is invalid: {_botApiClient.ComponentState.State}";
                ComponentState = BotApiComponentState.CreateError(message);
                _logger.LogDebug(message);
            }
        }

        public void UnregisterPoller()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            ComponentState = BotApiComponentState.CreateUnregistered();
            _logger.LogDebug("TODO log unregistered"); // todo: apply single text format convention for all Fake Bot API components log messages
        }

        private async Task GetUpdatesAsync()
        {
            if (BotApiComponentState.IsRegistered(_botApiClient))
            {
                var updates = await _botApiClient.OperationalClient.GetUpdatesAsync();
                foreach (var update in updates) 
                    await _mediator.Send(new FakeBotUpdateCommand(update));
            }
            else
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                // todo: apply single text format convention for all Fake Bot API components log messages
                var message = $"TODO log error: client state is invalid: {_botApiClient.ComponentState.State}, stop polling updates";
                ComponentState = BotApiComponentState.CreateError(message);
                _logger.LogDebug(message);
            }
        }
    }
}
