using System.Linq.Expressions;
using FUMiniHotelSystem.DataAccess.Database;
using FUMiniHotelSystem.DataAccess.Interfaces;

namespace FUMiniHotelSystem.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly InMemoryDatabase _database;

        protected BaseRepository()
        {
            _database = InMemoryDatabase.Instance;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.FromResult(GetAll());
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await Task.FromResult(GetById(id));
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(Find(predicate));
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            return await Task.FromResult(Add(entity));
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await Task.Run(() => Update(entity));
        }

        public virtual async Task DeleteAsync(int id)
        {
            await Task.Run(() => Delete(id));
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await Task.FromResult(Exists(id));
        }

        protected abstract IEnumerable<T> GetAll();
        protected abstract T? GetById(int id);
        protected abstract IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        protected abstract T Add(T entity);
        protected abstract void Update(T entity);
        protected abstract void Delete(int id);
        protected abstract bool Exists(int id);
    }
}

