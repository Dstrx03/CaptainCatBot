using System.Data.Entity;
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
    public abstract class ScheduledTaskBase
    {
        protected readonly AppDbContext _dbContext;
        protected readonly IContainer _container;

        protected static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ScheduledTaskBase()
        {
            _dbContext = new AppDbContext();
            _container = StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer();
            _container.Configure(x =>
            {
                x.For<AppDbContext>().Use(_dbContext);
                x.For<DbContext>().Use(_dbContext);
            });
        }
    }
}