using Cat.Domain.Repositories;

namespace Cat.Business.Services.SystemLogging
{

    public class ATriggerLoggingService : SystemLoggingServiceBase
    {
        public ATriggerLoggingService(ISystemLogEntriesRespository logEntriesRepo)
            : base(logEntriesRepo, "ATriggerService")
        {
        }
    }
}
