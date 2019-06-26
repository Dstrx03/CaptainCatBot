
using System;

namespace Telegram.Models
{
    public class TelegramStatusViewModel
    {
        public TelegramServiceStatus TelegramBotClientStatus { get; set; }

        public TelegramServiceStatus WebhookStatus { get; set; }



        public DateTime? WebhookUpdateDate { get; set; }

        public string WebhookUrl { get; set; }

        public bool? WebhookHasCustomCertificate { get; set; }

        public int? WebhookPendingUpdateCount { get; set; }

        public DateTime? WebhookLastErrorDate { get; set; }

        public string WebhookLastErrorMessage { get; set; }

        public int? WebhookMaxConnections { get; set; }

        public string WebhookAllowedUpdates { get; set; }

    }

    public enum TelegramServiceStatus
    {
        Unknown = 0,
        Stopped = 1,
        Running = 2,
        Ok = 3,
        Error = 4
    }
}
