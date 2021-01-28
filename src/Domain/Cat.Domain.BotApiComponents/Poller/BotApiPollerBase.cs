using Cat.Domain.BotApiComponents.Component;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Poller
{
    public abstract class BotApiPollerBase<TUpdate> : BotApiComponentBase, IBotApiPoller
    {
        public abstract void RegisterPoller();
        public abstract void UnregisterPoller();
        protected abstract Task SendUpdateCommand(TUpdate update);
    }
}
