using Cat.Domain.BotApiComponents.Component;

namespace Cat.Domain.BotApiComponents.Poller
{
    public interface IBotApiPoller : IBotApiComponent
    {
        void RegisterPoller();
        void UnregisterPoller();
    }
}
