using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cat.Business.Services;
using Cat.Domain.Entities;
using Cat.Domain.Repositories;

namespace Cat.Web.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly ITestEntitiesRespository _testRepository;
        private readonly ITestEntityManager _testManager;

        public TestController(ITestEntitiesRespository testRepository, ITestEntityManager testManager)
        {
            _testRepository = testRepository;
            _testManager = testManager;
        }

        public async Task<ActionResult> Index()
        {
            return View(await _testRepository.GetAll().OrderBy(x => x.Name).ToListAsync());
        }

        public async Task<ActionResult> Details(string id)
        {
            return View(await _testRepository.GetByIdAsync(id));
        }

        public ActionResult Add()
        {
            return View(new TestEntity());
        }

        [HttpPost]
        public ActionResult Add(TestEntity entity)
        {
            _testManager.CreateTest(entity.Name);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            return View(await _testRepository.GetByIdAsync(id));
        }

        [HttpPost]
        public ActionResult Edit(TestEntity entity)
        {
            _testManager.EditTest(entity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Remove(string id)
        {
            await _testManager.RemoveTest(id);
            return RedirectToAction("Index");
        }
    }
}