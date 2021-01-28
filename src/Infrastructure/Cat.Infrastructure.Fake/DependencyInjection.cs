using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Domain.BotApiComponents.ComponentsManager;
using Cat.Domain.BotUpdates.Context;
using Cat.Domain.BotUpdates.PreProcessor;
using Cat.Infrastructure.Fake.BotApiComponents;
using Cat.Infrastructure.Fake.BotUpdates;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureFake(this IServiceCollection services)
        {
            services.AddTransient<IBotUpdatePreProcessor<FakeBotUpdate>, FakeBotUpdatePreProcessor>();
            services.AddTransient<IBotUpdateContextFactory<FakeBotUpdate>, FakeBotUpdateContextFactory>();

            services.AddSingleton<IBotApiComponentsManager, FakeBotApiComponentsManager>();

            services.AddTransient<FakeBotApiSender>();
            services.AddSingleton<FakeBotApiClient>();
            services.AddSingleton<FakeBotApiWebhook>();
            services.AddSingleton<FakeBotApiPoller>();

            return services;
        }
    }
}
