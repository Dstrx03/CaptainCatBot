using Cat.Domain.BotApiComponents.Component;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Client
{
    public interface IBotApiClient<TOperationalClient> : IBotApiStatefulComponent
    {
        Task RegisterClientAsync();
        Task UnregisterClientAsync();
        TOperationalClient OperationalClient { get; }
        bool CanConsumeOperationalClient { get; }
    }
}
