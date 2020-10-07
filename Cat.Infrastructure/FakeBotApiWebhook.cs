using System;
using System.Threading.Tasks;
using Cat.Application;
using Cat.Domain;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public class FakeBotApiWebhook : BotApiComponentBase, IBotApiRevisableWebhook<FakeWebhookInfo>
    {
        private readonly ILogger<FakeBotApiWebhook> _logger;
        private readonly FakeBotApiClient _botApiClient;

        private bool _persistentMode;

        // todo: ==== move to separate component with URL composition/management responsibility ====
        private string _webhookUrl = "https://localhost:44336/FakeBotApiEndpoint";
        // todo: ===================================================================================

        public FakeBotApiWebhook(ILogger<FakeBotApiWebhook> logger, FakeBotApiClient botApiClient)
        {
            _logger = logger;
            _botApiClient = botApiClient;
        }

        protected override void InitComponentState()
        {
            base.InitComponentState();
            _persistentMode = false;
        }

        public Task RegisterWebhookAsync()
        {
            return TryConsumeClientAsync(async () => await TrySetWebhookAsync());
        }

        public Task UnregisterWebhookAsync()
        {
            return TryConsumeClientAsync(async () => await TryDeleteWebhookAsync());
        }

        public FakeWebhookInfo WebhookInfo { get; private set; }

        public Task UpdateWebhookInfoAsync()
        {
            return TryConsumeClientAsync(async () => await ApplyWebhookInfoUpdateAsync());
        }

        public Task ReviseWebhookConsistencyAsync()
        {
            return TryConsumeClientAsync(async () => await ReviseWebhookRemoteStateAsync());
        }

        #region Private methods

        private async Task TryConsumeClientAsync(Func<Task> predicate)
        {
            if (BotApiComponentState.IsRegistered(_botApiClient))
            {
                await predicate();
            }
            else
            {
                // todo: apply single text format convention for all Fake Bot API components log messages
                var message = $"TODO log error: client state is invalid: {_botApiClient.ComponentState.State}";
                ComponentState = BotApiComponentState.CreateError(message);
                _logger.LogDebug(message); 
            }
        }

        private async Task TrySetWebhookAsync()
        {
            await _botApiClient.OperationalClient.SetWebhookAsync(_webhookUrl);
            WebhookInfo = await _botApiClient.OperationalClient.GetWebhookInfoAsync();
            _persistentMode = true;
            if (WebhookInfo.Url == _webhookUrl)
            {
                ComponentState = BotApiComponentState.CreateRegistered();
                _logger.LogDebug("TODO log registered"); // todo: apply single text format convention for all Fake Bot API components log messages
            }
            else
            {
                // todo: apply single text format convention for all Fake Bot API components log messages
                var message = $"TODO log error: incorrect webhook url '{WebhookInfo.Url}' must be '{_webhookUrl}'";
                ComponentState = BotApiComponentState.CreateError(message);
                _logger.LogDebug(message); 
            }
        }

        private async Task TryDeleteWebhookAsync()
        {
            await _botApiClient.OperationalClient.DeleteWebhookAsync();
            WebhookInfo = await _botApiClient.OperationalClient.GetWebhookInfoAsync();
            _persistentMode = false;
            if (WebhookInfo.Url != _webhookUrl)
            {
                ComponentState = BotApiComponentState.CreateUnregistered();
                _logger.LogDebug("TODO log unregistered"); // todo: apply single text format convention for all Fake Bot API components log messages
            }
            else
            {
                // todo: apply single text format convention for all Fake Bot API components log messages
                var message = $"TODO log error: incorrect webhook url '{WebhookInfo.Url}' must be '{_webhookUrl}'";
                ComponentState = BotApiComponentState.CreateError(message); 
                _logger.LogDebug(message);
            }
        }

        private async Task ApplyWebhookInfoUpdateAsync()
        {
            WebhookInfo = await _botApiClient.OperationalClient.GetWebhookInfoAsync();
            ApplyStateUpdate();
        }

        private void ApplyStateUpdate()
        {
            if (_persistentMode && WebhookInfo.Url == _webhookUrl && !BotApiComponentState.IsRegistered(this))
            {
                ComponentState = BotApiComponentState.CreateRegistered();
                _logger.LogDebug("TODO log updated state to registered due updated webhook info"); // todo: apply single text format convention for all Fake Bot API components log messages
            }
            else if (!_persistentMode && WebhookInfo.Url != _webhookUrl && !BotApiComponentState.IsUnregistered(this))
            {
                ComponentState = BotApiComponentState.CreateUnregistered();
                _logger.LogDebug("TODO log updated state to unregistered due updated webhook info"); // todo: apply single text format convention for all Fake Bot API components log messages
            }
            else if (_persistentMode && WebhookInfo.Url != _webhookUrl || !_persistentMode && WebhookInfo.Url == _webhookUrl)
            {
                ComponentState = BotApiComponentState.CreateError();
                _logger.LogDebug("TODO log updated state to error due updated webhook info"); // todo: apply single text format convention for all Fake Bot API components log messages
            }
        }

        private async Task ReviseWebhookRemoteStateAsync()
        {
            WebhookInfo = await _botApiClient.OperationalClient.GetWebhookInfoAsync();
            if (_persistentMode && WebhookInfo.Url != _webhookUrl)
            {
                // todo: apply single text format convention for all Fake Bot API components log messages
                _logger.LogDebug($"TODO log trying to set webhook due actual url '{WebhookInfo.Url}' is incorrect (should be registered)");
                await TrySetWebhookAsync();
            } 
            else if (!_persistentMode && WebhookInfo.Url == _webhookUrl)
            {
                // todo: apply single text format convention for all Fake Bot API components log messages
                _logger.LogDebug($"TODO log trying to delete webhook due actual url '{WebhookInfo.Url}' is set to this endpoint (should be unregistered)");
                await TryDeleteWebhookAsync();
            }
            else
            {
                ApplyStateUpdate();
            }
        }

        #endregion
    }
}
