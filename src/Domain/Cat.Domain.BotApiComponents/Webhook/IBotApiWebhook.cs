using Cat.Domain.BotApiComponents.Component;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Webhook
{
    public interface IBotApiWebhook<TWebhookInfo> : IBotApiStatefulComponent
    {
        Task RegisterWebhookAsync();
        Task UnregisterWebhookAsync();
        TWebhookInfo WebhookInfo { get; }
    }
}
