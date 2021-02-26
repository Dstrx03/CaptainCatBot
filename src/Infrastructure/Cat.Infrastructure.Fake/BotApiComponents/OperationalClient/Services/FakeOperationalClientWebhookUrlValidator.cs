using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Domain;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Services
{
    public class FakeOperationalClientWebhookUrlValidator : IFakeOperationalClientWebhookUrlValidator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, string> _webhookUrlValidationTokensDictionary;
        private readonly TimeSpan _webhookUrlValidationTimeout;

        public FakeOperationalClientWebhookUrlValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _webhookUrlValidationTokensDictionary = new ConcurrentDictionary<string, string>();
            _webhookUrlValidationTimeout = TimeSpan.FromSeconds(5);
        }

        public async Task ValidateWebhookUrlAsync(string webhookUrl)
        {
            var validationToken = string.Empty;
            try
            {
                validationToken = await GenerateConfirmedWebhookUrlValidationTokenAsync(webhookUrl);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"The provided webhook URL ({webhookUrl.Bar()}) is invalid.", e);
            }
            finally
            {
                _webhookUrlValidationTokensDictionary.TryRemove(validationToken, out _);
            }
        }

        private async Task<string> GenerateConfirmedWebhookUrlValidationTokenAsync(string webhookUrl)
        {
            if (string.IsNullOrEmpty(webhookUrl) || string.IsNullOrWhiteSpace(webhookUrl))
                throw new ArgumentException("Webhook URL cannot be null, empty or whitespace.");

            var validationToken = Guid.NewGuid().ToString();
            var confirmationResponse = await RequestWebhookUrlValidationTokenConfirmationAsync(webhookUrl, validationToken);

            _webhookUrlValidationTokensDictionary.TryGetValue(validationToken, out var storedWebhookUrl);

            if (confirmationResponse.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"The server responded with an unacceptable status ({confirmationResponse.StatusCode:D} {confirmationResponse.StatusCode:G})."); // todo: HTTP status codes formatting
            if (storedWebhookUrl == null)
                throw new InvalidOperationException($"The validation token ({validationToken.Bar()}) was not confirmed.");
            if (!string.Equals(storedWebhookUrl, webhookUrl, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException($"The URL ({storedWebhookUrl.Bar()}) provided at confirmation of the validation token do not match the webhook URL.");

            return validationToken;
        }

        private Task<HttpResponseMessage> RequestWebhookUrlValidationTokenConfirmationAsync(string webhookUrl, string validationToken)
        {
            var confirmationRequestUpdate = new FakeBotUpdate { ValidationToken = validationToken };
            var webhookUpdatesSender = _serviceProvider.GetRequiredService<IFakeOperationalClientWebhookUpdatesSender>();
            webhookUpdatesSender.Timeout = _webhookUrlValidationTimeout;
            return webhookUpdatesSender.SendUpdateAsync(webhookUrl, confirmationRequestUpdate);
        }

        public void ConfirmWebhookUrlValidationToken(string validationToken, string webhookUrl)
        {
            _webhookUrlValidationTokensDictionary.TryAdd(validationToken, webhookUrl);
            EnsureWebhookUrlValidationTokenReleased(validationToken);
        }

        private void EnsureWebhookUrlValidationTokenReleased(string validationToken)
        {
            Task.Run(async () =>
            {
                await Task.Delay(_webhookUrlValidationTimeout); // todo: maybe there is a better alternative to Task.Delay?
                _webhookUrlValidationTokensDictionary.TryRemove(validationToken, out _);
            });
        }
    }
}
