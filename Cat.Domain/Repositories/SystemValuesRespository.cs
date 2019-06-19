using System.Data.Entity;
using Cat.Domain.Entities.SystemValues;
using Cat.Domain.Repositories.Base;

namespace Cat.Domain.Repositories
{
    public interface ISystemValuesRespository : IRepository<SystemValue, string>
    {
    }

    public class SystemValuesRespository : Repository<SystemValue, string>, ISystemValuesRespository
    {
        public SystemValuesRespository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
