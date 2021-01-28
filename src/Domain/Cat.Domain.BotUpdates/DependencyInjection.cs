using Cat.Domain.BotUpdates.Processor;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainBotUpdates(this IServiceCollection services)
        {
            services.AddTransient<BotUpdateProcessor>();

            return services;
        }
    }
}
