using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace DataVerseManager.Models
{
    public class Coach : User
    {
        // Attribute
        public string CoachName { get; set; }
        public Team CoachTeam { get; set; }
        
      

        // Constructor
        public Coach() 
        {
            UserWallet = new Wallet();
            CoachTeam = new Team();
            UserWallet.GetMoney(1000);
        }

        // Method
        public void ChangeCoachName()
        {
            Console.WriteLine("What would you like your new Coach Name to be?: ");
            string newName = Console.ReadLine();

            var yesOrNo = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title($"Is the name {newName} okay?")
        .AddChoices(
          "Yes", "No"
        )
        );

            yesOrNo = yesOrNo?.Trim();
            if (yesOrNo?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true)
            {
                CoachName = newName;
                AnsiConsole.WriteLine($"Your name has changed to {newName}");
                Console.ReadLine();
                Console.Clear();
            }
            else if ((yesOrNo?.Equals("No", StringComparison.OrdinalIgnoreCase) == true))
            {
                AnsiConsole.WriteLine("Your old name has been keept!");
                Console.ReadLine();
                Console.Clear();
                return;
            }
        }
        public void ChangeTeamName()
        {
            Console.WriteLine("Enter the new Name of your Team: ");
            string newName = Console.ReadLine();

            var yesOrNo = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
            .Title($"Is the name {newName} okay?")
            .AddChoices(
            "Yes", "No")
            );

            yesOrNo = yesOrNo?.Trim();
            if (yesOrNo?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true)
            {
                CoachTeam.TeamName = newName;
                AnsiConsole.WriteLine($"Your Team's Name has changed to {newName}");
                Console.ReadLine();
                Console.Clear();
            }
            else if ((yesOrNo?.Equals("No", StringComparison.OrdinalIgnoreCase) == true))
            {
                AnsiConsole.WriteLine("Your old Team Name has been keept!");
                Console.ReadLine();
                Console.Clear();
                return;
            }
        }

        // List of Players in the Coach's Team
        public List<Player> playersList { get; set; } = new List<Player>();

        public void ShowTeamPlayers()
        {
            Console.Clear();

            
            
            if (playersList.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Your team has no players![/]");
                Console.ReadLine();
                Console.Clear();
                return;
            }
             
            var playerNames = playersList.Select(p => p.PlayerName).ToList();

            var selectedName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a player from your team:[/]")
                    .PageSize(10)
                    .AddChoices(playerNames)
            );

            var selectedPlayer = playersList.First(p => p.PlayerName == selectedName);

            Console.Clear();

            selectedPlayer.ShowPlayerInformation();

            AnsiConsole.MarkupLine("\n[grey]Press Enter to go back...[/]");
            Console.ReadLine();
            Console.Clear();
        }


        public void CreatePlayer()
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
                name, age, height, country, CoachTeam,
                "PlayerName",
                speed, defending, accuracy, power
            );

            // LÄGG TILL I TEAM-LISTAN
            playersList.Add(newPlayer);

            Console.Clear();

            // 🔥 VISA SPELARENS INFO DIREKT
            AnsiConsole.MarkupLine($"[green]Player '{name}' created![/]\n");
            newPlayer.ShowPlayerInformation();

            AnsiConsole.MarkupLine("\n[grey]Press Enter to return...[/]");
            Console.ReadLine();
        }


    }
}
