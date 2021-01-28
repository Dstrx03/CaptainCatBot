using Cat.Domain.BotUpdates.Context;
using System.Threading.Tasks;

namespace Cat.Domain.BotUpdates.Processor
{
    public class BotUpdateProcessor
    {
        public async Task ProcessUpdateAsync(BotUpdateContext updateContext)
        {
            await updateContext.BotApiSender.SendMessageAsync($"Echo: {updateContext.Message}");
        }
    }
}
