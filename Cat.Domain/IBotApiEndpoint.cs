
namespace Cat.Domain
{
    public interface IBotApiEndpoint
    {
        void RegisterEndpoint();
        void UnregisterEndpoint();
        // todo: EnpointPath property?
        bool GetStatus(); // todo: use enum?
    }
}
