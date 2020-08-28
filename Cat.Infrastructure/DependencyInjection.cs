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
            // todo: use Lamar/StructureMap with default conventions
            services.AddTransient(typeof(FakeBotApiSender), typeof(FakeBotApiSender));
            services.AddTransient(typeof(BotUpdateProcessor), typeof(BotUpdateProcessor));
            // todo: ==================================

            services.AddTransient(typeof(IBotUpdateContextFactory<FakeBotUpdate>), typeof(FakeBotUpdateContextFactory));

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
