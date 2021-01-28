using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddLogging(this ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddLog4Net("Logging/log4net.config");

            return builder;
        }
    }
}
