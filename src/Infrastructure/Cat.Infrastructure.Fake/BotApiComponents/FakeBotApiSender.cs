﻿using Cat.Domain;
using Cat.Domain.BotApiComponents.Component;
using Cat.Domain.BotApiComponents.Sender;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents
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

        public BotApiComponentDescriptor ComponentDescriptor =>
            BotApiComponentDescriptor.Fake;

        public Task SendMessageAsync(string message) => HandlingConsumeOperationalClientAsync(_ => _.SendFakeMessageAsync(message), "SendMessageAsync");

        private Task HandlingConsumeOperationalClientAsync(Func<FakeOperationalClient, Task> actionAsync, string operationName)
        {
            try
            {
                return _botApiClient.ConsumeOperationalClientAsync(
                    actionAsync,
                    () => _logger.LogError($"Fake Bot API Sender the [{operationName}] operation failed, the Fake Bot API Client ({_botApiClient.ComponentState.FooBar()}) cannot be consumed."));
            }
            catch (Exception e)
            {
                _logger.LogError($"Fake Bot API Sender the [{operationName}] operation failed, an exception has occurred.{Environment.NewLine}{e}");
                return Task.CompletedTask;
            }
        }
    }
}
