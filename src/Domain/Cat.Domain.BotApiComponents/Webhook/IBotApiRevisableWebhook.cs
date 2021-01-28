using System.Threading.Tasks;

namespace Cat.Domain.BotApiComponents.Webhook
{
    public interface IBotApiRevisableWebhook<TWebhookInfo> : IBotApiWebhook<TWebhookInfo>
    {
        Task UpdateWebhookInfoAsync();
        Task ReviseWebhookConsistencyAsync();
    }
}
