
namespace Cat.Domain
{
    public interface IBotUpdateContextFactory<TUpdate>
    {
        BotUpdateContext CreateContext(TUpdate update);
    }
}
