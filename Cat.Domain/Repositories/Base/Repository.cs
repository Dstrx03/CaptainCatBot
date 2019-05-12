using System;
using System.Data.Entity;
using System.Linq;

namespace Cat.Domain.Repositories.Base
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IConvertible
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(TKey id);

        void Add(TEntity entity);

        void Remove(TKey id);

        void Update(TEntity entity);

        void SaveChanges();
    }

    public abstract class Repository<TEntity, TKey> : EntityFrameworkRepository<TEntity>, IRepository<TEntity, TKey> 
        where TEntity : class 
        where TKey : IConvertible
    {
        public Repository(DbContext dbContext) : base(dbContext)
        {
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Objects;
        }

        public virtual TEntity GetById(TKey id)
        {
            return Objects.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            Objects.Add(entity);
        }

        public virtual void Remove(TKey id)
        {
            Objects.Remove(GetById(id));
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
