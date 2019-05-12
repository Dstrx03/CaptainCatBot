using System.Data.Entity;

namespace Cat.Domain.Repositories.Base
{
    public abstract class EntityFrameworkRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<T> Objects
        {
            get { return _dbContext.Set<T>(); }
        }

        protected virtual DbContext Context
        {
            get { return _dbContext; }
        }

    }
}
