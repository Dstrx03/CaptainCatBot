using System.Threading.Tasks;

namespace Cat.Domain
{
    public interface IBotApiClient<TOperationalClient> : IBotApiComponent
    {
        Task RegisterClientAsync();
        Task UnregisterClientAsync();
        TOperationalClient OperationalClient { get; }
    }
}
