using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Exceptions;
using Bede.SimplifiedLottery.Domain.Interfaces.Repositories;

namespace Bede.SimplifiedLottery.Repository
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        public string GetNameById(int id)
        {
            var player = _entities.SingleOrDefault(entity => entity.Id == id) ?? throw new NotFoundException($"Player {id} not found");

            return player.Name;
        }

        public IReadOnlyCollection<Player> GetAllByType<T>() where T : Player, new()
            => _entities.Where(entity => entity.GetType() == typeof(T)).ToList().AsReadOnly();

        public Player GetUserPlayer()
            => _entities.SingleOrDefault(p => p.GetType() == typeof(UserPlayer)) ?? throw new NotFoundException("User Player not found");
    }
}
