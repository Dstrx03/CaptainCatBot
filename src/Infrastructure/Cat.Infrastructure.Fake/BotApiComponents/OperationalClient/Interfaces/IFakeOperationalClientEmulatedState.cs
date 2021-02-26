using Cat.Application.BotUpdates.Commands.FakeBotUpdate;
using Cat.Infrastructure.Fake.BotApiComponents.Models;
using Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient.Interfaces
{
    public interface IFakeOperationalClientEmulatedState
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
}
