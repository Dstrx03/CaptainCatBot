using System;
using Cat.Domain.Repositories;
using Microsoft.AspNet.SignalR;

namespace Cat.Business.Services.SystemLogging
{

    public class RefresherLoggingService : SystemLoggingServiceBase
    {
        public RefresherLoggingService(ISystemLogEntriesRespository logEntriesRepo)
            : base(logEntriesRepo, "RefresherService")
        {
        }

        public override TimeSpan CleanThreshold()
        {
            // 1 week
            return TimeSpan.FromDays(7);
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
