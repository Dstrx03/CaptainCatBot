using Cat.Application;
using Cat.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Cat.Infrastructure
{
    public class FakeOperationalClientHelper : IDisposable
    {
        #region Const fields

        private const string SecretFakeToken = "0b858ebff3c55c563c4664aa8c3538763c24779b57e2742793e7f3c516156bbc"; // <= SLMX4ga5.t84Q
        private const string FakeConflictingWebhookUrl = "*FakeConflictingWebhookUrl*";
        private const string UpdateMessages = @"
                                    Hwæt! Wé Gárdena in géardagum þéodcyninga þrym gefrúnon·$msg_sep$
                                    hú ðá æþelingas ellen fremedon.$msg_sep$
                                    Oft Scyld Scéfing sceaþena þréatum monegum maégþum meodosetla oftéah·$msg_sep$
                                    egsode Eorle syððan aérest wearð féasceaft funden hé þæs frófre gebád·$msg_sep$
                                    wéox under wolcnum·$msg_sep$
                                    weorðmyndum þáh oð þæt him aéghwylc þára ymbsittendra ofer hronráde hýran scolde, gomban gyldan·$msg_sep$
                                    þæt wæs gód cyning.$msg_sep$
                                    Ðaém eafera wæs æfter cenned geong in geardum þone god sende folce tó frófre·$msg_sep$
                                    fyrenðearfe ongeat·$msg_sep$
                                    þæt híe aér drugon aldorléase lange hwíle·$msg_sep$
                                    him þæs líffréä wuldres wealdend woroldáre forgeaf: Béowulf wæs bréme --blaéd wíde sprang-- Scyldes eafera Scedelandum in.$msg_sep$
                                    Swá sceal geong guma góde gewyrcean fromum feohgiftum on fæder bearme þæt hine on ylde eft gewunigen wilgesíþas þonne wíg cume·$msg_sep$
                                    léode gelaésten: lofdaédum sceal in maégþa gehwaére man geþéön.$msg_sep$
                                    Him ðá Scyld gewát tó gescæphwíle felahrór féran on fréan waére·$msg_sep$
                                    hí hyne þá ætbaéron tó brimes faroðe swaése gesíþas swá hé selfa bæd þenden wordum wéold wine Scyldinga léof landfruma lange áhte·$msg_sep$
                                    þaér æt hýðe stód hringedstefna ísig ond útfús æþelinges fær·$msg_sep$
                                    álédon þá léofne þéoden béaga bryttan on bearm scipes maérne be mæste·$msg_sep$
                                    þaér wæs mádma fela of feorwegum frætwa gelaéded·$msg_sep$
                                    ne hýrde ic cýmlícor céol gegyrwan hildewaépnum ond heaðowaédum billum ond byrnum·$msg_sep$
                                    him on bearme læg mádma mænigo þá him mid scoldon on flódes aéht feor gewítan·$msg_sep$
                                    nalæs hí hine laéssan lácum téodan þéodgestréonum þonne þá dydon þe hine æt frumsceafte forð onsendon aénne ofer ýðe umborwesende·$msg_sep$
                                    þá gýt híe him ásetton segen gyldenne héah ofer héafod·$msg_sep$
                                    léton holm beran·$msg_sep$
                                    géafon on gársecg·$msg_sep$
                                    him wæs geómor sefa murnende mód·$msg_sep$
                                    men ne cunnon secgan tó sóðe seleraédenne hæleð under heofenum hwá þaém hlæste onféng.                           
                                ";

        #endregion

        private static readonly HttpClient HttpClient = new HttpClient(); // todo: try (consider) to use IHttpClientFactory for HttpClients instantiation!

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        private readonly ConcurrentDictionary<string, string> _webhookUrlValidationTokensDictionary = new ConcurrentDictionary<string, string>();
        private readonly TimeSpan _webhookUrlValidationTimeout = TimeSpan.FromMinutes(1);

        private readonly Settings _settings;
        private readonly Timer _webhookUpdatesTimer;

        private string _webhookUrl;
        private DateTime _lastUpdateTimeoutReset;
        private int _updateTimeoutSeconds;
        private bool _webhookUpdatesTimerIsRunning;

        private DateTime _lastConflictingWebhookUrlTimeoutReset;
        private int _conflictingWebhookUrlTimeoutSeconds;

        public ILogger<FakeOperationalClient> OperationalClientLogger { get; }

        public FakeOperationalClientHelper(Settings settings, ILogger<FakeOperationalClient> logger)
        {
            _settings = settings ?? new Settings();
            OperationalClientLogger = logger;
            _webhookUpdatesTimer = new Timer(HandlePostWebhookUpdatesCallback, null, Timeout.Infinite, Timeout.Infinite);
            _webhookUpdatesTimerIsRunning = false;
            RandomUpdatesResetTimeout();
            ConflictingWebhookUrlResetTimeout();
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
                if (value) ConflictingWebhookUrlResetTimeout();
            }
        }

        public int ConflictingWebhookUrlDifficultyClass
        {
            get => _settings.ConflictingWebhookUrlDifficultyClass;
            set => _settings.ConflictingWebhookUrlDifficultyClass = value;
        }

        #region Helper common methods

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            string hashedFakeToken = null;
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(token.Trim()));
                var builder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                hashedFakeToken = builder.ToString();
            }
            return SecretFakeToken == hashedFakeToken;
        }

        public bool RandomBoolean(int difficultyClass)
        {
            if (difficultyClass > 20) difficultyClass = 20;
            if (difficultyClass < 1) difficultyClass = 1;
            var roll = _random.Next(1, 21);
            if (roll == 1) return false;
            return roll >= difficultyClass;
        }

        #endregion

        #region Webhook

        public async Task SetWebhookAsync(string webhookUrl)
        {
            await ValidateWebhookUrlAsync(webhookUrl);
            _webhookUrl = webhookUrl;
            ApplyWebhookUpdatesTimerInterval();
            ConflictingWebhookUrlResetTimeout();
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

        private async void HandlePostWebhookUpdatesCallback(object state)
        {
            if (EmulateFakeConflictingWebhookUrl()) return;
            try
            {
                foreach (var update in GenerateRandomUpdates())
                {
                    await HttpClient.PostAsync(_webhookUrl, CreateUpdateStringContent(update));
                }
            }
            catch (Exception e)
            {
                OperationalClientLogger.LogError($"Fake Operational Client post Webhook Updates operation failed, an exception has occurred.{Environment.NewLine}{e}");
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
            if (!EmulateConflictingWebhookUrl ||
                (DateTime.Now - _lastConflictingWebhookUrlTimeoutReset).TotalSeconds < _conflictingWebhookUrlTimeoutSeconds ||
                !RandomBoolean(ConflictingWebhookUrlDifficultyClass))
                return false;
            ApplyWebhookUpdatesTimerInterval(stopWebhookUpdatesTimer: true);
            _webhookUrl = FakeConflictingWebhookUrl;
            return true;
        }

        private void ConflictingWebhookUrlResetTimeout()
        {
            _lastConflictingWebhookUrlTimeoutReset = DateTime.Now;
            _conflictingWebhookUrlTimeoutSeconds = RandomUpdateTimeoutSeconds();
        }

        #endregion

        #region Random Updates

        public IEnumerable<FakeBotUpdate> GenerateRandomUpdates()
        {
            if ((DateTime.Now - _lastUpdateTimeoutReset).TotalSeconds < _updateTimeoutSeconds)
                return Enumerable.Empty<FakeBotUpdate>();
            var updatesCount = RandomUpdatesCount();
            if (updatesCount == 0)
                return Enumerable.Empty<FakeBotUpdate>();
            var updates = new List<FakeBotUpdate>();
            for (var i = 0; i < updatesCount; i++)
            {
                updates.Add(new FakeBotUpdate { Message = RandomUpdateMessage() });
            }
            RandomUpdatesResetTimeout();
            return updates;
        }

        private void RandomUpdatesResetTimeout()
        {
            _lastUpdateTimeoutReset = DateTime.Now;
            _updateTimeoutSeconds = RandomUpdateTimeoutSeconds();
        }

        private string RandomUpdateMessage()
        {
            var messages = UpdateMessages.Split("$msg_sep$", StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).ToArray();
            return messages[_random.Next(0, messages.Length)];
        }

        private int RandomUpdatesCount()
        {
            var updatesCount = 0;

            var roll = _random.Next(1, 21);
            if (roll >= 14 && roll <= 17) updatesCount = 1;
            else if (roll == 17) updatesCount = 2;
            else if (roll == 19) updatesCount = 3;
            else if (roll == 20) updatesCount = _random.Next(3, 11);

            return updatesCount;
        }

        private int RandomUpdateTimeoutSeconds()
        {
            var minTimeoutSeconds = 0;
            var maxTimeoutSeconds = 0;

            var roll = _random.Next(1, 21);
            if (roll == 1)
            {
                minTimeoutSeconds = (int)TimeSpan.FromMinutes(45).TotalSeconds;
                maxTimeoutSeconds = (int)TimeSpan.FromHours(3).TotalSeconds;
            }
            if (roll >= 2 && roll <= 8)
            {
                minTimeoutSeconds = (int)TimeSpan.FromMinutes(5).TotalSeconds;
                maxTimeoutSeconds = (int)TimeSpan.FromMinutes(30).TotalSeconds;
            }
            if (roll >= 9 && roll <= 15)
            {
                minTimeoutSeconds = 45;
                maxTimeoutSeconds = (int)TimeSpan.FromMinutes(2).TotalSeconds;
            }
            if (roll >= 16 && roll <= 18)
            {
                minTimeoutSeconds = 10;
                maxTimeoutSeconds = 15;
            }
            if (roll == 19)
            {
                minTimeoutSeconds = 0;
                maxTimeoutSeconds = 5;
            }

            return _random.Next(minTimeoutSeconds, maxTimeoutSeconds + 1);
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

        private async Task<HttpResponseMessage> RequestWebhookUrlValidationTokenConfirmationAsync(string webhookUrl, string validationToken)
        {
            var confirmationRequestUpdate = new FakeBotUpdate { ValidationToken = validationToken };
            using var cts = new CancellationTokenSource((int)_webhookUrlValidationTimeout.TotalMilliseconds);
            return await HttpClient.PostAsync(webhookUrl, CreateUpdateStringContent(confirmationRequestUpdate), cts.Token);
        }

        #endregion

        private StringContent CreateUpdateStringContent(FakeBotUpdate update) =>
            new StringContent(JsonSerializer.Serialize(update, _serializerOptions), Encoding.UTF8, "application/json");

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
