using Cat.Application;
using Cat.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // todo: use Lamar/StructureMap with default conventions, lifetimes etc.
            services.AddTransient<IBotUpdateContextFactory<FakeBotUpdate>, FakeBotUpdateContextFactory>();
            services.AddTransient<FakeBotApiSender>();
            services.AddSingleton<FakeBotApiClient>();
            services.AddSingleton<FakeBotApiWebhook>();
            services.AddSingleton<FakeBotApiPoller>();
            // todo: ==================================

            return services;
        }

        public static ILoggerFactory AddLogging(this ILoggerFactory loggerFactory)
        {
            // todo: decide where to keep log4net.config file
            loggerFactory.AddLog4Net();
            return loggerFactory;
        }
    }
}
