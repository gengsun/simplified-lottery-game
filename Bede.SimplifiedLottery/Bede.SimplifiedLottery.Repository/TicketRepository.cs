using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Enums;
using Bede.SimplifiedLottery.Domain.Exceptions;
using Bede.SimplifiedLottery.Domain.Interfaces.Repositories;

namespace Bede.SimplifiedLottery.Repository
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public IReadOnlyCollection<Ticket> GetWinners()
            => _entities.Where(entity => entity.DrawStatus != DrawStatus.NotSet).ToList().AsReadOnly();

        public IReadOnlyCollection<Ticket> GetNonWinners()
            => _entities.Where(entity => entity.DrawStatus == DrawStatus.NotSet).ToList().AsReadOnly();

        public int GetCount() => _entities.Count;

        public void Update(int id, DrawStatus drawStatus, int PrizeAmount)
        {
            var ticket = _entities.SingleOrDefault(entity => entity.Id == id) ?? throw new NotFoundException($"Ticket {id} not found");

            Execute(() =>
            {
                ticket.DrawStatus = drawStatus;
                ticket.PrizeAmount = PrizeAmount;
            });
        }
    }
}
