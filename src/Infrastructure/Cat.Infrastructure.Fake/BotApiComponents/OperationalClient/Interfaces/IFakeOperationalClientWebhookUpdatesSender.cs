using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces
{
    public interface IFakeOperationalClientWebhookUpdatesSender
    {
        TimeSpan Timeout { get; set; }
        Task<HttpResponseMessage> SendUpdateAsync(string webhookUrl, FakeBotUpdate update);
    }
}
