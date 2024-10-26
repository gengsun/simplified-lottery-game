using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Interfaces.Repositories;

namespace Bede.SimplifiedLottery.Repository
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        public IReadOnlyCollection<Player> GetAllByType<T>() where T : Player, new()
            => _entities.Where(entity => entity.GetType() == typeof(T)).ToList().AsReadOnly();

        public Player GetUserPlayer()
            => _entities.SingleOrDefault(p => p.GetType() == typeof(UserPlayer)) ?? throw new ArgumentNullException("User Player");
    }
}
