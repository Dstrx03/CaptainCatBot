using System.Threading.Tasks;

namespace Cat.Domain
{
    public class BotUpdateProcessor
    {
        public async Task ProcessUpdate(BotUpdateContext updateContext)
        {
            await updateContext.BotApiSender.SendMessageAsync($"Echo: {updateContext.Message}");
        }
    }
}
