using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    public static class PlayerMarket
    {
        // Attributes
        public static List<Player> MarketPlayers = new List<Player>()
{
    new Player("LeBron James", 39, 206, "USA", new Team(), 90, 85, 88, 92),
    new Player("Stephen Curry", 36, 188, "USA", new Team(), 88, 75, 96, 80),
    new Player("Giannis Antetokounmpo", 30, 211, "Greece", new Team(), 94, 90, 82, 95),
    new Player("Nikola Jokić", 29, 211, "Serbia", new Team(), 80, 82, 86, 88),
    new Player("Luka Dončić", 25, 201, "Slovenia", new Team(), 84, 76, 90, 85),
    new Player("Kevin Durant", 36, 208, "USA", new Team(), 86, 78, 92, 88)
}; // List of Players available in the Market



    // meothods 

    public static void ShowPlayerMarket(Coach coach)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                SpectreGeneric.PresentTopTitle("PLAYER MARKET", AppSettings.MainColor, AppSettings.SubColor);
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[{AppSettings.MainColor}]What would you like to do?[/]")
                        .PageSize(5)
                        .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                        .AddChoices(new[]
                        {
      "BUY PLAYERS",
      "SELL PLAYERS",
      "CREATE PLAYER",
      "RETURN TO COACH MENU"
                        })
                );
                switch (choice)
                {
                    case "BUY PLAYERS":
                        // Buy players
                        BuyPlayer(coach);
                        break;
                    case "SELL PLAYERS":
                        // Sell players
                        SellPlayer(coach);
                        break;
                    case "CREATE PLAYER":
                        // Create player
                        CreatePlayer(coach);
                        break;
                    case "RETURN TO COACH MENU":
                        running = false;
                        break;
                }
            }
        }

        public static void SellPlayer(Coach myCoach)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Sell Players to Market[/]\n");
            if (myCoach.playersList.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]You have no players to sell.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
                Console.ReadLine();
                return;
            }
            var playerNames = myCoach.playersList.Select(p => p.PlayerName).ToList();
            playerNames.Add("Return to Player Market Menu");
            var selectedPlayerName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a player to sell:")
                    .PageSize(10)
                    .AddChoices(playerNames)
            );
            if (selectedPlayerName == "Return to Player Market Menu")
            {
                return;
            }
            var selectedPlayer = myCoach.playersList.First(p => p.PlayerName == selectedPlayerName);
            // Assume each player sells for 150 dollars
            const double playerSellPrice = 150;
            myCoach.UserWallet.GetMoney(playerSellPrice);
            myCoach.playersList.Remove(selectedPlayer);
            MarketPlayers.Add(selectedPlayer);
            AnsiConsole.MarkupLine($"[green]You have successfully sold {selectedPlayer.PlayerName} for ${playerSellPrice}![/]");
            AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
            Console.ReadLine(); return;
        }

        public static void BuyPlayer(Coach myCoach)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Buy Players from Market[/]\n");
            if (MarketPlayers.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No players available in the market.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
                Console.ReadLine();
                return;
            }
            var playerNames = MarketPlayers.Select(p => p.PlayerName).ToList();
            playerNames.Add("Return to Player Market Menu");
            var selectedPlayerName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a player to buy:")
                    .PageSize(10)
                    .AddChoices(playerNames)
            );
            if (selectedPlayerName == "Return to Player Market Menu")
            {
                return;
            }
            var selectedPlayer = MarketPlayers.First(p => p.PlayerName == selectedPlayerName);
            // Assume each player costs 200 dollars
            const double playerCost = 200;
            if (myCoach.UserWallet.ReturnWalletBalance() < playerCost)
            {
                AnsiConsole.MarkupLine("[red]Insufficient funds to buy this player.[/]");
            }
            else
            {
                myCoach.UserWallet.UseMoney(playerCost);
                myCoach.playersList.Add(selectedPlayer);
                MarketPlayers.Remove(selectedPlayer);
                AnsiConsole.MarkupLine($"[green]You have successfully bought {selectedPlayer.PlayerName} for ${playerCost}![/]");
            }
            AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
            Console.ReadLine();
        }
        public static void CreatePlayer(Coach myCoach)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Create a New Player[/]\n");

            string name = AnsiConsole.Ask<string>("Enter [green]player name[/]: ");
            int age = AnsiConsole.Ask<int>("Enter [green]age[/]: ");
            double height = AnsiConsole.Ask<double>("Enter [green]height (cm)[/]: ");
            string country = AnsiConsole.Ask<string>("Enter [green]country[/]: ");

            double speed = AnsiConsole.Ask<double>("Enter [blue]Speed[/]: ");
            double defending = AnsiConsole.Ask<double>("Enter [blue]Defending[/]: ");
            double accuracy = AnsiConsole.Ask<double>("Enter [blue]Accuracy[/]: ");
            double power = AnsiConsole.Ask<double>("Enter [blue]Power[/]: ");



            Player newPlayer = new Player(
                name, age, height, country, myCoach.CoachTeam,
                "images/default.png",
                speed, defending, accuracy, power
            );
            // i want to ask if the user wants to pay a player
            var yesOrNo = AnsiConsole.Prompt(
new SelectionPrompt<string>()
.Title($"Do you want to pay 100 dollars to create this player")
.AddChoices(
"Yes", "No")
);

            if (yesOrNo == "Yes")
            {
                myCoach.UserWallet.UseMoney(100);
            }
            else if (yesOrNo == "No")
            {
                Console.WriteLine("Player creation cancelled. Returning to menu...");
                Console.ReadLine();

                return;
            }
            // LÄGG TILL I TEAM-LISTAN
            myCoach.playersList.Add(newPlayer);

            Console.Clear();

            // 🔥 VISA SPELARENS INFO DIREKT
            AnsiConsole.MarkupLine($"[green]Player '{name}' created![/]\n");
            newPlayer.ShowPlayerInformation();

            AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
            Console.ReadLine();
        }
    }
}
