using System.Threading.Tasks;
using Cat.Domain.Entities;
using Cat.Domain.Repositories;

namespace Cat.Business.Services
{
    public interface ITestEntityManager
    {
        TestEntity CreateTest(string name);

        Task RemoveTest(string id);

        TestEntity EditTest(TestEntity entity);
    }

    public class TestEntityManager : ITestEntityManager
    {
        private readonly ITestEntitiesRespository _testRespository;

        public TestEntityManager(ITestEntitiesRespository testRespository)
        {
            _testRespository = testRespository;
        }

        public TestEntity CreateTest(string name)
        {
            var entity = new TestEntity { Name = name };
            _testRespository.Add(entity);
            return entity;
        }

        public async Task RemoveTest(string id)
        {
            await _testRespository.RemoveAsync(id);
        }

        public TestEntity EditTest(TestEntity entity)
        {
            _testRespository.Update(entity);
            return entity;
        }
    }
}
