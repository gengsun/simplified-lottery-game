using Bede.SimplifiedLottery.Domain.Entities;

namespace Bede.SimplifiedLottery.Domain.DTOs
{
    public record class PlayerDto(
        string Name,
        IReadOnlyCollection<Ticket> Tickets,
        string Winning);
}
