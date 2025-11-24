using DataVerseManager.Services;
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
        public Team CoachTeam { get; set; }
        public int OriginalId { get; set; }

        Dictionary<string, string> colordictionary = new Dictionary<string, string>()
        {
            {"Red", "red"},
            {"Blue", "blue"},
            {"Green", "green"},
            {"Yellow", "yellow"},
            {"Purple", "purple"},
            {"Orange", "orange1"},
            {"White", "white"},
            {"Black", "black"},
            {"Grey", "grey"}
        };

        Dictionary<string, Color> Accentcolors = new Dictionary<string, Color>()
        {
            {"Red", Color.Red},
            {"Blue", Color.Blue},
            {"Green", Color.Green},
            {"Yellow", Color.Yellow},
            {"Purple", Color.Purple},
            {"Orange", Color.Orange1},
            {"White", Color.White},
            {"Black", Color.Black},
            {"Grey", Color.Grey}
        };

        // Constructor
        public Coach() 
        {
           
            hasCoachStatus = true;
        }

        public override string ReturnUserInformation()
        {
            string salt = ("c" + Id.ToString());
            string info = $"ID: #{Id} " +
                            $"|| Name: {Name} " +
                            $"|| Team Name: {CoachTeam.TeamName} " +
                            $"|| Wallet Balance: {UserWallet.ReturnWalletBalance()} " +
                            $"|| Decoded Password: {AccountManager.PasswordDecrypt(Password)
                                                      .Substring(0, Password.Length - salt.Length)}";
            return info;
        }

        // Method
        public void ChangeCoachName()
        {
            Console.WriteLine("What would you like your new Coach Name to be?: ");
            string newName = Console.ReadLine();

            // If new name already exists in any of the registered jsons, deny request
            if (AccountManager.RegisteredUsers.Any(user => user.Name == newName) 
                || AccountManager.RegisteredCoaches.Any(user => user.Name == newName))
            {
                SpectreGeneric.PrintMessagePrompt("A User with this Username already exits! Try something different!", "red");
                return;
            }

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
                Name = newName;
                JsonHandeler.SaveJson(AccountManager.RegisteredCoaches, "registeredCoaches.json");
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

            // TODO: DOUBLE CHECK THIS
            //// If new name already exists in any of the registered jsons, deny request
            //if (MatchGenerator.AllTeams.Any(team => team.TeamName.Equals(newName, StringComparison.OrdinalIgnoreCase) == true))
            //{
            //    SpectreGeneric.PrintMessagePrompt("There is already a team registered! Try something different!", "red");
            //    return;
            //}

            var yesOrNo = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
            .Title($"Do you want to pay 100 dollars to create this characeter")
            .AddChoices(
            "Yes", "No")
            );

            if (yesOrNo == "Yes")
            {
                CoachTeam.TeamName = newName;
                
                JsonHandeler.SaveJson(AccountManager.RegisteredCoaches, "registeredCoaches.json");
                
                AnsiConsole.WriteLine($"Your Team's Name has changed to {newName}");
                Console.ReadLine();
                Console.Clear();
            }
            else if (yesOrNo == "No")
            {
                AnsiConsole.WriteLine("Your old Team Name has been keept!");
                Console.ReadLine();
                Console.Clear();
                return;
            }
        }
        public void ShowCoachSettings(Coach coach, Dictionary<string, Color> accentcolors)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                SpectreGeneric.PresentTopTitle("COACH SETTINGS", AppSettings.MainColor, AppSettings.SubColor);
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[{AppSettings.MainColor}]What would you like to do?[/]")
                        .PageSize(5)
                        .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                        .AddChoices(new[]
                        {
             "CHANGE COACH NAME",
             "CHANGE TEAM NAME",
             "CHANGE TEAM COLORS",
             "RETURN TO COACH MENU"
                        })
                );
                switch (choice)
                {
                    case "CHANGE COACH NAME":
                        ChangeCoachName();
                        break;
                    case "CHANGE TEAM NAME":
                        ChangeTeamName();
                        break;
                    case "CHANGE TEAM COLORS":
                        ChangeTeamColors(accentcolors);
                        break;
                    case "RETURN TO COACH MENU":
                        running = false;
                        break;
                }
            }
        }
        public Dictionary<string, Color> GetAccentcolors()
        {
            return Accentcolors;
        }

        public void ChangeTeamColors(Dictionary<string, Color> accentcolors)
        {
            Console.Clear();

            AnsiConsole.Write(
                new Panel("[bold]Choose a new color theme for your team[/]")
                    .BorderColor(Color.Grey)
                    .Header("TEAM COLOR SETTINGS")
                    .HeaderAlignment(Justify.Center)
            );

            // Let coach pick an NBA team theme from AppSettings
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a new primary color")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                    .AddChoices(colordictionary.Keys)
            );


            string secondaryChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a new secondary color")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                    .AddChoices(colordictionary.Keys)
            );

            string accentChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose a new accent color")
            .PageSize(10)
            .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
            .AddChoices(Accentcolors.Keys)
            );

            // Confirm
            var yesOrNo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[white]Apply theme personal[/]?")
                    .AddChoices("Yes", "No")
            );

            if (yesOrNo == "Yes")
            {
                // Apply colors to the coach's team
                CoachTeam.PrimaryColor = colordictionary[choice];
                CoachTeam.SecondaryColor = colordictionary[secondaryChoice];
                CoachTeam.AccentColor = accentcolors[accentChoice];

                // Also apply to AppSettings for consistency
                AppSettings.MainColor = colordictionary[choice];
                AppSettings.SubColor = colordictionary[secondaryChoice];
                AppSettings.AccentColor = accentcolors[accentChoice];

                AnsiConsole.MarkupLine(
                    $"[green]Team colors updated to personal theme![/]"
                );

                Color prime = Accentcolors[choice];
                Color secondary = Accentcolors[secondaryChoice];
                Color accent = Accentcolors[accentChoice];
                TeamTheme ourNewTheme = new TeamTheme (prime, secondary, accent);
                string key = CoachTeam.TeamName;
                // Do we want to save this as a theme to our Team name to Dictionary TeamThemes in AppSettings
                AppSettings.TeamThemes.Add(key, ourNewTheme);
                // Save to json
                JsonHandeler.SaveJson(AppSettings.TeamThemes, "teamthemes.json");
                // Save my team changes to json
                JsonHandeler.SaveJson(AccountManager.RegisteredCoaches, "registeredCoaches.json");
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No changes were made.[/]");
            }

            Console.ReadLine();
            Console.Clear();
        }
    }
}
