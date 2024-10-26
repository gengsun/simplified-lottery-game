using Bede.SimplifiedLottery.Domain.DTOs;
using Bede.SimplifiedLottery.Domain.Entities;
using Bede.SimplifiedLottery.Domain.Enums;
using Bede.SimplifiedLottery.Domain.Extensions;
using Bede.SimplifiedLottery.Domain.Interfaces;
using Bede.SimplifiedLottery.Domain.Interfaces.Repositories;

namespace Bede.SimplifiedLottery.GameEngine
{
    public class GameEngine : IGameEngine
    {
        private readonly IGameStrategy _strategy;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITicketRepository _ticketRepository;

        public GameEngine(IGameStrategy strategy, IPlayerRepository playerRepository, ITicketRepository ticketRepository)
        {
            _strategy = strategy;
            _playerRepository = playerRepository;
            _ticketRepository = ticketRepository;
        }

        public InitialisationResult Initialise()
        {
            CreatePlayers<UserPlayer>();
            CreatePlayers<CpuPlayer>();

            return new InitialisationResult(
                _playerRepository.GetUserPlayer().Name,
                _strategy.GetStartBalance().ToDisplayString(),
                _strategy.GetTicketPrice().ToDisplayString());
        }

        public PurchaseTicketsResult PurchaseTickets(int numOfUserTickets)
        {
            Purchase<UserPlayer>(numOfUserTickets);
            Purchase<CpuPlayer>();

            return new PurchaseTicketsResult(
                _playerRepository.GetAllByType<CpuPlayer>().Count,
                _playerRepository.GetAll(),
                _ticketRepository.GetAll());
        }

        public DrawResult Draw()
        {
            DrawTickets(DrawStatus.GrandPrize);
            DrawTickets(DrawStatus.SecondTier);
            DrawTickets(DrawStatus.ThirdTier);

            int houseRevenue = _ticketRepository.GetCount() * _strategy.GetTicketPrice();
            var tickets = _ticketRepository.GetWinners();
            foreach (var ticket in tickets)
            {
                if (ticket.PrizeAmount != default) houseRevenue -= ticket.PrizeAmount;
            }

            Console.WriteLine(_playerRepository.GetAll().Count);
            Console.WriteLine(_ticketRepository.GetAll().Count);

            return new DrawResult(
                _playerRepository.GetAll(),
                _ticketRepository.GetAll(),
                houseRevenue.ToDisplayString());
        }

        private void Purchase<T>(int? userTicketCount = null) where T : Player, new()
        {
            var players = _playerRepository.GetAllByType<T>();
            foreach (var player in players)
            {
                int ticketCount = _strategy.GetTicketCount<T>(userTicketCount);
                for (int i = 0; i < ticketCount; i++)
                {
                    _ticketRepository.Add(new Ticket { PlayerId = player.Id });
                }
            }
        }

        private void CreatePlayers<T>() where T : Player, new()
        {
            int playerCount = _strategy.GetPlayerCount<T>();
            for (int i = 0; i < playerCount; i++)
            {
                _playerRepository.Add(new T());
            }
        }

        private void DrawTickets(DrawStatus drawStatus)
        {
            (int ticketCount, int prizeAmount) = _strategy.GetTicketCountAndPrizeAmountForTier(drawStatus, _ticketRepository.GetCount());

            int count = 0;
            while (count < ticketCount)
            {
                int[] ticketIds = _ticketRepository.GetNonWinners().Select(t => t.Id).ToArray();
                int ran = new Random().Next(0, ticketIds.Length);
                _ticketRepository.Update(ticketIds[ran], drawStatus, prizeAmount);

                count++;
            }
        }
    }
}
