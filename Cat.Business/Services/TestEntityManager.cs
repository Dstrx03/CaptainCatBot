
using Cat.Domain.Entities;
using Cat.Domain.Repositories;

namespace Cat.Business.Services
{
    public interface ITestEntityManager
    {
        TestEntity CreateTest(string name);

        void RemoveTest(string id);

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

        public void RemoveTest(string id)
        {
            _testRespository.Remove(id);
        }

        public TestEntity EditTest(TestEntity entity)
        {
            _testRespository.Update(entity);
            return entity;
        }
    }
}
