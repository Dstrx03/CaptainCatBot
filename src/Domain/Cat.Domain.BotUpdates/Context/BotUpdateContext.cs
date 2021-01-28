using Cat.Domain.BotApiComponents.Sender;

namespace Cat.Domain.BotUpdates.Context
{
    public class BotUpdateContext
    {
        public string Message { get; set; }

        public IBotApiSender BotApiSender { get; set; }
    }
}
