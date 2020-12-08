using Microsoft.Extensions.DependencyInjection;

namespace Cat.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<BotUpdateProcessor>();

            return services;
        }
    }
}
