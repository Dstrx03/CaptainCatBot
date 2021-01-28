using Cat.Domain.BotApiComponents.Component;

namespace Cat.Domain.BotApiComponents.Endpoint
{
    public interface IBotApiEndpoint : IBotApiComponent
    {
        void RegisterEndpoint();
        void UnregisterEndpoint();
    }
}
