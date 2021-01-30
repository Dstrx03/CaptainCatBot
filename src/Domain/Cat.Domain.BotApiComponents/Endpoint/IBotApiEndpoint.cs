using Cat.Domain.BotApiComponents.Component;

namespace Cat.Domain.BotApiComponents.Endpoint
{
    public interface IBotApiEndpoint : IBotApiStatefulComponent
    {
        void RegisterEndpoint();
        void UnregisterEndpoint();
    }
}
