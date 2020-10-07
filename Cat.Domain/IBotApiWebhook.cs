using System.Threading.Tasks;

namespace Cat.Domain
{
    public interface IBotApiWebhook<TWebhookInfo> : IBotApiComponent
    {
        Task RegisterWebhookAsync();
        Task UnregisterWebhookAsync();
        TWebhookInfo WebhookInfo { get; }
    }
}
