
namespace Cat.Domain
{
    public interface IBotApiPoller : IBotApiComponent
    {
        void RegisterPoller();
        void UnregisterPoller();
    }
}
