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
        private readonly FakeOperationalClientHelper _helper;

        public FakeOperationalClient(Settings settings, FakeOperationalClientHelper helper)
        {
            _settings = settings ?? new Settings();
            _helper = helper;
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
            EmulateFakeRecurrentException();
            return Task.FromResult(_helper.IsTokenValid(Token));
        }

        public Task SendFakeMessageAsync(string message)
        {
            CheckClientValidity();
            var details = TodoMsgFmtr.DetailsMessage("Fake message details", new (string title, object value)[] // todo: content
            {
                ("Message", message)
            });
            _helper.OperationalClientLogger.LogInformation($"Fake Operational Client sending fake message...{Environment.NewLine}{details}");
            return Task.CompletedTask;
        }

        public Task SetWebhookAsync(string webhookUrl)
        {
            CheckClientValidity();
            return _helper.SetWebhookAsync(webhookUrl);
        }

        public Task<FakeWebhookInfo> GetWebhookInfoAsync()
        {
            CheckClientValidity();
            return Task.FromResult(_helper.GetWebhookInfo());
        }

        public Task DeleteWebhookAsync()
        {
            CheckClientValidity();
            _helper.DeleteWebhook();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<FakeBotUpdate>> GetUpdatesAsync()
        {
            CheckClientValidity();
            return Task.FromResult(_helper.GenerateRandomUpdates());
        }

        public Task ConfirmWebhookUrlValidationTokenAsync(string validationToken, string webhookUrl)
        {
            CheckClientValidity();
            _helper.ConfirmWebhookUrlValidationToken(validationToken, webhookUrl);
            return Task.CompletedTask;
        }

        #region CheckClientValidity

        private void CheckClientValidity()
        {
            EmulateFakeRecurrentException();
            CheckFakeTokenValidity();
        }

        private void EmulateFakeRecurrentException()
        {
            if (EmulateRecurrentExceptions && _helper.RandomBoolean(RecurrentExceptionDifficultyClass))
                throw new FakeOperationalClientEmulatedException();
        }

        private void CheckFakeTokenValidity()
        {
            if (!_helper.IsTokenValid(Token))
                throw new InvalidOperationException($"Operation cannot be executed due to the provided token ({Token.Bar()}) is invalid.");
        }

        #endregion

        public class Settings
        {
            public string Token { get; set; } = null;
            public bool EmulateRecurrentExceptions { get; set; } = false;
            public int RecurrentExceptionDifficultyClass { get; set; } = 17;
        }
    }
}
