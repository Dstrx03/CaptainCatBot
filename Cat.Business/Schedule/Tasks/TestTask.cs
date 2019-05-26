﻿using System;
using Cat.Business.Services;

namespace Cat.Business.Schedule.Tasks
{
    public class TestTask : IScheduledTask
    {
        private readonly ITestEntityManager _testManager;

        public TestTask(ITestEntityManager testManager)
        {
            _testManager = testManager;
        }

        public void Execute()
        {
            _testManager.CreateTest(string.Format("Test Entity generated by task at {0}", DateTime.Now));
        }
    }
}