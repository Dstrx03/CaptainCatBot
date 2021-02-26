using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Domain;
using Cat.Infrastructure.Fake.BotApiComponents.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient
{
    public class FakeOperationalClient
    {
        private readonly Settings _settings;
        private readonly IFakeOperationalClientEmulatedState _emulatedState;

        public FakeOperationalClient(IFakeOperationalClientEmulatedState emulatedState, Settings settings = null)
        {
            _emulatedState = emulatedState;
            _settings = settings == null ? new Settings() : new Settings(settings);
        }

        public string Token
        {
            get => _settings.Token;
            set => _settings.Token = value;
        }

        public bool EmulateRecurrentExceptions
        {
            get => _settings.EmulateRecurrentExceptions;
            set => _settings.EmulateRecurrentExceptions = value;
        }

        public int RecurrentExceptionDifficultyClass
        {
            get => _settings.RecurrentExceptionDifficultyClass;
            set => _settings.RecurrentExceptionDifficultyClass = value;
        }

        public Task<bool> ValidateClientAsync()
        {
            EmulateRecurrentExceptionByRemoteService();
            return Task.FromResult(_emulatedState.Token.IsTokenValid(Token));
        }

        public Task SendFakeMessageAsync(string message)
        {
            EmulateRequestToRemoteService();
            var details = TodoMsgFmtr.DetailsMessage("Fake message details", new (string title, object value)[] // todo: content
            {
                ("Message", message)
            });
            _emulatedState.Logger.LogInformation($"Fake Operational Client sending fake message...{Environment.NewLine}{details}");
            return Task.CompletedTask;
        }

        public Task SetWebhookAsync(string webhookUrl)
        {
            EmulateRequestToRemoteService();
            return _emulatedState.SetWebhookAsync(webhookUrl);
        }

        public Task<FakeWebhookInfo> GetWebhookInfoAsync()
        {
            EmulateRequestToRemoteService();
            return Task.FromResult(_emulatedState.GetWebhookInfo());
        }

        public Task DeleteWebhookAsync()
        {
            EmulateRequestToRemoteService();
            _emulatedState.DeleteWebhook();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<FakeBotUpdate>> GetUpdatesAsync()
        {
            EmulateRequestToRemoteService();
            return Task.FromResult(_emulatedState.GenerateRandomUpdates());
        }

        public Task ConfirmWebhookUrlValidationTokenAsync(string validationToken, string webhookUrl)
        {
            EmulateRequestToRemoteService();
            _emulatedState.WebhookUrlValidator.ConfirmWebhookUrlValidationToken(validationToken, webhookUrl);
            return Task.CompletedTask;
        }

        private void EmulateRequestToRemoteService()
        {
            EmulateRecurrentExceptionByRemoteService();
            EmulateRemoteTokenValidationByRemoteService();
        }

        private void EmulateRecurrentExceptionByRemoteService()
        {
            if (EmulateRecurrentExceptions && _emulatedState.RandomUtils.NextBoolean(RecurrentExceptionDifficultyClass))
                throw new FakeOperationalClientEmulatedException();
        }

        private void EmulateRemoteTokenValidationByRemoteService()
        {
            if (!_emulatedState.Token.IsTokenValid(Token))
                throw new InvalidOperationException($"Operation cannot be executed due to the provided token ({Token.Bar()}) is invalid.");
        }

        public class Settings
        {
            public Settings()
            {
            }

            public Settings(Settings settings)
            {
                Token = settings.Token;
                EmulateRecurrentExceptions = settings.EmulateRecurrentExceptions;
                RecurrentExceptionDifficultyClass = settings.RecurrentExceptionDifficultyClass;
            }

            public string Token { get; set; } = null;
            public bool EmulateRecurrentExceptions { get; set; } = false;
            public int RecurrentExceptionDifficultyClass { get; set; } = 17;
        }
    }
}
