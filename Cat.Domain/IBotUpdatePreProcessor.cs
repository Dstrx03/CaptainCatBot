using System.Threading.Tasks;

namespace Cat.Domain
{
    public interface IBotUpdatePreProcessor<TUpdate>
    {
        bool PreProcessingIsRequired(TUpdate update);
        Task<bool> PreProcessUpdateAsync(TUpdate update);
    }
}
