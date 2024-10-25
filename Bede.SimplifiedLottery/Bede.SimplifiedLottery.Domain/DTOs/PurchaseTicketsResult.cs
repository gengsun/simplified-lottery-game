using Bede.SimplifiedLottery.Domain.Entities;

namespace Bede.SimplifiedLottery.Domain.DTOs
{
    public record class PurchaseTicketsResult(
        int NumberOfCpuPlayers,
        IReadOnlyCollection<Player> Players,
        IReadOnlyCollection<Ticket> Tickets);
}
