using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
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
