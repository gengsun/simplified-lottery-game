namespace Bede.SimplifiedLottery.Domain.DTOs
{
    public record class InitialisationResult(
        string UserPlayerName,
        string StartBalance,
        string TicketPrice);
}
