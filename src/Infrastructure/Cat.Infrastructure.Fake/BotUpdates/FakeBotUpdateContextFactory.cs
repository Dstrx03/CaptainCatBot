using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Domain.BotUpdates.Context;
using Cat.Infrastructure.Fake.BotApiComponents;

namespace Cat.Infrastructure.Fake.BotUpdates
{
    public class FakeBotUpdateContextFactory : IBotUpdateContextFactory<FakeBotUpdate>
    {
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
