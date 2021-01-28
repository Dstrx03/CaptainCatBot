using System.Threading.Tasks;

namespace Cat.Domain.BotUpdates.PreProcessor
{
    public interface IBotUpdatePreProcessor<TUpdate>
    {
        bool PreProcessingIsRequired(TUpdate update);
        Task<bool> PreProcessUpdateAsync(TUpdate update);
    }
}
