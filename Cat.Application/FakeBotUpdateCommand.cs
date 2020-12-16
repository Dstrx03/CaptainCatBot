using Cat.Domain;

namespace Cat.Application
{
    public class FakeBotUpdateCommand : IBotUpdateCommand<FakeBotUpdate>
    {
        public FakeBotUpdateCommand(FakeBotUpdate update)
        {
            Update = update;
        }

        public FakeBotUpdate Update { get; set; }
    }

    public class FakeBotUpdateCommandHandler : BotUpdateCommandHandlerBase<FakeBotUpdateCommand, FakeBotUpdate>
    {
        public FakeBotUpdateCommandHandler(BotUpdateProcessor updateProcessor, IBotUpdateContextFactory<FakeBotUpdate> updateContextFactory) 
            : base(updateProcessor, updateContextFactory)
        {
        }
    }
}
