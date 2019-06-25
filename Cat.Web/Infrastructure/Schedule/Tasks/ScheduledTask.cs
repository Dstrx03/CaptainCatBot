using System;
using System.Data;
using System.Data.Entity;
using Cat.Business.Schedule.Tasks;
using Cat.Domain;
using Cat.Web.App_Start;
using log4net;
using StructureMap;

namespace Cat.Web.Infrastructure.Schedule.Tasks
{
    /**
     * <summary>
     * This class provides database context and DI infrastructure for Hangfire jobs execution.
     * </summary>
     */
    public class ScheduledTask<T> : IScheduledTask where T : IScheduledTask
    {
        private readonly AppDbContext _dbContext;
        private readonly IContainer _container;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ScheduledTask()
        {
            _dbContext = new AppDbContext();
            _container = StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer();
            _container.Configure(x =>
            {
                x.For<AppDbContext>().Use(_dbContext);
                x.For<DbContext>().Use(_dbContext);
            });
        }

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