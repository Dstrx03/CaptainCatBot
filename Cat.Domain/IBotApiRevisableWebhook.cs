using System.Threading.Tasks;

namespace Cat.Domain
{
    public interface IBotApiRevisableWebhook<TWebhookInfo> : IBotApiWebhook<TWebhookInfo>
    {
        Task UpdateWebhookInfoAsync();
        Task ReviseWebhookConsistencyAsync();
    }
}
