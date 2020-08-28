
namespace Cat.Domain
{
    public interface IBotUpdateContextFactory<T>
    {
        BotUpdateContext CreateContext(T update);
    }
}
