using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Domain;
using Cat.Infrastructure.Fake.BotApiComponents.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient
{
    public class FakeOperationalClientEmulatedState : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private const string FakeConflictingWebhookUrl = "*FakeConflictingWebhookUrl*";

        private readonly ConcurrentDictionary<string, string> _webhookUrlValidationTokensDictionary = new ConcurrentDictionary<string, string>();
        private readonly TimeSpan _webhookUrlValidationTimeout = TimeSpan.FromSeconds(10);

        private readonly Settings _settings;
        private readonly Timer _webhookUpdatesTimer;

        private string _webhookUrl;
        private bool _webhookUpdatesTimerIsRunning;

        private readonly IFakeOperationalClientTimeout _randomUpdatesTimeout; 
        private readonly IFakeOperationalClientTimeout _conflictingWebhookUrlTimeout;

        public ILogger<FakeOperationalClient> Logger { get; }
        public IFakeOperationalClientToken Token { get; }
        public IFakeOperationalClientRandomUtils RandomUtils { get; }

        public FakeOperationalClientEmulatedState(Settings settings, IFakeOperationalClientToken token, IServiceProvider serviceProvider, ILogger<FakeOperationalClient> logger)
        {
            _serviceProvider = serviceProvider;
            _settings = settings ?? new Settings();
            Logger = logger;
            Token = token;
            RandomUtils = new FakeOperationalClientRandomUtils();

            _webhookUpdatesTimer = new Timer(HandleSendWebhookUpdatesCallback, null, Timeout.Infinite, Timeout.Infinite);
            _webhookUpdatesTimerIsRunning = false;

            _randomUpdatesTimeout = new FakeOperationalClientTimeout(RandomUtils);
            _conflictingWebhookUrlTimeout = new FakeOperationalClientTimeout(RandomUtils);

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

        #region Webhook

        public async Task SetWebhookAsync(string webhookUrl)
        {
            await ValidateWebhookUrlAsync(webhookUrl);
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

        #endregion

        #region Webhook Updates

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

        private void ApplyWebhookUpdatesTimerInterval(bool stopWebhookUpdatesTimer = false)
        {
            if (!stopWebhookUpdatesTimer) _webhookUpdatesTimer.Change(TimeSpan.Zero, WebhookUpdatesTimerInterval);
            else _webhookUpdatesTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _webhookUpdatesTimerIsRunning = !stopWebhookUpdatesTimer;
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

        #endregion

        #region Random Updates

        public IEnumerable<FakeBotUpdate> GenerateRandomUpdates()
        {
            if (_randomUpdatesTimeout.IsNotElapsed)
                return Enumerable.Empty<FakeBotUpdate>();

            var updates = RandomUtils.NextUpdates();
            _randomUpdatesTimeout.Reset();

            return updates;
        }

        #endregion

        #region Webhook URL validation

        public void ConfirmWebhookUrlValidationToken(string validationToken, string webhookUrl)
        {
            _webhookUrlValidationTokensDictionary.TryAdd(validationToken, webhookUrl);
            EnsureWebhookUrlValidationTokenReleased(validationToken);
        }

        private void EnsureWebhookUrlValidationTokenReleased(string validationToken)
        {
            Task.Run(async () =>
            {
                await Task.Delay(_webhookUrlValidationTimeout); // todo: maybe there is a better alternative to Task.Delay?
                _webhookUrlValidationTokensDictionary.TryRemove(validationToken, out _);
            });
        }

        private async Task ValidateWebhookUrlAsync(string webhookUrl)
        {
            var validationToken = string.Empty;
            try
            {
                validationToken = await GenerateConfirmedWebhookUrlValidationTokenAsync(webhookUrl);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"The provided webhook URL ({webhookUrl.Bar()}) is invalid.", e);
            }
            finally
            {
                _webhookUrlValidationTokensDictionary.TryRemove(validationToken, out _);
            }
        }

        private async Task<string> GenerateConfirmedWebhookUrlValidationTokenAsync(string webhookUrl)
        {
            if (string.IsNullOrEmpty(webhookUrl) || string.IsNullOrWhiteSpace(webhookUrl))
                throw new ArgumentException("Webhook URL cannot be null, empty or whitespace.");

            var validationToken = Guid.NewGuid().ToString();
            var confirmationResponse = await RequestWebhookUrlValidationTokenConfirmationAsync(webhookUrl, validationToken);

            _webhookUrlValidationTokensDictionary.TryGetValue(validationToken, out var storedWebhookUrl);

            if (confirmationResponse.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"The server responded with an unacceptable status ({confirmationResponse.StatusCode:D} {confirmationResponse.StatusCode:G})."); // todo: HTTP status codes formatting
            if (storedWebhookUrl == null)
                throw new InvalidOperationException($"The validation token ({validationToken.Bar()}) was not confirmed.");
            if (!string.Equals(storedWebhookUrl, webhookUrl, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException($"The URL ({storedWebhookUrl.Bar()}) provided at confirmation of the validation token do not match the webhook URL.");

            return validationToken;
        }

        private Task<HttpResponseMessage> RequestWebhookUrlValidationTokenConfirmationAsync(string webhookUrl, string validationToken)
        {
            var confirmationRequestUpdate = new FakeBotUpdate { ValidationToken = validationToken };
            var webhookUpdatesSender = _serviceProvider.GetRequiredService<IFakeOperationalClientWebhookUpdatesSender>();
            webhookUpdatesSender.Timeout = _webhookUrlValidationTimeout;
            return webhookUpdatesSender.SendUpdateAsync(webhookUrl, confirmationRequestUpdate);
        }

        #endregion

        public void Dispose()
        {
            _webhookUpdatesTimer?.Dispose();
        }

        public class Settings
        {
            public TimeSpan WebhookUpdatesTimerInterval { get; set; } = TimeSpan.FromSeconds(1);
            public bool EmulateConflictingWebhookUrl { get; set; } = false;
            public int ConflictingWebhookUrlDifficultyClass { get; set; } = 15;
        }
    }
}
