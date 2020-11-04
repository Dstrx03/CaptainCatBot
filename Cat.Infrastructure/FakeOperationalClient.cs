using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cat.Application;
using Microsoft.Extensions.Logging;

namespace Cat.Infrastructure
{
    public class FakeOperationalClient
    {
        #region Const fields

        private const string SecretFakeToken = "0b858ebff3c55c563c4664aa8c3538763c24779b57e2742793e7f3c516156bbc"; // <= SLMX4ga5.t84Q
        private const string DummyText = @"
            A meme (/miːm/ MEEM) is an idea, behavior, or style that becomes a fad and spreads by means of imitation from person to person within a culture and often carries symbolic meaning representing a particular phenomenon or theme.$sentence$ 
            A meme acts as a unit for carrying cultural ideas, symbols, or practices, that can be transmitted from one mind to another through writing, speech, gestures, rituals, or other imitable phenomena with a mimicked theme.$sentence$
            Supporters of the concept regard memes as cultural analogues to genes in that they self-replicate, mutate, and respond to selective pressures.$sentence$
            Proponents theorize that memes are a viral phenomenon that may evolve by natural selection in a manner analogous to that of biological evolution.$sentence$
            Memes do this through the processes of variation, mutation, competition, and inheritance, each of which influences a meme's reproductive success.$sentence$
            Memes spread through the behavior that they generate in their hosts.$sentence$
            Memes that propagate less prolifically may become extinct, while others may survive, spread, and (for better or for worse) mutate.$sentence$
            Memes that replicate most effectively enjoy more success, and some may replicate effectively even when they prove to be detrimental to the welfare of their hosts.$sentence$
            A field of study called memetics arose in the 1990s to explore the concepts and transmission of memes in terms of an evolutionary model.$sentence$
            Criticism from a variety of fronts has challenged the notion that academic study can examine memes empirically.$sentence$
            However, developments in neuroimaging may make empirical study possible.$sentence$
            Some commentators in the social sciences question the idea that one can meaningfully categorize culture in terms of discrete units, and are especially critical of the biological nature of the theory's underpinnings.$sentence$
            Others have argued that this use of the term is the result of a misunderstanding of the original proposal.$sentence$
            The word meme itself is a neologism coined by Richard Dawkins, originating from his 1976 book The Selfish Gene.$sentence$
            Dawkins's own position is somewhat ambiguous.$sentence$
            He welcomed N. K. Humphrey's suggestion that ""memes should be considered as living structures, not just metaphorically"" and proposed to regard memes as ""physically residing in the brain.""$sentence$
            Later, he argued that his original intentions, presumably before his approval of Humphrey's opinion, had been simpler.
        ";

        #endregion

        private static readonly HttpClient HttpClient = new HttpClient();

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        private readonly string _fakeToken;
        private readonly TimeSpan _webhookTimerInterval;
        private readonly ILogger<FakeOperationalClient> _logger;
        private readonly Timer _webhookTimer;
        
        private string _webhookUrl;
        private DateTime _lastUpdateApplied;
        private int _updateTimeoutSeconds;

        public FakeOperationalClient(string fakeToken, TimeSpan webhookTimerInterval, ILogger<FakeOperationalClient> logger)
        {
            _fakeToken = fakeToken;
            _webhookTimerInterval = webhookTimerInterval;
            _logger = logger;
            _webhookTimer = new Timer(async s => await PostWebhookUpdatesAsync(), null, Timeout.Infinite, Timeout.Infinite);
            _lastUpdateApplied = DateTime.Now;
            _updateTimeoutSeconds = RandomUpdateTimeoutSeconds();
        }

        public Task<bool> ValidateClientAsync()
        {
            string hashedFakeToken = null;
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(_fakeToken));
                var builder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                hashedFakeToken = builder.ToString();
            }
            var tokenIsValid = SecretFakeToken == hashedFakeToken;
            return Task.FromResult(tokenIsValid);
        }

        public async Task SendFakeMessageAsync(string message)
        {
            await EnsureTokenIsValidAsync();
            _logger.LogDebug($"[SendFakeMessageAsync] '{message}'"); // todo: apply single text format convention for all Fake Bot API components log messages
        }

        public async Task SetWebhookAsync(string webhookUrl)
        {
            await EnsureTokenIsValidAsync();
            _webhookUrl = webhookUrl;
            _webhookTimer.Change(0, (int)_webhookTimerInterval.TotalMilliseconds);
        }

        public async Task<FakeWebhookInfo> GetWebhookInfoAsync()
        {
            await EnsureTokenIsValidAsync();
            return new FakeWebhookInfo
            {
                Url = _webhookUrl, 
                Date = DateTime.Now
            };
        }

        public async Task DeleteWebhookAsync()
        {
            await EnsureTokenIsValidAsync();
            _webhookUrl = null;
            _webhookTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private async Task PostWebhookUpdatesAsync()
        {
            var updates = await GenerateRandomUpdates();
            foreach (var update in updates)
            {
                var stringContent = new StringContent(JsonSerializer.Serialize(update, _serializerOptions), Encoding.UTF8, "application/json");
                await HttpClient.PostAsync(_webhookUrl, stringContent);
            }
        }

        public async Task<IEnumerable<FakeBotUpdate>> GetUpdatesAsync()
        {
            await EnsureTokenIsValidAsync();
            return await GenerateRandomUpdates();
        }

        private Task<IEnumerable<FakeBotUpdate>> GenerateRandomUpdates()
        {
            var updates = new List<FakeBotUpdate>();
            if ((DateTime.Now - _lastUpdateApplied).TotalSeconds < _updateTimeoutSeconds) 
                return Task.FromResult<IEnumerable<FakeBotUpdate>>(updates);
            var updatesCount = RandomUpdatesCount();
            for (var i = 0; i < updatesCount; i++)
            {
                updates.Add(new FakeBotUpdate { Message = RandomUpdateMessage() });
            }
            _lastUpdateApplied = DateTime.Now;
            _updateTimeoutSeconds = RandomUpdateTimeoutSeconds();
            return Task.FromResult<IEnumerable<FakeBotUpdate>>(updates);
        }

        private int RandomUpdatesCount()
        {
            var updatesCount = 0;

            var roll = _random.Next(1, 21);
            if (roll >= 14 && roll <= 18) updatesCount = 1;
            else if (roll == 19) updatesCount = 2;
            else if (roll == 20) updatesCount = 3;

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

        private string RandomUpdateMessage()
        {
            var messages = DummyText.Split("$sentence$", StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).ToArray();
            return messages[_random.Next(0, messages.Length)];
        }

        private async Task EnsureTokenIsValidAsync()
        {
            // todo: throw random exceptions on public methods calls for debug?
            if (!await ValidateClientAsync())
                throw new InvalidOperationException($"TODO token is invalid: '{_fakeToken}'"); // todo: apply single text format convention for all Fake Bot API components log messages
        }
    }
}
