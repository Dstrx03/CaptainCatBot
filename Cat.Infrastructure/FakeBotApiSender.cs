using System;
using System.Threading.Tasks;
using Cat.Domain;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public class FakeBotApiSender : IBotApiSender
    {
        private readonly ILogger<FakeBotApiSender> _logger;
        private readonly FakeBotApiClient _botApiClient;

        public FakeBotApiSender(ILogger<FakeBotApiSender> logger, FakeBotApiClient botApiClient)
        {
            _logger = logger;
            _botApiClient = botApiClient;
        }

        public async Task SendMessageAsync(string message)
        {
            _logger.LogDebug($"[SendMessageAsync] '{message}'"); // todo: apply single text format convention for all Fake Bot API components log messages
            if (BotApiComponentState.IsRegistered(_botApiClient))
                await _botApiClient.OperationalClient.SendFakeMessageAsync(message);
            else 
                _logger.LogDebug("can't invoke [SendMessageAsync], operational client is invalid"); // todo: apply single text format convention for all Fake Bot API components log messages
        }

    }
}
