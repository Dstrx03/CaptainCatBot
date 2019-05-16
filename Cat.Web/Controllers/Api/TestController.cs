using System.Threading.Tasks;
using System.Web.Http;
using Cat.Business.Services;
using Cat.Domain.Entities;
using Cat.Domain.Repositories;

namespace Cat.Web.Controllers.Api
{
    //[Authorize]
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
        public async Task<TestEntity> GetEntity(string id)
        {
            return await _testRespository.GetByIdAsync(id);
        }

        [HttpDelete]
        public async Task RemoveEntity(string id)
        {
            await _testManager.RemoveTest(id);
        }
    }
}
