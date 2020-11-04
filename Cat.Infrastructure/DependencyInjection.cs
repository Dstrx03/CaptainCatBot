using Cat.Application;
using Cat.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    /* todo: from docs.microsoft.com => namespace Microsoft.Extensions.DependencyInjection
     * Note: Each services.Add{GROUP_NAME} extension method adds and potentially configures services.
     * For example, AddControllersWithViews adds the services MVC controllers with views require,
     * and AddRazorPages adds the services Razor Pages requires. We recommended that apps follow this naming convention.
     * Place extension methods in the Microsoft.Extensions.DependencyInjection namespace to encapsulate groups of service registrations.
     */
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
