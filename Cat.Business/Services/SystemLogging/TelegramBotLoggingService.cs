﻿using System;
using Cat.Business.Services.SystemLogging.Factory;
using Cat.Domain.Repositories;
using Microsoft.AspNet.SignalR;

namespace Cat.Business.Services.SystemLogging
{

    public class TelegramBotLoggingService : SystemLoggingServiceBase
    {
        public TelegramBotLoggingService(ISystemLogEntriesRespository logEntriesRepo)
            : base(logEntriesRepo, "Telegram Bot Logging Service", SystemLoggingServiceFactory.TelegramBotDescriptor)
        {
        }

        public override TimeSpan DefaultCleanThreshold()
        {
            // 3 days
            return TimeSpan.FromDays(3);
        }

        protected override IHubContext HubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<TelegramBotSystemLoggingServiceHub>();
        }
    }

    public class TelegramBotSystemLoggingServiceHub : Hub
    {
    }
}
