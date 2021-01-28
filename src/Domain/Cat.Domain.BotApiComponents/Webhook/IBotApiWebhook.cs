using Cat.Domain.BotApiComponents.Component;
using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Webhook
{
    public interface IBotApiWebhook<TWebhookInfo> : IBotApiComponent
    {
        Task RegisterWebhookAsync();
        Task UnregisterWebhookAsync();
        TWebhookInfo WebhookInfo { get; }
    }
}
