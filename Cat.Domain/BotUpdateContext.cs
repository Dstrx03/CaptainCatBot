
namespace Cat.Domain
{
    public class BotUpdateContext
    {
        public string Message { get; set; }

        public IBotApiSender BotApiSender { get; set; }
    }
}
