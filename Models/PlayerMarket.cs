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
        public static List<Player> MarketPlayers = JsonHandeler.LoadJson<List<Player>>("marketlist.json") ?? new List<Player>()
        {
        new Player("Jordan “Hawk” Miller", 26, 1.98, "USA", "images/default.png",
        85, 70, 88, 75, "Fast two-way guard", null),

        new Player("Leo “Anchor” Silva", 29, 2.08, "Brazil", "images/default.png",
        65, 92, 60, 90, "Strong defensive center", null),

        new Player("Victor “Blaze” Carter", 24, 1.92, "USA", "images/default.png",
        90, 60, 82, 70, "Explosive scorer, high energy", null),

        new Player("Evan “Ice” Thompson", 30, 1.97, "Canada", "images/default.png",
        72, 75, 93, 65, "Cold-blooded shooter", null),

        new Player("Rafael “Engine” Cruz", 27, 1.90, "Spain", "images/default.png",
        88, 68, 78, 72, "Elite floor general", null),

        new Player("Damon “Rhino” Brooks", 31, 2.05, "USA", "images/default.png",
        60, 88, 64, 95, "Power forward with strength", null),

        new Player("Kai “Shadow” Tanaka", 25, 1.88, "Japan", "images/default.png",
        92, 58, 84, 62, "Speedy slasher", null),

        new Player("Mason “Tower” Grant", 28, 2.12, "USA", "images/default.png",
        55, 95, 58, 97, "Dominant rim protector", null),

        new Player("Nikolai “Sniper” Markov", 23, 1.96, "Russia", "images/default.png",
        75, 62, 95, 68, "Elite 3-point shooter", null),

        new Player("Tariq “Wizard” Hassan", 26, 1.93, "Egypt", "images/default.png",
        82, 70, 80, 70, "Creative passer and playmaker", null)
        }; 
  
    
        // Methods 

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
            // Introduce sellings
            AnsiConsole.MarkupLine("[bold yellow]Sell Players to Market[/]\n");
           
            // If player count in the coach team is 0, then boot us out 
            if (myCoach.CoachTeam.TeamPlayer.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]You have no players to sell.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
                Console.ReadLine();
                return;
            }

            // Make a string list to put into the Specter.Console prompt AddChoices
            List<string> playerNames = myCoach.CoachTeam.TeamPlayer.Select(player => player.PlayerName).ToList();
            playerNames.Add("Return to Player Market Menu"); 
          
            var selectedPlayerName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a player to sell:")
                    .PageSize(10)
                    .AddChoices(playerNames) // Here is the list with player names, instead of seperate strings input manually by us
            );
            
            // If select return to player market, get booted out
            if (selectedPlayerName == "Return to Player Market Menu")
            {
                return;
            }

            Player selectedPlayer = myCoach.CoachTeam.TeamPlayer.FirstOrDefault(player => player.PlayerName.Equals(selectedPlayerName));
            // Assume each player sells for 150 dollars
            const double playerSellPrice = 150;
            myCoach.UserWallet.GetMoney(playerSellPrice);
            myCoach.CoachTeam.TeamPlayer.Remove(selectedPlayer);
            // Save changes to json
            if (MatchGenerator.AllTeams.Any(team => team.TeamName == myCoach.CoachTeam.TeamName))
                JsonHandeler.SaveJson<List<Team>>(MatchGenerator.AllTeams, "allteams.json");
           
            MarketPlayers.Add(selectedPlayer);
            // Save changes to marketlist to json
            JsonHandeler.SaveJson<List<Player>>(MarketPlayers, "marketlist.json");

            AnsiConsole.MarkupLine($"[green]You have successfully sold {selectedPlayer.PlayerName} for ${playerSellPrice}![/]");
            AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
            Console.ReadLine(); return;
        }

        public static void BuyPlayer(Coach myCoach)
        {
            Console.Clear();

            // Introduce Buy
            AnsiConsole.MarkupLine("[bold yellow]Buy Players from Market[/]\n");

            // If there are no players in the market there wont be anything shown and we go back
            if (MarketPlayers.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No players available in the market.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
                Console.ReadLine();
                return;
            }

            // Creates a list with all the players from MarketPlayers
            List<string> playerNames = MarketPlayers.Select(player => player.PlayerName).ToList();
            playerNames.Add("Return to Player Market Menu"); // Extra menu choice to return to Player Market
            
            // Specter.Console Select Prompt
            string selectedPlayerName = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Select a player to buy:")
                .PageSize(10)
                .AddChoices(playerNames)
            );

            // Here we return to player market on choice
            if (selectedPlayerName == "Return to Player Market Menu")
            {
                return;
            }

            // Set the player to the choice that matches the PlayerName
            Player selectedPlayer = MarketPlayers.First(player => player.PlayerName == selectedPlayerName);
            
            // Assume each player costs 200 dollars
            const double playerCost = 200;
            
            // Checks if your wallet is less than playerCost
            if (myCoach.UserWallet.ReturnWalletBalance() < playerCost)
            {
                AnsiConsole.MarkupLine("[red]Insufficient funds to buy this player.[/]");
            }
            else // else if you have money
            {
                myCoach.UserWallet.UseMoney(playerCost);
                
                // Adds the player to our team
                myCoach.CoachTeam.TeamPlayer.Add(selectedPlayer);
                // Save changes to json
                if (MatchGenerator.AllTeams.Any(team => team.TeamName == myCoach.CoachTeam.TeamName))
                    JsonHandeler.SaveJson<List<Team>>(MatchGenerator.AllTeams, "allteams.json");

                // Removes the same player from the MarketPlace so there won't be doubles
                MarketPlayers.Remove(selectedPlayer);
                // Save changes in MarketList to json
                JsonHandeler.SaveJson<List<Player>>(MarketPlayers, "marketlist.json");

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

            Player newPlayer = new Player
            (
            name,
            age,
            height,
            country,
            "images/default.png",    
            speed,
            defending,
            accuracy,
            power,
            "",                      
            myCoach.CoachTeam      
            );

            // I want to ask if the user wants to pay a player
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

            // Put into my list
            myCoach.CoachTeam.TeamPlayer.Add(newPlayer);
            newPlayer.PlayerTeam = myCoach.CoachTeam;

            // Save changes to json
            if(MatchGenerator.AllTeams.Any(team => team.TeamName == myCoach.CoachTeam.TeamName))
            JsonHandeler.SaveJson<List<Team>>(MatchGenerator.AllTeams, "allteams.json");

            Console.Clear();

            // Show player info after creation
            AnsiConsole.MarkupLine($"[green]Player '{name}' created![/]\n");
            newPlayer.ShowPlayerInformation();

            AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
            Console.ReadLine();
        }
    }
}
