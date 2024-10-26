using Bede.SimplifiedLottery.Domain.Exceptions;
using Bede.SimplifiedLottery.Domain.Interfaces;
using Bede.SimplifiedLottery.Domain.Interfaces.Repositories;
using Bede.SimplifiedLottery.Domain.Settings;
using Bede.SimplifiedLottery.GameEngine;
using Bede.SimplifiedLottery.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bede.SimplifiedLottery.IoC
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IPlayerRepository, PlayerRepository>()
                .AddSingleton<ITicketRepository, TicketRepository>()
                .AddScoped<IGameEngine, GameEngine.GameEngine>()
                .AddScoped<IGameStrategy>(_ => new GameStrategy(configuration.GetCustomSection<GameSettings>()));

            return services;
        }

        private static T GetCustomSection<T>(this IConfiguration configuration)
            => configuration.GetSection(typeof(T).Name).Get<T>() ?? throw new InvalidConfigurationException(typeof(T).Name);
    }
}
