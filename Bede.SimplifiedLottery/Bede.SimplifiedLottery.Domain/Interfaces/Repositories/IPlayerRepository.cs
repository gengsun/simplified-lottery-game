using Bede.SimplifiedLottery.Domain.Entities;

namespace Bede.SimplifiedLottery.Domain.Interfaces.Repositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Player GetUserPlayer();

        IReadOnlyCollection<Player> GetAllByType<T>() where T : Player, new();
    }
}
