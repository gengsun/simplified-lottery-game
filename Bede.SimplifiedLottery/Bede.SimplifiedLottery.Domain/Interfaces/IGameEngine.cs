using Bede.SimplifiedLottery.Domain.DTOs;

namespace Bede.SimplifiedLottery.Domain.Interfaces
{
    public interface IGameEngine
    {
        InitialisationResult Initialise();

        PurchaseTicketsResult PurchaseTickets(int numOfUserTickets);

        DrawResult Draw();
    }
}
