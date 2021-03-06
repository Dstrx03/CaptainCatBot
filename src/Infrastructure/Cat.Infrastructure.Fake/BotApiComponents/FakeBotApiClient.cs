﻿using Cat.Domain;
using Cat.Domain.BotApiComponents.Client;
using Cat.Domain.BotApiComponents.Component;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents
{
    public class FakeBotApiClient : BotApiClientBase<FakeOperationalClient>, IDisposable
    {
        private readonly ILogger<FakeBotApiClient> _logger;
        private readonly IFakeOperationalClientEmulatedState _operationalClientEmulatedState;

        // todo: ============== move to separate fake component/IOptions<>? ==============
        private string _token = "SLMX4ga5.t84Q";
        private bool _emulateRecurrentExceptions = /*true*/false;
        private int _recurrentExceptionDifficultyClass = 17;
        private TimeSpan _webhookUpdatesTimerInterval = TimeSpan.FromSeconds(10);
        private bool _emulateConflictingWebhookUrl = false;
        private int _conflictingWebhookUrlDifficultyClass = 15;
        // todo: ==============================================================

        public FakeBotApiClient(ILogger<FakeBotApiClient> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _operationalClientEmulatedState = CreateOperationalClientEmulatedState(serviceProvider);
        }

        public override BotApiComponentDescriptor ComponentDescriptor =>
            BotApiComponentDescriptor.Fake;

        public override async Task RegisterClientAsync()
        {
            try
            {
                var operationalClientSettings = new FakeOperationalClient.Settings
                {
                    Token = _token,
                    EmulateRecurrentExceptions = _emulateRecurrentExceptions,
                    RecurrentExceptionDifficultyClass = _recurrentExceptionDifficultyClass
                };
                OperationalClient = new FakeOperationalClient(_operationalClientEmulatedState, operationalClientSettings);
                if (!await OperationalClient.ValidateClientAsync())
                {
                    HandleClientInitializationFailed($"Fake Bot API Client registration failed, the token ({OperationalClient.Token.Bar()}) is invalid.");
                    return;
                }
                ComponentState = BotApiComponentState.CreateRegistered();
                _logger.LogInformation("Fake Bot API Client registered.");
            }
            catch (Exception e)
            {
                const string errorMessage = "Fake Bot API Client registration failed, an exception has occurred";
                HandleClientInitializationFailed($"{errorMessage}: {e.Message}", $"{errorMessage}.{Environment.NewLine}{e}");
            }
        }

        private void HandleClientInitializationFailed(string errorMessage) => HandleClientInitializationFailed(errorMessage, errorMessage);

        private void HandleClientInitializationFailed(string errorStateDescription, string errorMessage)
        {
            OperationalClient = null;
            ComponentState = BotApiComponentState.CreateError(errorStateDescription);
            _logger.LogError(errorMessage);
        }

        public override Task UnregisterClientAsync()
        {
            OperationalClient = null;
            ComponentState = BotApiComponentState.CreateUnregistered();
            _logger.LogInformation("Fake Bot API Client unregistered.");
            return Task.CompletedTask;
        }

        private IFakeOperationalClientEmulatedState CreateOperationalClientEmulatedState(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<FakeOperationalClient>>();
            var token = new FakeOperationalClientToken(_token);
            var randomUtils = new FakeOperationalClientRandomUtils();
            var webhookUrlValidator = new FakeOperationalClientWebhookUrlValidator(serviceProvider);
            var settings = new FakeOperationalClientEmulatedState.Settings
            {
                WebhookUpdatesTimerInterval = _webhookUpdatesTimerInterval,
                EmulateConflictingWebhookUrl = _emulateConflictingWebhookUrl,
                ConflictingWebhookUrlDifficultyClass = _conflictingWebhookUrlDifficultyClass
            };
            return new FakeOperationalClientEmulatedState(serviceProvider, logger, token, randomUtils, webhookUrlValidator, settings);
        }

        public void Dispose()
        {
            (_operationalClientEmulatedState as FakeOperationalClientEmulatedState)?.Dispose();
        }
    }
}
