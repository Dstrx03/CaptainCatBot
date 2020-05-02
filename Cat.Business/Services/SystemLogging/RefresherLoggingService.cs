﻿using System;
using Cat.Business.Services.SystemLogging.Factory;
using Cat.Domain.Repositories;
using Microsoft.AspNet.SignalR;

namespace Cat.Business.Services.SystemLogging
{

    public class RefresherLoggingService : SystemLoggingServiceBase
    {
        public RefresherLoggingService(ISystemLogEntriesRespository logEntriesRepo)
            : base(logEntriesRepo, "Refresher Logging Service", SystemLoggingServiceFactory.RefresherServiceDescriptor)
        {
        }

        public override TimeSpan DefaultCleanThreshold()
        {
            // 3 days
            return TimeSpan.FromDays(3);
        }

        protected override IHubContext HubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<RefresherServiceSystemLoggingServiceHub>();
        }
    }

    public class RefresherServiceSystemLoggingServiceHub : Hub
    {
    }
}
