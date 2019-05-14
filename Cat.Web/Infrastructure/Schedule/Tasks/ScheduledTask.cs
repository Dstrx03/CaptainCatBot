using System;
using System.Data;
using System.Data.Entity;
using Cat.Business.Schedule.Tasks;
using Cat.Domain;
using Cat.Web.App_Start;
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
                        var task = _container.GetInstance<T>();
                        task.Execute();
                        _dbContext.SaveChanges();
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
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