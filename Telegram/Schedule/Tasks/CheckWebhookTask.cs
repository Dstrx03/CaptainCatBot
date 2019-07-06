﻿using System.ComponentModel;
using Cat.Business.Schedule.Tasks;
using log4net;

namespace Telegram.Schedule.Tasks
{
    public class CheckWebhookTask : IScheduledTask
    {
        private readonly ITelegramService _telegramService;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CheckWebhookTask(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public void Execute()
        {
            _telegramService.CheckWebhook();
        }
    }
}