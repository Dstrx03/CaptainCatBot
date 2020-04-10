﻿using System;
using System.Data;
using Cat.Business.Schedule.Tasks;

namespace Cat.Web.Infrastructure.Schedule.Tasks
{
    public class ScheduledTask<T> : ScheduledTaskBase, IScheduledTask where T : IScheduledTask
    {
        public virtual void Execute()
        {
            try
            {
                using (var tran = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        _log.DebugFormat("Scheduled task '{0}' execution started", typeof(T).ToString());

                        var task = _container.GetInstance<T>();
                        task.Execute();
                        _dbContext.SaveChanges();
                        tran.Commit();

                        _log.DebugFormat("Scheduled task '{0}' execution completed", typeof(T).ToString());
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error while executing scheduled task '{0}': {1}", typeof(T).ToString(), e);
                        throw;
                    }
                }
            }
            finally
            {
                _container.Dispose();
            }
        }
    }
}