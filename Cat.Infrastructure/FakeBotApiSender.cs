using Cat.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

        public Task SendMessageAsync(string message) => HandlingConsumeOperationalClientAsync(_ => _.SendFakeMessageAsync(message), "SendMessageAsync");

        private Task HandlingConsumeOperationalClientAsync(Func<FakeOperationalClient, Task> actionAsync, string operationName) =>
            _botApiClient.ConsumeOperationalClientAsync(_ =>
                {
                    try
                    {
                        return actionAsync(_);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Fake Bot API Sender the [{operationName}] operation failed, an exception has occurred.{Environment.NewLine}{e}");
                        return Task.CompletedTask;
                    }
                },
                () => _logger.LogError($"Fake Bot API Sender the [{operationName}] operation failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."));
    }
}
