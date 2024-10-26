using Bede.SimplifiedLottery.Domain.Entities;

namespace Bede.SimplifiedLottery.Domain.DTOs
{
    public record class DrawResult(
        string GrandPrize,
        string SecondTierPrize,
        string ThirdTierPrize,
        IReadOnlyCollection<PlayerDto> Players,
        IReadOnlyCollection<Ticket> Tickets,
        string HouseRevenue);
}
