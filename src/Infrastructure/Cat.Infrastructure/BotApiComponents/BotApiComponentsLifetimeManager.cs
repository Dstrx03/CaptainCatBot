using Cat.Domain.BotApiComponents.ComponentsLifetimeManager;
using Cat.Domain.BotApiComponents.ComponentsLocator;
using Cat.Domain.BotApiComponents.ComponentsManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Infrastructure.BotApiComponents
{
    public class BotApiComponentsLifetimeManager : BotApiComponentsLifetimeManagerBase, IHostedService
    {
        private readonly ILogger<BotApiComponentsLifetimeManager> _logger;
        private readonly IOptions<BotApiComponentsLifetimeManagerOptions> _options;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;

        public BotApiComponentsLifetimeManager(
            ILogger<BotApiComponentsLifetimeManager> logger,
            IOptions<BotApiComponentsLifetimeManagerOptions> options,
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _options = options;
            _applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botApiComponentsLocator = scope.ServiceProvider.GetRequiredService<IBotApiComponentsLocator>();

            if (_options.Value.CheckComponentsSetup)
                CheckComponentsSetup(botApiComponentsLocator);

            var componentsManagersToRegister = GetComponentsManagersToRegister(botApiComponentsLocator);
            await ApplyRegisterManagersComponentsAsync(componentsManagersToRegister, botApiComponentsLocator);

            _applicationLifetime.ApplicationStarted.Register(ApplicationStartedCallback);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botApiComponentsLocator = scope.ServiceProvider.GetRequiredService<IBotApiComponentsLocator>();

            var botApiComponentsManagers = GetComponentsManagers(botApiComponentsLocator);
            await ApplyUnregisterManagersComponentsAsync(botApiComponentsManagers, botApiComponentsLocator);
        }

        private async void ApplicationStartedCallback()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var botApiComponentsLocator = scope.ServiceProvider.GetRequiredService<IBotApiComponentsLocator>();

                var componentsManagersToRegisterAfterAppHost = GetComponentsManagersToRegisterAfterAppHost(botApiComponentsLocator);
                await ApplyRegisterManagersComponentsAsync(componentsManagersToRegisterAfterAppHost, botApiComponentsLocator);
            }
            catch (Exception e)
            {
                _logger.LogError($"Bot API Components Lifetime Manager an exception occurred while executing ApplicationStarted callback.{Environment.NewLine}{e}");
            }
        }

        protected override Task ApplyRegisterManagersComponentsAsync(IEnumerable<IBotApiComponentsManager> botApiComponentsManagers, IBotApiComponentsLocator botApiComponentsLocator)
        {
            try
            {
                return base.ApplyRegisterManagersComponentsAsync(botApiComponentsManagers, botApiComponentsLocator);
            }
            catch (Exception e)
            {
                _logger.LogError($"Bot API Components Lifetime Manager components registration failed, an exception has occurred.{Environment.NewLine}{e}");
                return Task.CompletedTask;
            }
        }

        protected override Task ApplyUnregisterManagersComponentsAsync(IEnumerable<IBotApiComponentsManager> botApiComponentsManagers, IBotApiComponentsLocator botApiComponentsLocator)
        {
            try
            {
                return base.ApplyUnregisterManagersComponentsAsync(botApiComponentsManagers, botApiComponentsLocator);
            }
            catch (Exception e)
            {
                _logger.LogError($"Bot API Components Lifetime Manager components unregistration failed, an exception has occurred.{Environment.NewLine}{e}");
                return Task.CompletedTask;
            }
        }
    }

    public class BotApiComponentsLifetimeManagerOptions
    {
        public bool CheckComponentsSetup { get; set; } = true;
    }

    public static class BotApiComponentsLifetimeManagerExtensions
    {
        public static IServiceCollection AddBotApiComponentsLifetimeManager(this IServiceCollection services, Action<BotApiComponentsLifetimeManagerOptions> configuration = null)
        {
            services.AddHostedService<BotApiComponentsLifetimeManager>();
            if (configuration != null)
                services.Configure(configuration);

            return services;
        }
    }
}
