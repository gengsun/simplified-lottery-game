using Bede.SimplifiedLottery.Domain.Entities;

namespace Bede.SimplifiedLottery.Domain.DTOs
{
    public record class DrawResult(
        IReadOnlyCollection<Player> Players,
        IReadOnlyCollection<Ticket> Tickets,
        string HouseRevenue);
}
