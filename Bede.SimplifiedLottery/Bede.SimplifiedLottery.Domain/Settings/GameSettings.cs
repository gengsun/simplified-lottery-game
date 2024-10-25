using Bede.SimplifiedLottery.Domain.Exceptions;

namespace Bede.SimplifiedLottery.Domain.Settings
{
    public sealed record class GameSettings
    {
        private int _ticketPrice;
        public int TicketPrice
        {
            get => _ticketPrice;
            init => _ticketPrice = value == default
                ? throw new InvalidConfigurationException(nameof(TicketPrice))
                : value;
        }

        private int _startBalance;
        public int StartBalance
        {
            get => _startBalance;
            init => _startBalance = value < TicketPrice
                ? throw new InvalidConfigurationException(nameof(StartBalance))
                : value;
        }

        private int _minimumNumberOfPlayers;
        public int MinimumNumberOfPlayers
        {
            get => _minimumNumberOfPlayers;
            init => _minimumNumberOfPlayers = value == default
                ? throw new InvalidConfigurationException(nameof(MinimumNumberOfPlayers))
                : value;
        }

        private int _maximumNumberOfPlayers;
        public int MaximumNumberOfPlayers
        {
            get => _maximumNumberOfPlayers;
            init => _maximumNumberOfPlayers = value < MinimumNumberOfPlayers
                ? throw new InvalidConfigurationException(nameof(MaximumNumberOfPlayers))
                : value;
        }

        private int _minimumAllowedTickets;
        public int MinimumAllowedTickets
        {
            get => _minimumAllowedTickets;
            init => _minimumAllowedTickets = value == default
                ? throw new InvalidConfigurationException(nameof(MinimumAllowedTickets))
                : value;
        }

        private int _maximumAllowedTickets;
        public int MaximumAllowedTickets
        {
            get => _maximumAllowedTickets;
            init => _maximumAllowedTickets = value < MinimumAllowedTickets
                ? throw new InvalidConfigurationException(nameof(MaximumAllowedTickets))
                : value;
        }

        private int _grandPriceRevenuePercentage;
        public int GrandPriceRevenuePercentage
        {
            get => _grandPriceRevenuePercentage;
            init => _grandPriceRevenuePercentage = value < SecondTierRevenuePercentage
                ? throw new InvalidConfigurationException(nameof(GrandPriceRevenuePercentage))
                : value;
        }

        private int _secondTierRevenuePercentage;
        public int SecondTierRevenuePercentage
        {
            get => _secondTierRevenuePercentage;
            init => _secondTierRevenuePercentage = value < ThirdTierRevenuePercentage
                ? throw new InvalidConfigurationException(nameof(SecondTierRevenuePercentage))
                : value;
        }

        private int _thirdTierRevenuePercentage;
        public int ThirdTierRevenuePercentage
        {
            get => _thirdTierRevenuePercentage;
            init => _thirdTierRevenuePercentage = value == default
                ? throw new InvalidConfigurationException(nameof(ThirdTierRevenuePercentage))
                : value;
        }

        private int _secondTierTicketPercentage;
        public int SecondTierTicketPercentage
        {
            get => _secondTierTicketPercentage;
            init => _secondTierTicketPercentage = value == default
                ? throw new InvalidConfigurationException(nameof(SecondTierTicketPercentage))
                : value;
        }

        private int _thirdTierTicketPercentage;
        public int ThirdTierTicketPercentage
        {
            get => _thirdTierTicketPercentage;
            init => _thirdTierTicketPercentage = value < ThirdTierTicketPercentage
                ? throw new InvalidConfigurationException(nameof(ThirdTierTicketPercentage))
                : value;
        }
    }
}
