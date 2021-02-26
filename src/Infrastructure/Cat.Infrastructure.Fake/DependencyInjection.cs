using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Domain.BotApiComponents.Component;
using Cat.Domain.BotUpdates.Context;
using Cat.Domain.BotUpdates.PreProcessor;
using Cat.Infrastructure.Fake.BotApiComponents;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Services;
using Cat.Infrastructure.Fake.BotUpdates;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureFake(this IServiceCollection services)
        {
            services.AddHttpClient<IFakeOperationalClientWebhookUpdatesSender, FakeOperationalClientWebhookUpdatesSender>();

            services.AddTransient<IBotUpdatePreProcessor<FakeBotUpdate>, FakeBotUpdatePreProcessor>();
            services.AddTransient<IBotUpdateContextFactory<FakeBotUpdate>, FakeBotUpdateContextFactory>();

            services.AddBotApiComponent<FakeBotApiComponentsManager>();
            services.AddBotApiComponent<FakeBotApiClient>();
            services.AddBotApiComponent<FakeBotApiSender>(); // todo: sender should use transient lifetime?
            services.AddBotApiComponent<FakeBotApiWebhook>();
            services.AddBotApiComponent<FakeBotApiPoller>();

            return services;
        }
    }
}
