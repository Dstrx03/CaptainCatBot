
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
            _testRespository.SaveChanges();
            return entity;
        }

        public void RemoveTest(string id)
        {
            _testRespository.Remove(id);
            _testRespository.SaveChanges();
        }

        public TestEntity EditTest(TestEntity entity)
        {
            var dbEntity = _testRespository.GetById(entity.Id);
            dbEntity.Name = entity.Name;
            _testRespository.Update(dbEntity);
            _testRespository.SaveChanges();
            return dbEntity;
        }
    }
}
