﻿using Cat.Application;
using Cat.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Infrastructure
{
    public class FakeBotApiWebhook : BotApiComponentBase, IBotApiRevisableWebhook<FakeWebhookInfo>
    {
        private readonly ILogger<FakeBotApiWebhook> _logger;
        private readonly FakeBotApiClient _botApiClient;

        private bool _persistentMode;
        private bool _unknownWebhookInfo;

        // todo: ==== move to separate component with URL composition/management responsibility ====
        private string _webhookUrl = "https://localhost:44336/FakeBotApiEndpoint";
        // todo: ===================================================================================

        public FakeBotApiWebhook(ILogger<FakeBotApiWebhook> logger, FakeBotApiClient botApiClient)
        {
            _logger = logger;
            _botApiClient = botApiClient;

            _persistentMode = false;
            _unknownWebhookInfo = false;
        }

        private bool WebhookInfoIsUnknown => _unknownWebhookInfo && WebhookInfo == null;
        private bool WebhookUrlIsArranged => string.Equals(WebhookInfo?.Url, _webhookUrl, StringComparison.InvariantCultureIgnoreCase);
        private bool PersistentButUrlIsNotArranged => _persistentMode && !WebhookUrlIsArranged;
        private bool NotPersistentButUrlIsArranged => !_persistentMode && WebhookUrlIsArranged;
        private bool IsRegisteredByInternalState => _persistentMode && WebhookUrlIsArranged;
        private bool IsUnregisteredByInternalState => !_persistentMode && !WebhookUrlIsArranged;
        private bool IsIncorrectWebhookInfoState => _unknownWebhookInfo && WebhookInfo != null || !_unknownWebhookInfo && WebhookInfo == null;
        private bool IsIncorrectInternalState => PersistentButUrlIsNotArranged || NotPersistentButUrlIsArranged || IsIncorrectWebhookInfoState;

        public Task RegisterWebhookAsync() => RegisterWebhookAsync(bypassAppliedNonErrorUpdate: false, ("registration", specifyOperationName: false));

        private async Task RegisterWebhookAsync(bool bypassAppliedNonErrorUpdate, (string initialOperationName, bool specifyOperationName) messagesConfiguration)
        {
            try
            {
                if (!await HandlingConsumeOperationalClientConfirmableAsync(
                    _ => _.SetWebhookAsync(_webhookUrl),
                    $"Fake Bot API Webhook registration (set Webhook) failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."))
                    return;
            }
            catch (Exception e)
            {
                const string errorMessage = "Fake Bot API Webhook registration (set Webhook) failed, an exception has occurred";
                ComponentState = BotApiComponentState.CreateError($"{errorMessage}: {e.Message}");
                _logger.LogError($"{errorMessage}.{Environment.NewLine}{e}");
                return;
            }

            _persistentMode = true;

            if (!await ApplyUpdateWebhookInfoAsync(
                "Fake Bot API Webhook registration (update Webhook Info) failed, an exception has occurred",
                $"Fake Bot API Webhook registration (update Webhook Info) failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."))
                return;

            ApplyComponentStateUpdate(bypassAppliedNonErrorUpdate, messagesConfiguration);
        }

        public Task UnregisterWebhookAsync() => UnregisterWebhookAsync(bypassAppliedNonErrorUpdate: false, ("unregistration", specifyOperationName: false));

        private async Task UnregisterWebhookAsync(bool bypassAppliedNonErrorUpdate, (string initialOperationName, bool specifyOperationName) messagesConfiguration)
        {
            try
            {
                if (!await HandlingConsumeOperationalClientConfirmableAsync(
                    _ => _.DeleteWebhookAsync(),
                    $"Fake Bot API Webhook unregistration (delete Webhook) failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."))
                    return;
            }
            catch (Exception e)
            {
                const string errorMessage = "Fake Bot API Webhook unregistration (delete Webhook) failed, an exception has occurred";
                ComponentState = BotApiComponentState.CreateError($"{errorMessage}: {e.Message}");
                _logger.LogError($"{errorMessage}.{Environment.NewLine}{e}");
                return;
            }

            _persistentMode = false;

            if (!await ApplyUpdateWebhookInfoAsync(
                "Fake Bot API Webhook unregistration (update Webhook Info) failed, an exception has occurred",
                $"Fake Bot API Webhook unregistration (update Webhook Info) failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."))
                return;

            ApplyComponentStateUpdate(bypassAppliedNonErrorUpdate, messagesConfiguration);
        }

        public FakeWebhookInfo WebhookInfo { get; private set; }

        public async Task UpdateWebhookInfoAsync()
        {
            if (!await ApplyUpdateWebhookInfoAsync(
                "Fake Bot API Webhook update Webhook Info failed, an exception has occurred", 
                $"Fake Bot API Webhook update Webhook Info failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed.")) 
                return;

            ApplyComponentStateUpdate(bypassAppliedNonErrorUpdate: true, ("update Webhook Info", specifyOperationName: true));
        }

        public async Task ReviseWebhookConsistencyAsync()
        {
            if (!await ApplyUpdateWebhookInfoAsync(
                "Fake Bot API Webhook consistency revision (update Webhook Info) failed, an exception has occurred",
                $"Fake Bot API Webhook consistency revision (update Webhook Info) failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."))
                return;

            if (PersistentButUrlIsNotArranged)
            {
                const string message = "Fake Bot API Webhook consistency revision revealed that the component is actually not registered but it should be according to the persistent mode. Invoking registration operation...";
                _logger.LogInformation($"{message}{Environment.NewLine}{InternalStateDetailsMessage()}");
                await RegisterWebhookAsync(bypassAppliedNonErrorUpdate: false, ("consistency revision", specifyOperationName: true));
            }
            else if (NotPersistentButUrlIsArranged)
            {
                const string message = "Fake Bot API Webhook consistency revision revealed that the component is actually registered but it should not be according to the non-persistent mode. Invoking unregistration operation...";
                _logger.LogInformation($"{message}{Environment.NewLine}{InternalStateDetailsMessage()}");
                await UnregisterWebhookAsync(bypassAppliedNonErrorUpdate: false, ("consistency revision", specifyOperationName: true));
            }
            else
                ApplyComponentStateUpdate(bypassAppliedNonErrorUpdate: true, ("consistency revision", specifyOperationName: true));
        }

        private Task<bool> HandlingConsumeOperationalClientConfirmableAsync(Func<FakeOperationalClient, Task> actionAsync, string nonConsumableClientErrorMessage) =>
            _botApiClient.ConsumeOperationalClientConfirmableAsync(actionAsync, () => HandleNonConsumableClient(nonConsumableClientErrorMessage));

        private Task<bool> ApplyUpdateWebhookInfoAsync(string unknownWebhookInfoErrorMessage, string nonConsumableClientErrorMessage) =>
            ApplyUpdateWebhookInfoAsync(e => HandleUnknownWebhookInfo(e, unknownWebhookInfoErrorMessage), () => HandleNonConsumableClient(nonConsumableClientErrorMessage));

        private async Task<bool> ApplyUpdateWebhookInfoAsync(Action<Exception> unknownWebhookInfoHandlingAction, Action nonConsumableClientHandlingAction)
        {
            try
            {
                WebhookInfo = await _botApiClient.ConsumeOperationalClientAsync(_ => _.GetWebhookInfoAsync(), () =>
                {
                    nonConsumableClientHandlingAction?.Invoke();
                    return Task.FromResult<FakeWebhookInfo>(null);
                });
                _unknownWebhookInfo = WebhookInfo == null;
            }
            catch (Exception e)
            {
                _unknownWebhookInfo = true;
                WebhookInfo = null;
                unknownWebhookInfoHandlingAction?.Invoke(e);
            }

            return !_unknownWebhookInfo;
        }

        private void ApplyComponentStateUpdate(bool bypassAppliedNonErrorUpdate, (string initialOperationName, bool specifyOperationName) messagesConfiguration)
        {
            if (WebhookInfoIsUnknown)
            {
                var errorMessage = $"Fake Bot API Webhook {messagesConfiguration.initialOperationName} failed, the Webhook Info is unknown.";
                ComponentState = BotApiComponentState.CreateUnknown(errorMessage);
                _logger.LogError(errorMessage);
            }
            else if (IsIncorrectInternalState && !BotApiComponentState.IsUnknown(this))
            {
                var errorMessage = IncorrectInternalStateErrorMessage(messagesConfiguration.initialOperationName);
                ComponentState = BotApiComponentState.CreateError(errorMessage);
                _logger.LogError($"{errorMessage}{Environment.NewLine}{InternalStateDetailsMessage()}");
            }
            else if (IsRegisteredByInternalState)
            {
                if (bypassAppliedNonErrorUpdate && BotApiComponentState.IsRegistered(this)) return;
                ComponentState = BotApiComponentState.CreateRegistered();
                var errorMessage = !messagesConfiguration.specifyOperationName ? 
                    "Fake Bot API Webhook registered." : 
                    $"Fake Bot API Webhook registered by the {messagesConfiguration.initialOperationName} operation.";
                _logger.LogInformation(errorMessage);
            }
            else if (IsUnregisteredByInternalState)
            {
                if (bypassAppliedNonErrorUpdate && BotApiComponentState.IsUnregistered(this)) return;
                ComponentState = BotApiComponentState.CreateUnregistered();
                var errorMessage = !messagesConfiguration.specifyOperationName ? 
                    "Fake Bot API Webhook unregistered." : 
                    $"Fake Bot API Webhook unregistered by the {messagesConfiguration.initialOperationName} operation.";
                _logger.LogInformation(errorMessage);
            }
        }

        private void HandleUnknownWebhookInfo(Exception e, string errorMessage)
        {
            ComponentState = BotApiComponentState.CreateUnknown($"{errorMessage}: {e.Message}");
            _logger.LogError($"{errorMessage}.{Environment.NewLine}{e}");
        }

        private void HandleNonConsumableClient(string errorMessage)
        {
            if (_persistentMode && !_unknownWebhookInfo)
            {
                _unknownWebhookInfo = true;
                WebhookInfo = null;
            }

            ComponentState = WebhookInfoIsUnknown ?
                BotApiComponentState.CreateUnknown(errorMessage) :
                BotApiComponentState.CreateError(errorMessage);
            _logger.LogError(errorMessage);
        }

        private string IncorrectInternalStateErrorMessage(string operationName)
        {
            string errorDetails = null;
            if (IsIncorrectWebhookInfoState)
                errorDetails = "The Webhook Info data is incorrect or outdated.";
            else if (PersistentButUrlIsNotArranged)
                // todo: null/empty formatting implemented in unified logs formatting component
                errorDetails = $"The mode is persistent while Webhook Info url ({(WebhookInfo != null ? WebhookInfo.Url.Bar() : "*null*")}) do not match the actual webhook url ({_webhookUrl.Bar()}).";
            else if (NotPersistentButUrlIsArranged)
                errorDetails = $"The mode is non-persistent while Webhook Info url ({(WebhookInfo != null ? WebhookInfo.Url.Bar() : "*null*")}) do match the actual webhook url ({_webhookUrl.Bar()}).";

            return $"Fake Bot API Webhook {operationName} failed, internal state is incorrect: {errorDetails}";
        }

        private string InternalStateDetailsMessage() => // todo: name?, content
            FooBar("Fake Bot API Webhook internal state details", new (string, object)[] {
                ("Persistent mode", _persistentMode),
                ("Webhook Info is unknown", _unknownWebhookInfo),
                ("Webhook Info is null", WebhookInfo == null),
                ("Webhook url", _webhookUrl.Bar()),
                ("Webhook Info url", WebhookInfo != null ? WebhookInfo.Url.Bar() : "*null*"),
                ("...", "*empty*") // todo: null/empty formatting implemented in unified logs formatting component
            });




        public static string FooBar(string detailsTitle, (string title, object value)[] details) // todo: name, params, design, move to special logs formatting component
        {
            var stringBuilder = new StringBuilder($"{detailsTitle}.{Environment.NewLine}");
            for (var i = 0; i < details.Length; i++)
            {
                var (title, value) = details[i];
                stringBuilder.Append($"{null,3}{title}: {value}{(i == details.Length - 1 ? string.Empty : Environment.NewLine)}");
            }
            return stringBuilder.ToString();
        }




    }
}
