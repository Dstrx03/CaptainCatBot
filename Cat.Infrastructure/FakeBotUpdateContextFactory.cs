﻿using Cat.Application;
using Cat.Domain;

namespace Cat.Infrastructure
{
    public class FakeBotUpdateContextFactory : IBotUpdateContextFactory<FakeBotUpdate>
    {
        // todo: implement better solution to provide instances of IBotApiSender
        private readonly FakeBotApiSender _botApiSender;

        public FakeBotUpdateContextFactory(FakeBotApiSender botApiSender)
        {
            _botApiSender = botApiSender;
        }

        public BotUpdateContext CreateContext(FakeBotUpdate update)
        {
            return new BotUpdateContext
            {
                Message = update.Message, 
                BotApiSender = _botApiSender
            };
        }
    }
}