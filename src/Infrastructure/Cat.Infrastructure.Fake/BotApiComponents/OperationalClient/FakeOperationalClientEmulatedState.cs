using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Infrastructure.Fake.BotApiComponents.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient
{
    public interface IFakeOperationalClientEmulatedState : IDisposable
    {
        ILogger<FakeOperationalClient> Logger { get; }
        IFakeOperationalClientToken Token { get; }
        IFakeOperationalClientRandomUtils RandomUtils { get; }
        IFakeOperationalClientWebhookUrlValidator WebhookUrlValidator { get; }

        TimeSpan WebhookUpdatesTimerInterval { get; set; }
        bool EmulateConflictingWebhookUrl { get; set; }
        int ConflictingWebhookUrlDifficultyClass { get; set; }

        Task SetWebhookAsync(string webhookUrl);
        FakeWebhookInfo GetWebhookInfo();
        void DeleteWebhook();
        IEnumerable<FakeBotUpdate> GenerateRandomUpdates();
    }

    public class FakeOperationalClientEmulatedState : IFakeOperationalClientEmulatedState
    {
        private const string FakeConflictingWebhookUrl = "*FakeConflictingWebhookUrl*";

        private readonly IServiceProvider _serviceProvider;
        private readonly Settings _settings;
        private readonly Timer _webhookUpdatesTimer;
        private readonly IFakeOperationalClientTimeout _randomUpdatesTimeout;
        private readonly IFakeOperationalClientTimeout _conflictingWebhookUrlTimeout;

        private string _webhookUrl;
        private bool _webhookUpdatesTimerIsRunning;

        public ILogger<FakeOperationalClient> Logger { get; }
        public IFakeOperationalClientToken Token { get; }
        public IFakeOperationalClientRandomUtils RandomUtils { get; }
        public IFakeOperationalClientWebhookUrlValidator WebhookUrlValidator { get; }

        public FakeOperationalClientEmulatedState(
            IServiceProvider serviceProvider,
            ILogger<FakeOperationalClient> logger,
            IFakeOperationalClientToken token,
            Settings settings = null)
        {
            _serviceProvider = serviceProvider;
            Logger = logger;
            Token = token;
            RandomUtils = new FakeOperationalClientRandomUtils();
            WebhookUrlValidator = new FakeOperationalClientWebhookUrlValidator(serviceProvider);
            _settings = settings == null ? new Settings() : new Settings(settings);
            _webhookUpdatesTimer = new Timer(HandleSendWebhookUpdatesCallback, null, Timeout.Infinite, Timeout.Infinite);
            _randomUpdatesTimeout = new FakeOperationalClientTimeout(RandomUtils);
            _conflictingWebhookUrlTimeout = new FakeOperationalClientTimeout(RandomUtils);

            _webhookUpdatesTimerIsRunning = false;
            _randomUpdatesTimeout.Reset();
            _conflictingWebhookUrlTimeout.Reset();
        }

        public TimeSpan WebhookUpdatesTimerInterval
        {
            get => _settings.WebhookUpdatesTimerInterval;
            set
            {
                _settings.WebhookUpdatesTimerInterval = value;
                if (_webhookUpdatesTimerIsRunning) ApplyWebhookUpdatesTimerInterval();
            }
        }

        public bool EmulateConflictingWebhookUrl
        {
            get => _settings.EmulateConflictingWebhookUrl;
            set
            {
                _settings.EmulateConflictingWebhookUrl = value;
                if (value) _conflictingWebhookUrlTimeout.Reset();
            }
        }

        public int ConflictingWebhookUrlDifficultyClass
        {
            get => _settings.ConflictingWebhookUrlDifficultyClass;
            set => _settings.ConflictingWebhookUrlDifficultyClass = value;
        }

        private void ApplyWebhookUpdatesTimerInterval(bool stopWebhookUpdatesTimer = false)
        {
            if (!stopWebhookUpdatesTimer) _webhookUpdatesTimer.Change(TimeSpan.Zero, WebhookUpdatesTimerInterval);
            else _webhookUpdatesTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _webhookUpdatesTimerIsRunning = !stopWebhookUpdatesTimer;
        }

        public async Task SetWebhookAsync(string webhookUrl)
        {
            await WebhookUrlValidator.ValidateWebhookUrlAsync(webhookUrl);
            _webhookUrl = webhookUrl;
            ApplyWebhookUpdatesTimerInterval();
            _conflictingWebhookUrlTimeout.Reset();
        }

        public FakeWebhookInfo GetWebhookInfo()
        {
            return new FakeWebhookInfo
            {
                Url = _webhookUrl,
                Date = DateTime.Now
            };
        }

        public void DeleteWebhook()
        {
            ApplyWebhookUpdatesTimerInterval(stopWebhookUpdatesTimer: true);
            _webhookUrl = null;
        }

        public IEnumerable<FakeBotUpdate> GenerateRandomUpdates()
        {
            if (_randomUpdatesTimeout.IsNotElapsed)
                return Enumerable.Empty<FakeBotUpdate>();

            var updates = RandomUtils.NextUpdates();
            _randomUpdatesTimeout.Reset();

            return updates;
        }

        private async void HandleSendWebhookUpdatesCallback(object state)
        {
            if (EmulateFakeConflictingWebhookUrl()) return;
            try
            {
                var webhookUpdatesSender = _serviceProvider.GetRequiredService<IFakeOperationalClientWebhookUpdatesSender>();
                foreach (var update in GenerateRandomUpdates())
                {
                    await webhookUpdatesSender.SendUpdateAsync(_webhookUrl, update);
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Fake Operational Client post Webhook Updates operation failed, an exception has occurred.{Environment.NewLine}{e}");
            }
        }

        private bool EmulateFakeConflictingWebhookUrl()
        {
            if (!EmulateConflictingWebhookUrl || _conflictingWebhookUrlTimeout.IsNotElapsed)
                return false;
            if (!RandomUtils.NextBoolean(ConflictingWebhookUrlDifficultyClass))
            {
                _conflictingWebhookUrlTimeout.Reset();
                return false;
            }

            ApplyWebhookUpdatesTimerInterval(stopWebhookUpdatesTimer: true);
            _webhookUrl = FakeConflictingWebhookUrl;

            return true;
        }

        public void Dispose()
        {
            _webhookUpdatesTimer?.Dispose();
        }

        public class Settings
        {
            public Settings()
            {
            }

            public Settings(Settings settings)
            {
                WebhookUpdatesTimerInterval = settings.WebhookUpdatesTimerInterval;
                EmulateConflictingWebhookUrl = settings.EmulateConflictingWebhookUrl;
                ConflictingWebhookUrlDifficultyClass = settings.ConflictingWebhookUrlDifficultyClass;
            }

            public TimeSpan WebhookUpdatesTimerInterval { get; set; } = TimeSpan.FromSeconds(1);
            public bool EmulateConflictingWebhookUrl { get; set; } = false;
            public int ConflictingWebhookUrlDifficultyClass { get; set; } = 15;
        }
    }
}
