using Bede.SimplifiedLottery.Domain.Enums;
using Bede.SimplifiedLottery.Domain.Interfaces.GameEngine;
using Bede.SimplifiedLottery.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddDependencies(configuration);

using IHost host = builder.Build();

Play(host.Services);

await host.RunAsync();

static void Play(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IGameEngine gameEngine = serviceScope.ServiceProvider.GetRequiredService<IGameEngine>();

    while (true)
    {
        Initialise(gameEngine);
        PurchaseTickets(gameEngine);
        Draw(gameEngine);

        Console.WriteLine($"\nPress Enter to play again or any other key to exit...\n");
        var keyInfo = Console.ReadKey();
        if (keyInfo.Key != ConsoleKey.Enter) break;

        gameEngine.Reset();
    }
}

static void Initialise(IGameEngine gameEngine)
{
    var initResult = gameEngine.Initialise();

    Console.WriteLine($"Welcome to the Bede Lottery, {initResult.UserPlayerName}!");
    Console.WriteLine($"\n* Your digital balance: {initResult.StartBalance}");
    Console.WriteLine($"* Ticket Price: {initResult.TicketPrice} each");
    Console.WriteLine($"\nHow many tickets do you want to buy, {initResult.UserPlayerName}?");
}

static void PurchaseTickets(IGameEngine gameEngine)
{
    if (!int.TryParse(Console.ReadLine(), out var input)) input = 10;
    var purchaseResult = gameEngine.PurchaseTickets(input);

    Console.WriteLine($"\n{purchaseResult.NumberOfCpuPlayers} CPU players have also purchased tickets.\n");

    foreach (var player in purchaseResult.Players)
    {
        string ticketIds = string.Join(", ", player.Tickets.Select(p => p.Id));
        Console.WriteLine($"{player.Name} purchased {player.Tickets.Count} ticket(s): {ticketIds}");
    }
}

static void Draw(IGameEngine gameEngine)
{
    Console.WriteLine("\nPress any key to start the draw...");
    Console.ReadKey();

    var drawResult = gameEngine.Draw();
    var grandPrizeTicket = drawResult.Tickets.Single(t => t.DrawStatus == DrawStatus.GrandPrize);
    var secondTierTickets = drawResult.Tickets.Where(t => t.DrawStatus == DrawStatus.SecondTier);
    var thirdTierTickets = drawResult.Tickets.Where(t => t.DrawStatus == DrawStatus.ThirdTier);
    var secondTierTicketIds = string.Join(", ", secondTierTickets.Select(r => r.Id));
    var thirdTierTicketIds = string.Join(", ", thirdTierTickets.Select(r => r.Id));

    Console.WriteLine($"\nTicket Draw Results:");
    Console.WriteLine($"\n* Grand Prize: Ticket {grandPrizeTicket.Id} wins {drawResult.GrandPrize}!");
    Console.WriteLine($"* Second Tier: Tickets {secondTierTicketIds} win {drawResult.SecondTierPrize} each!");
    Console.WriteLine($"* Third Tier: Tickets {thirdTierTicketIds} win {drawResult.ThirdTierPrize} each!\n");

    foreach (var player in drawResult.Players)
    {
        string ticketIds = string.Join(", ", player.Tickets.Select(p => p.Id));
        Console.WriteLine($"{player.Name} wins {player.Winning} with ticket(s): {ticketIds}");
    }

    Console.WriteLine("\nCongratulations to the winners!");
    Console.WriteLine($"\nHouse Revenue: {drawResult.HouseRevenue}");
}