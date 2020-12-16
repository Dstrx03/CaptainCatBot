using System;
using System.Threading.Tasks;
using Cat.Application;
using Cat.Domain;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public class FakeBotUpdatePreProcessor : IBotUpdatePreProcessor<FakeBotUpdate>
    {
        private readonly ILogger<FakeBotUpdatePreProcessor> _logger;
        private readonly FakeBotApiClient _botApiClient;

        // todo: ==== move to separate component with URL composition/management responsibility ====
        private string _webhookUrl = "https://localhost:44336/FakeBotApiEndpoint";
        // todo: ===================================================================================

        public FakeBotUpdatePreProcessor(ILogger<FakeBotUpdatePreProcessor> logger, FakeBotApiClient botApiClient)
        {
            _logger = logger;
            _botApiClient = botApiClient;
        }

        public bool PreProcessingIsRequired(FakeBotUpdate update) => 
            update != null && !string.IsNullOrEmpty(update.ValidationToken) && !string.IsNullOrWhiteSpace(update.ValidationToken);

        public async Task<bool> PreProcessUpdateAsync(FakeBotUpdate update)
        {
            try
            {
                await _botApiClient.ConsumeOperationalClientAsync(
                    _ => _.ConfirmWebhookUrlValidationTokenAsync(update.ValidationToken, _webhookUrl), 
                    () => _logger.LogError($"Fake Bot Update Pre-processor execution failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."));
            }
            catch (Exception e)
            {
                _logger.LogError($"Fake Bot Update Pre-processor execution failed, an exception has occurred.{Environment.NewLine}{e}");
            }
            return true;
        }
    }
}
