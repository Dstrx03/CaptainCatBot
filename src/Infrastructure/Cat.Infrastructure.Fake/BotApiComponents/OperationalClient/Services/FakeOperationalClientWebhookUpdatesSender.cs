using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Services
{
    public class FakeOperationalClientWebhookUpdatesSender : IFakeOperationalClientWebhookUpdatesSender
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public FakeOperationalClientWebhookUpdatesSender(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        public Task<HttpResponseMessage> SendUpdateAsync(string webhookUrl, FakeBotUpdate update) =>
            _httpClient.PostAsync(webhookUrl, CreateUpdateContent(update));

        private StringContent CreateUpdateContent(FakeBotUpdate update) =>
            new StringContent(JsonSerializer.Serialize(update, _serializerOptions), Encoding.UTF8, "application/json");
    }
}
