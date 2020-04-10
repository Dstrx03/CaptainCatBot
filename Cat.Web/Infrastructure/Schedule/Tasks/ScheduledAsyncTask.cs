using System;
using System.Data;
using System.Threading.Tasks;
using Cat.Business.Schedule.Tasks;

namespace Cat.Web.Infrastructure.Schedule.Tasks
{
    public class ScheduledAsyncTask<T> : ScheduledTaskBase, IScheduledAsyncTask where T : IScheduledAsyncTask
    {
        public async Task ExecuteAsync()
        {
            try
            {
                using (var tran = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        _log.DebugFormat("Scheduled async task '{0}' execution started", typeof(T).ToString());

                        var task = _container.GetInstance<T>();
                        await task.ExecuteAsync();
                        await _dbContext.SaveChangesAsync();
                        tran.Commit();

                        _log.DebugFormat("Scheduled async task '{0}' execution completed", typeof(T).ToString());
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error while executing scheduled async task '{0}': {1}", typeof(T).ToString(), e);
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