using System.Threading.Tasks;

namespace Cat.Domain
{
    public class BotUpdateProcessor
    {
        public async Task ProcessUpdateAsync(BotUpdateContext updateContext)
        {
            await updateContext.BotApiSender.SendMessageAsync($"Echo: {updateContext.Message}");
        }
    }
}
