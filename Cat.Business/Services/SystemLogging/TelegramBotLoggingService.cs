using Cat.Domain.Repositories;
using Microsoft.AspNet.SignalR;

namespace Cat.Business.Services.SystemLogging
{

    public class TelegramBotLoggingService : SystemLoggingServiceBase
    {
        public TelegramBotLoggingService(ISystemLogEntriesRespository logEntriesRepo)
            : base(logEntriesRepo, "TelegramBot")
        {
        }

        public override int DefaultSecondsThreshold()
        {
            // 3 days
            return 60 * 60 * 24 * 3;
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
