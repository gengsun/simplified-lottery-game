using Bede.SimplifiedLottery.Domain.DTOs;
using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Enums;
using Bede.SimplifiedLottery.Domain.Extensions;
using Bede.SimplifiedLottery.Domain.Interfaces;

namespace Bede.SimplifiedLottery.GameEngine
{
    public class GameEngine(IGameStrategy strategy) : IGameEngine
    {
        private readonly IGameStrategy _strategy = strategy;

        private int _playerId = 0;
        private readonly ICollection<Player> _players = [];

        private int _ticketId = 0;
        private readonly ICollection<Ticket> _tickets = [];

        private readonly SemaphoreSlim _gameEngineSemaphore = new(1, 1);

        public InitialisationResult Initialise()
        {
            CreatePlayers<UserPlayer>();
            CreatePlayers<CpuPlayer>();

            var userPlayer = _players.SingleOrDefault(p => p.GetType() == typeof(UserPlayer)) ?? throw new ArgumentNullException("User Player");

            return new InitialisationResult(
                userPlayer.Name,
                _strategy.GetStartBalance().ToDisplayString(),
                _strategy.GetTicketPrice().ToDisplayString());
        }

        public PurchaseTicketsResult PurchaseTickets(int numOfUserTickets)
        {
            Purchase<UserPlayer>(numOfUserTickets);
            Purchase<CpuPlayer>();

            return new PurchaseTicketsResult(
                _players.Where(p => p.GetType() == typeof(CpuPlayer)).Count(),
                _players.ToList().AsReadOnly(),
                _tickets.ToList().AsReadOnly());
        }

        public DrawResult Draw()
        {
            DrawTickets(DrawStatus.GrandPrize);
            DrawTickets(DrawStatus.SecondTier);
            DrawTickets(DrawStatus.ThirdTier);

            int houseRevenue = _tickets.Count * _strategy.GetTicketPrice();
            foreach (var ticket in _tickets)
            {
                if (ticket.PrizeAmount != default) houseRevenue -= ticket.PrizeAmount;
            }

            return new DrawResult(
                _players.ToList().AsReadOnly(),
                _tickets.ToList().AsReadOnly(),
                houseRevenue.ToDisplayString());
        }

        private void Purchase<T>(int? userTicketCount = null) where T : Player, new()
        {
            var players = _players.Where(p => p.GetType() == typeof(T));
            foreach (var player in players)
            {
                int ticketCount = _strategy.GetTicketCount<T>(userTicketCount);
                for (int i = 0; i < ticketCount; i++)
                {
                    Execute(() => _tickets.Add(new Ticket { Id = ++_ticketId, PlayerId = player.Id }));
                }
            }
        }

        private void CreatePlayers<T>() where T : Player, new()
        {
            int playerCount = _strategy.GetPlayerCount<T>();
            for (int i = 0; i < playerCount; i++)
            {
                Execute(() => _players.Add(new T { Id = ++_playerId }));
            }
        }

        private void DrawTickets(DrawStatus drawStatus)
        {
            (int ticketCount, int prizeAmount) = _strategy.GetTicketCountAndPrizeAmountForTier(drawStatus, _tickets.Count);

            int count = 0;
            while (count < ticketCount)
            {
                int[] ticketIds = _tickets.Where(t => t.DrawStatus == DrawStatus.NotSet).Select(t => t.Id).ToArray();
                int ran = new Random().Next(0, ticketIds.Length);
                var ticket = _tickets.Single(t => t.Id == ticketIds[ran]);

                Execute(() =>
                {
                    ticket.DrawStatus = drawStatus;
                    ticket.PrizeAmount = prizeAmount;
                });

                count++;
            }
        }

        private void Execute(Action action)
        {
            _gameEngineSemaphore.Wait();

            action();

            _gameEngineSemaphore.Release();
        }
    }
}
