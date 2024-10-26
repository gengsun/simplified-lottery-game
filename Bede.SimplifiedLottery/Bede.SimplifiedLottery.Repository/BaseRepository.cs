using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Interfaces.Repositories;

namespace Bede.SimplifiedLottery.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected ICollection<TEntity> _entities = [];

        private int _id = 0;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public virtual void Add(TEntity entity)
        {
            Execute(() =>
            {
                entity.Id = ++_id;
                _entities.Add(entity);
            });
        }

        public IReadOnlyCollection<TEntity> GetAll() => _entities.ToList().AsReadOnly();

        protected void Execute(Action action)
        {
            _semaphore.Wait();

            action();

            _semaphore.Release();
        }
    }
}
