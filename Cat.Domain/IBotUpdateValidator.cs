using System.Threading.Tasks;

namespace Cat.Domain
{
    public interface IBotUpdateValidator<TUpdate> // todo: component responsibility & design, update processing pipeline design
    {
        Task<bool> ValidateUpdateAsync(TUpdate update);
    }
}
