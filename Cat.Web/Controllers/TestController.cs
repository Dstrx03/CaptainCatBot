using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Index()
        {
            return View(_testRepository.GetAll().OrderBy(x => x.Name));
        }

        public ActionResult Details(string id)
        {
            return View(_testRepository.GetById(id));
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

        public ActionResult Edit(string id)
        {
            return View(_testRepository.GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(TestEntity entity)
        {
            _testManager.EditTest(entity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Remove(string id)
        {
            _testManager.RemoveTest(id);
            return RedirectToAction("Index");
        }
    }
}