using Cat.Domain.Repositories;

namespace Cat.Business.Services.SystemLogging
{

    public class RefresherLoggingService : SystemLoggingServiceBase
    {
        public RefresherLoggingService(ISystemLogEntriesRespository logEntriesRepo)
            : base(logEntriesRepo, "RefresherService")
        {
        }

        public override int DefaultSecondsThreshold()
        {
            // 1 week
            return 60 * 60 * 24 * 7;
        }
    }
}
