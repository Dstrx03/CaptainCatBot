using System.Data.Entity;
using Cat.Domain.Entities.SystemLog;
using Cat.Domain.Repositories.Base;

namespace Cat.Domain.Repositories
{
    public interface ISystemLogEntriesRespository : IRepository<SystemLogEntry, string>
    {
    }

    public class SystemLogEntriesRespository : Repository<SystemLogEntry, string>, ISystemLogEntriesRespository
    {
        public SystemLogEntriesRespository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
