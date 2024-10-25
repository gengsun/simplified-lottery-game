using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Enums;
using Bede.SimplifiedLottery.Domain.Interfaces;
using Bede.SimplifiedLottery.Domain.Settings;

namespace Bede.SimplifiedLottery.GameEngine
{
    public class GameStrategy(GameSettings settings) : IGameStrategy
    {
        private readonly GameSettings _settings = settings;

        public int GetPlayerCount<T>() where T : Player, new()
        {
            return typeof(T) == typeof(UserPlayer)
                ? 1
                : new Random().Next(_settings.MinimumNumberOfPlayers - 1, _settings.MaximumNumberOfPlayers);
        }

        public int GetTicketCount<T>(int? userTicketCount = null) where T : Player, new()
        {
            return typeof(T) == typeof(UserPlayer) && userTicketCount.HasValue
                ? GetAllowedTicketCount(userTicketCount.Value)
                : new Random().Next(
                    GetAllowedTicketCount(_settings.MinimumAllowedTickets),
                    GetAllowedTicketCount(_settings.MaximumAllowedTickets) + 1);
        }

        public (int, int) GetTicketCountAndPrizeAmountForTier(DrawStatus drawStatus, int ticketCount)
        {
            return drawStatus switch
            {
                DrawStatus.GrandPrize => Calculate(ticketCount, _settings.GrandPriceRevenuePercentage),
                DrawStatus.SecondTier => Calculate(ticketCount, _settings.SecondTierRevenuePercentage, _settings.SecondTierTicketPercentage),
                DrawStatus.ThirdTier => Calculate(ticketCount, _settings.ThirdTierRevenuePercentage, _settings.ThirdTierTicketPercentage),
                _ => throw new NotImplementedException(nameof(drawStatus))
            };
        }

        public int GetStartBalance() => _settings.StartBalance;

        public int GetTicketPrice() => _settings.TicketPrice;

        private int GetAllowedTicketCount(int numOfTickets)
        {
            int allowedTickets = _settings.StartBalance / _settings.TicketPrice;
            return numOfTickets <= allowedTickets ? numOfTickets : allowedTickets;
        }

        private (int, int) Calculate(int totalTicketCount, int revenuePercentage, int? ticketPercentage = null)
        {
            int ticketCount = ticketPercentage.HasValue
                ? (int)Math.Round(totalTicketCount * ticketPercentage.Value / 100m, MidpointRounding.AwayFromZero)
                : 1;

            int prizeAmout = totalTicketCount * _settings.TicketPrice * revenuePercentage / 100 / ticketCount;

            return (ticketCount, prizeAmout);
        }
    }
}
