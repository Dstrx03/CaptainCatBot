namespace Cat.Domain.BotUpdates.Context
{
    public interface IBotUpdateContextFactory<TUpdate>
    {
        BotUpdateContext CreateContext(TUpdate update);
    }
}
