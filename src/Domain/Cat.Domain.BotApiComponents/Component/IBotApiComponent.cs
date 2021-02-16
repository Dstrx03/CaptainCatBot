using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.Domain.BotApiComponents.Component
{
    public interface IBotApiComponent
    {
        public static IEnumerable<BotApiComponentDescriptor> ApplicableComponentDescriptors =>
            Enum.GetValues(typeof(BotApiComponentDescriptor))
                .Cast<BotApiComponentDescriptor>()
                .Where(_ => _ != BotApiComponentDescriptor.None);

        BotApiComponentDescriptor ComponentDescriptor { get; }
    }

    public static class IBotApiComponentExtensions
    {
        public static IServiceCollection AddBotApiComponent<TBotApiComponentImplementation>(
            this IServiceCollection services, Func<IServiceProvider, TBotApiComponentImplementation> implementationFactory = null)
            where TBotApiComponentImplementation : class, IBotApiComponent
        {
            if (implementationFactory != null)
                services.AddSingleton(implementationFactory);
            else
                services.AddSingleton<TBotApiComponentImplementation>();

            services.AddSingleton<IBotApiComponent, TBotApiComponentImplementation>(_ => _.GetRequiredService<TBotApiComponentImplementation>());

            return services;
        }
    }
}
