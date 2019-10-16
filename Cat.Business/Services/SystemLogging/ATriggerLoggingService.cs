using System;
using Cat.Domain.Repositories;
using Microsoft.AspNet.SignalR;

namespace Cat.Business.Services.SystemLogging
{

    public class ATriggerLoggingService : SystemLoggingServiceBase
    {
        public ATriggerLoggingService(ISystemLogEntriesRespository logEntriesRepo)
            : base(logEntriesRepo, "ATriggerService")
        {
        }

        public override TimeSpan CleanThreshold()
        {
            // 1 week
            return TimeSpan.FromDays(7);
        }

        protected override IHubContext HubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ATriggerServiceSystemLoggingServiceHub>();
        }
    }

    public class ATriggerServiceSystemLoggingServiceHub : Hub
    {
    }
}
