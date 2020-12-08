using System;
using System.Threading.Tasks;
using Cat.Application;
using Cat.Domain;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public class FakeBotUpdateValidator : IBotUpdateValidator<FakeBotUpdate> // todo: component responsibility & design, update processing pipeline design
    {
        private readonly ILogger<FakeBotUpdateValidator> _logger;
        private readonly FakeBotApiClient _botApiClient;

        // todo: ==== move to separate component with URL composition/management responsibility ====
        private string _webhookUrl = "https://localhost:44336/FakeBotApiEndpoint";
        // todo: ===================================================================================

        public FakeBotUpdateValidator(ILogger<FakeBotUpdateValidator> logger, FakeBotApiClient botApiClient)
        {
            _logger = logger;
            _botApiClient = botApiClient;
        }

        public async Task<bool> ValidateUpdateAsync(FakeBotUpdate update)
        {
            if (update.ValidationToken == null) return true;
            try
            {
                await _botApiClient.ConsumeOperationalClientAsync(async _ =>
                {
                    await _.ConfirmWebhookUrlValidationTokenAsync(update.ValidationToken, _webhookUrl);
                }, () =>
                {
                    _logger.LogDebug($"TODO can't validate webhookUrl due client is in incorrect state ({_botApiClient.ComponentState.FooBar()})");
                });
            }
            catch (Exception e)
            {
                _logger.LogDebug($"TODO can't validate webhookUrl due error{Environment.NewLine}{e}");
            }
            return false;
        }
    }
}
