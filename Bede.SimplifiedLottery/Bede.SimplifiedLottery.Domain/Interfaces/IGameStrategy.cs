using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Enums;

namespace Bede.SimplifiedLottery.Domain.Interfaces
{
    public interface IGameStrategy
    {
        (int, int) GetTicketCountAndPrizeAmountForTier(DrawStatus drawStatus, int ticketCount);
        int GetPlayerCount<T>() where T : Player, new();
        int GetTicketCount<T>(int? userTicketCount = null) where T : Player, new();
        int GetStartBalance();
        int GetTicketPrice();
    }
}
