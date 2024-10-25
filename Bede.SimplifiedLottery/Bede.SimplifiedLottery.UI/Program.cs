using Bede.SimplifiedLottery.Domain.Enums;
using Bede.SimplifiedLottery.Domain.Extensions;
using Bede.SimplifiedLottery.Domain.Interfaces;
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

    Initialise(gameEngine);
    PurchaseTickets(gameEngine);
    Draw(gameEngine);

    Console.ReadLine();
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

    Console.WriteLine($"\n{purchaseResult.NumberOfCpuPlayers} CPU players also have purchased tickets.\n");

    foreach (var player in purchaseResult.Players)
    {
        var playerTickets = purchaseResult.Tickets.Where(t => t.PlayerId == player.Id);
        Console.WriteLine($"{player.Name} purchased {playerTickets.Count()} ticket(s): {string.Join(", ", playerTickets.Select(p => p.Id))}");
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

    Console.WriteLine($"\nTicket Draw Results:");
    Console.WriteLine($"\n* Grand Prize: Ticket {grandPrizeTicket.Id} wins {grandPrizeTicket.PrizeAmount.ToDisplayString()}!");
    Console.WriteLine($"* Second Tier: Tickets {string.Join(", ", secondTierTickets.Select(r => r.Id))} win {secondTierTickets.First().PrizeAmount.ToDisplayString()} each!");
    Console.WriteLine($"* Third Tier: Tickets {string.Join(", ", thirdTierTickets.Select(r => r.Id))} win {thirdTierTickets.First().PrizeAmount.ToDisplayString()} each!\n");

    var ticketGroups = drawResult.Tickets.Where(t => t.DrawStatus != DrawStatus.NotSet).GroupBy(t => t.PlayerId);
    foreach (var ticketGroup in ticketGroups)
    {
        var player = drawResult.Players.Single(p => p.Id == ticketGroup.Key);
        int prizeAmount = 0;
        foreach (var ticket in ticketGroup)
        {
            prizeAmount += ticket.PrizeAmount;
        }

        Console.WriteLine($"{player.Name} wins {prizeAmount.ToDisplayString()} with ticket(s): {string.Join(", ", ticketGroup.Select(r => r.Id))}");
    }

    Console.WriteLine("\nCongratulations to the winners!");
    Console.WriteLine($"\nHouse Revenue: {drawResult.HouseRevenue}");
}