using Cat.Domain.BotApiComponents.ComponentsManager;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Services
{
    public class BotApiComponentsLifetimeManager : IHostedService
    {
        // todo: ensure all component which may be possibly used in concurrent context are thread safe (FakeBotApiWebhook definitely not thread safe)

        private readonly ILogger<BotApiComponentsLifetimeManager> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IEnumerable<IBotApiComponentsManager> _botApiComponentsManagers;

        public BotApiComponentsLifetimeManager(
            ILogger<BotApiComponentsLifetimeManager> logger,
            IHostApplicationLifetime applicationLifetime,
            IEnumerable<IBotApiComponentsManager> botApiComponentsManagers)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
            _botApiComponentsManagers = botApiComponentsManagers;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var componentsManagersToRegister = _botApiComponentsManagers.Where(_ => _.RegisterAtApplicationStart).ToList();
            await StartAsync(componentsManagersToRegister.Where(_ => !_.RegisterAtApplicationStartAfterAppHost), cancellationToken);
            _applicationLifetime.ApplicationStarted.Register(async () =>
            {
                await StartAsync(componentsManagersToRegister.Where(_ => _.RegisterAtApplicationStartAfterAppHost), cancellationToken);
            });
        }

        private async Task StartAsync(IEnumerable<IBotApiComponentsManager> botApiComponentsManagers, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var componentsManager in botApiComponentsManagers)
                    await componentsManager.RegisterComponentsAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Bot API Components Lifetime Manager start operation failed, an exception has occurred.{Environment.NewLine}{e}");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                foreach (var componentsManager in _botApiComponentsManagers)
                    await componentsManager.UnregisterComponentsAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Bot API Components Lifetime Manager stop operation failed, an exception has occurred.{Environment.NewLine}{e}");
            }
        }
    }
}
