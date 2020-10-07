using System;
using System.Threading.Tasks;
using Cat.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public class FakeBotApiClient : BotApiComponentBase, IBotApiClient<FakeOperationalClient>, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FakeBotApiClient> _logger;

        private IServiceScope _scope; // todo: research how to utilize and dispose IServiceScope correctly

        // todo: ============== move to separate fake component/IOptions<>? ==============
        private string _fakeToken = "SLMX4ga5.t84Q";
        private TimeSpan _webhookTimerInterval = TimeSpan.FromSeconds(10);
        // todo: ==============================================================

        public FakeBotApiClient(IServiceProvider serviceProvider, ILogger<FakeBotApiClient> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task RegisterClientAsync()
        {
            _scope = _serviceProvider.CreateScope();
            var logger = _scope.ServiceProvider.GetRequiredService<ILogger<FakeOperationalClient>>();
            OperationalClient = new FakeOperationalClient(_fakeToken, _webhookTimerInterval, logger);
            if (await OperationalClient.ValidateClientAsync())
            {
                ComponentState = BotApiComponentState.CreateRegistered();
                _logger.LogDebug("TODO log registered"); // todo: apply single text format convention for all Fake Bot API components log messages
            }
            else
            {
                OperationalClient = null;
                // todo: apply single text format convention for all Fake Bot API components log messages
                var message = $"TODO log error {_fakeToken}";
                ComponentState = BotApiComponentState.CreateError(message); 
                _logger.LogDebug(message);
            }
        }

        public Task UnregisterClientAsync()
        {
            OperationalClient = null;
            _scope.Dispose();
            _scope = null;
            ComponentState = BotApiComponentState.CreateUnregistered();
            _logger.LogDebug("TODO log unregistered"); // todo: apply single text format convention for all Fake Bot API components log messages
            return Task.CompletedTask;
        }

        public FakeOperationalClient OperationalClient { get; private set; }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
