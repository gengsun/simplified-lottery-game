namespace Bede.SimplifiedLottery.Domain.DTOs
{
    public record class PurchaseTicketsResult(int NumberOfCpuPlayers, IReadOnlyCollection<PlayerDto> Players);
}
