using Bede.SimplifiedLottery.Domain.Entities;

namespace Bede.SimplifiedLottery.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity entity);
        IReadOnlyCollection<TEntity> GetAll();

        void Reset();
    }
}
