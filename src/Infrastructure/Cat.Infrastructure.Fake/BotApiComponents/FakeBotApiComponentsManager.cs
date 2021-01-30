using Cat.Domain;
using Cat.Domain.BotApiComponents.Component;
using Cat.Domain.BotApiComponents.ComponentsManager;
using Cat.Domain.BotApiComponents.Endpoint;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents
{
    public class FakeBotApiComponentsManager : IBotApiComponentsManager
    {
        private readonly ILogger<FakeBotApiComponentsManager> _logger;

        private readonly FakeBotApiClient _botApiClient;
        private readonly IEnumerable<IBotApiEndpoint> _botApiEndpoints;
        private readonly FakeBotApiWebhook _botApiWebhook;
        private readonly FakeBotApiPoller _botApiPoller;

        public FakeBotApiComponentsManager(
            ILogger<FakeBotApiComponentsManager> logger,
            FakeBotApiClient botApiClient,
            IEnumerable<IBotApiEndpoint> botApiEndpoints,
            FakeBotApiWebhook botApiWebhook,
            FakeBotApiPoller botApiPoller)
        {
            _logger = logger;

            _botApiClient = botApiClient;
            _botApiEndpoints = botApiEndpoints;
            _botApiWebhook = botApiWebhook;
            _botApiPoller = botApiPoller;

            RegisterAtApplicationStart = true;
        }

        public BotApiComponentDescriptor ComponentDescriptor =>
            BotApiComponentDescriptor.Fake;

        public bool RegisterAtApplicationStart { get; }
        public bool RegisterAtApplicationStartAfterAppHost => true;

        public async Task RegisterComponentsAsync()
        {
            await _botApiClient.RegisterClientAsync();
            var botApiEndpoint = _botApiEndpoints
                .Single(_ => _.ComponentDescriptor == BotApiComponentDescriptor.Fake);
            botApiEndpoint.RegisterEndpoint();
            await _botApiWebhook.RegisterWebhookAsync();
            _botApiPoller.RegisterPoller();

            _logger.LogDebug(GetComponentsDetailsString("FakeBotApiComponentsManager Register completed", _botApiClient, botApiEndpoint, _botApiWebhook, _botApiPoller));
        }

        public async Task UnregisterComponentsAsync()
        {
            _botApiPoller.UnregisterPoller();
            await _botApiWebhook.UnregisterWebhookAsync();
            var botApiEndpoint = _botApiEndpoints
                .Single(_ => _.ComponentDescriptor == BotApiComponentDescriptor.Fake);
            botApiEndpoint.UnregisterEndpoint();
            await _botApiClient.UnregisterClientAsync();

            _logger.LogDebug(GetComponentsDetailsString("FakeBotApiComponentsManager Unregister completed", _botApiClient, botApiEndpoint, _botApiWebhook, _botApiPoller));
        }

        private string GetComponentsDetailsString(string title, FakeBotApiClient botApiClient, IBotApiEndpoint botApiEndpoint, FakeBotApiWebhook botApiWebhook, FakeBotApiPoller botApiPoller) =>
            TodoMsgFmtr.DetailsMessage(title, new (string title, object value)[]
            {
                ("Fake Bot API Client", botApiClient.ComponentState.FooBar()),
                ("Fake Bot API Endpoint", botApiEndpoint.ComponentState.FooBar()),
                ("Fake Bot API Webhook", botApiWebhook.ComponentState.FooBar()),
                ("Fake Bot API Webhook URL", botApiWebhook.WebhookInfo.BarBar(_ => _.Url)),
                ("Fake Bot API Poller", botApiPoller.ComponentState.FooBar())
            });
    }
}
