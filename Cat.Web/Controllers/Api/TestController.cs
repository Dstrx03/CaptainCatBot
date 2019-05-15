using System.Web.Http;
using Cat.Business.Services;
using Cat.Domain.Entities;
using Cat.Domain.Repositories;

namespace Cat.Web.Controllers.Api
{
    [Authorize]
    public class TestController : ApiController
    {
        private readonly ITestEntitiesRespository _testRespository;
        private readonly ITestEntityManager _testManager;

        public TestController(ITestEntitiesRespository testRespository, ITestEntityManager testManager)
        {
            _testRespository = testRespository;
            _testManager = testManager;
        }

        [HttpPost]
        public TestEntity AddEntity(string name)
        {
            return _testManager.CreateTest(name);
        }

        [HttpGet]
        public TestEntity GetEntity(string id)
        {
            return _testRespository.GetById(id);
        }
    }
}
