
namespace Cat.Domain
{
    public interface IBotApiEndpoint
    {
        void RegisterEndpoint();
        void UnregisterEndpoint();
        bool GetStatus(); // todo: use enum?
    }
}
