using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Enums;

namespace Bede.SimplifiedLottery.Domain.Interfaces.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        IReadOnlyCollection<Ticket> GetWinners();

        IReadOnlyCollection<Ticket> GetNonWinners();

        int GetCount();

        void Update(int id, DrawStatus drawStatus, int PrizeAmount);
    }
}
