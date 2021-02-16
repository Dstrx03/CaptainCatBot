using Cat.Domain;
using Cat.Domain.BotApiComponents.Component;
using Cat.Domain.BotApiComponents.ComponentsLocator;
using Cat.Domain.BotApiComponents.ComponentsManager;
using Cat.Domain.BotApiComponents.Endpoint;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents
{
    public class FakeBotApiComponentsManager : IBotApiComponentsManager
    {
        private readonly ILogger<FakeBotApiComponentsManager> _logger;

        public FakeBotApiComponentsManager(ILogger<FakeBotApiComponentsManager> logger)
        {
            _logger = logger;
        }

        public BotApiComponentDescriptor ComponentDescriptor =>
            BotApiComponentDescriptor.Fake;

        public bool RegisterAtApplicationStart => true;
        public bool RegisterAtApplicationStartAfterAppHost => true;

        public async Task RegisterComponentsAsync(IBotApiComponentsLocator botApiComponentsLocator)
        {
            // todo: implement components management via Strategy pattern

            var botApiClient = botApiComponentsLocator.GetComponentByDescriptor<FakeBotApiClient>(ComponentDescriptor);
            var botApiEndpoint = botApiComponentsLocator.GetComponentByDescriptor<IBotApiEndpoint>(ComponentDescriptor);
            var botApiWebhook = botApiComponentsLocator.GetComponentByDescriptor<FakeBotApiWebhook>(ComponentDescriptor);
            var botApiPoller = botApiComponentsLocator.GetComponentByDescriptor<FakeBotApiPoller>(ComponentDescriptor);

            await botApiClient.RegisterClientAsync();
            botApiEndpoint.RegisterEndpoint();
            await botApiWebhook.RegisterWebhookAsync();
            botApiPoller.RegisterPoller();

            _logger.LogDebug(GetComponentsDetailsString("FakeBotApiComponentsManager RegisterComponentsAsync completed", botApiClient, botApiEndpoint, botApiWebhook, botApiPoller));
        }

        public async Task UnregisterComponentsAsync(IBotApiComponentsLocator botApiComponentsLocator)
        {
            var botApiClient = botApiComponentsLocator.GetComponentByDescriptor<FakeBotApiClient>(ComponentDescriptor);
            var botApiEndpoint = botApiComponentsLocator.GetComponentByDescriptor<IBotApiEndpoint>(ComponentDescriptor);
            var botApiWebhook = botApiComponentsLocator.GetComponentByDescriptor<FakeBotApiWebhook>(ComponentDescriptor);
            var botApiPoller = botApiComponentsLocator.GetComponentByDescriptor<FakeBotApiPoller>(ComponentDescriptor);

            botApiPoller.UnregisterPoller();
            await botApiWebhook.UnregisterWebhookAsync();
            botApiEndpoint.UnregisterEndpoint();
            await botApiClient.UnregisterClientAsync();

            _logger.LogDebug(GetComponentsDetailsString("FakeBotApiComponentsManager UnregisterComponentsAsync completed", botApiClient, botApiEndpoint, botApiWebhook, botApiPoller));
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
