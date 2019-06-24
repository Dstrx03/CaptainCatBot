using Cat.Domain.Repositories;

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
            // 1 week
            return 60 * 60 * 24 * 7;
        }
    }
}
