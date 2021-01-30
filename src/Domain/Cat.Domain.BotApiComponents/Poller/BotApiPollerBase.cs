using Cat.Domain.BotApiComponents.Component;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Poller
{
    public abstract class BotApiPollerBase<TUpdate> : BotApiStatefulComponentBase, IBotApiPoller
    {
        public abstract void RegisterPoller();
        public abstract void UnregisterPoller();
        protected abstract Task SendUpdateCommand(TUpdate update);
    }
}
