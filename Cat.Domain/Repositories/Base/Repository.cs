using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Cat.Domain.Repositories.Base
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IConvertible
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(TKey id);

        Task<TEntity> GetByIdAsync(TKey id);

        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Remove(TKey id);

        Task RemoveAsync(TKey id);

        void Update(TEntity entity);

        void SaveChanges();

        Task SaveChangesAsync();
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

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await Objects.FindAsync(id);
        }

        public virtual void Add(TEntity entity)
        {
            Objects.Add(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Objects.Remove(entity);
        }

        public virtual void Remove(TKey id)
        {
            Objects.Remove(GetById(id));
        }

        public virtual async Task RemoveAsync(TKey id)
        {
            Objects.Remove(await GetByIdAsync(id));
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
