using System.Data.Entity;
using Cat.Domain.Entities;
using Cat.Domain.Repositories.Base;

namespace Cat.Domain.Repositories
{
    public interface ITestEntitiesRespository : IRepository<TestEntity, string>
    {
    }

    public class TestRespository : Repository<TestEntity, string>, ITestEntitiesRespository
    {
        public TestRespository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
