
namespace Cat.Domain
{
    public interface IBotApiEndpoint : IBotApiComponent
    {
        void RegisterEndpoint();
        void UnregisterEndpoint();
        // todo: ControllerPath, EnpointPath properties?
    }
}
