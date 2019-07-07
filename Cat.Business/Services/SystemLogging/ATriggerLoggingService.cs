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

        public override int DefaultSecondsThreshold()
        {
            // 1 week
            return 60 * 60 * 24 * 7;
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
