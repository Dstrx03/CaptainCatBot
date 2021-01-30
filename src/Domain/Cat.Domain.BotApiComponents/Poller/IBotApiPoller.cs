using Cat.Domain.BotApiComponents.Component;

namespace Cat.Domain.BotApiComponents.Poller
{
    public interface IBotApiPoller : IBotApiStatefulComponent
    {
        void RegisterPoller();
        void UnregisterPoller();
    }
}
