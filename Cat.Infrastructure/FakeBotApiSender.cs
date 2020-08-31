using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public class FakeBotApiSender : IBotApiSender
    {
        private readonly ILogger<FakeBotApiSender> _logger;

        public FakeBotApiSender(ILogger<FakeBotApiSender> logger)
        {
            _logger = logger;
        }

        public Task SendMessageAsync(string message)
        {
            FakeSend("SendMessageAsync", Arg(nameof(message), message));
            return Task.CompletedTask;
        }

        private void FakeSend(string method, params KeyValuePair<string, object>[] args)
        {
            _logger.LogDebug($"Fake Bot API Sender [{method}] {string.Join(", ", args.Select(x => $"{x.Key}:'{x.Value}'"))}");
        }

        private KeyValuePair<string, object> Arg(string argumentName, object argument)
        {
            return new KeyValuePair<string, object>(argumentName, argument);
        }
    }
}
