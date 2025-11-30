using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using static DataVerseManager.AppSettings;

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
        public static void RunCoachMenu(User currentUser)
        {
            // This will be our coach in this menu
            Coach thisCoach = AccountManager.RegisteredCoaches.FirstOrDefault(user => user.Name == currentUser.Name);

            if (thisCoach == null)
            {
                // Exit back to the top menu if something goes wrong here
                return;
            }

            // Start coach menu loop
            bool inCoach = true;
            while (inCoach)
            {
                // Clear Console
                Console.Clear();

                // Selection
                SpectreGeneric.PresentTopTitle("COACH SELECT SCREEN", AppSettings.MainColor, AppSettings.SubColor);
                string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title($"What's up next, Coach {thisCoach.Name}⭐")
                .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                .PageSize(10)
                .AddChoices(new[] {
                    "PLAY MATCH", "YOUR TEAM","PLAYER WORKOUT",
                    "PLAYER MARKET", "COACH SETTINGS", "RETURN TO TOP MENU"
                }));

                // Fake Loading
                SpectreGeneric.LoadScreen();

                switch (choice)
                {
                    case "PLAY MATCH":
                        // Plays betting automatically, always bets on your team
                        if (thisCoach.CoachTeam.TeamPlayer.Count() >= 5)
                        {
                            (Team A, Team B) = MatchGenerator.GameGenerator();
                            while (B == thisCoach.CoachTeam)
                            {
                                (A, B) = MatchGenerator.GameGenerator();
                            }
                            Betting.PlaceBet(thisCoach.CoachTeam, B, thisCoach, true);
                        }
                        else
                        {
                            SpectreGeneric.PrintMessagePrompt("You need to have a team of 5 players to play!", "red");
                        }
                        break;
                    case "YOUR TEAM":
                        // Status screen for your players
                        LookMembers.ShowMyTeam(thisCoach.CoachTeam);
                        break;
                    case "PLAYER WORKOUT":
                        Gym.RunGym(thisCoach);
                        break;
                    case "PLAYER MARKET":
                        // Buy and sell players, make custom new player
                        PlayerMarket.ShowPlayerMarket(thisCoach);
                        break;
                    case "COACH SETTINGS":
                        // Change coach name, team name, make your own colors etc
                        thisCoach.ShowCoachSettings(thisCoach.GetAccentcolors());
                        break;
                    case "RETURN TO TOP MENU":
                        // Return to top menu
                        inCoach = false;
                        return;

                    default:
                        Console.Error.WriteLine("Invalid choice in coach menu");
                        inCoach = false;
                        break;
                }
            }
        }
        public void ShowCoachSettings(Dictionary<string, Color> accentcolors)
        {
            bool running = true;
            while (running)
            {
                SpectreGeneric.LoadScreen();

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
        public void ChangeCoachName()
        {
            string newName = "";
          
            while (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("What would you like your new Coach Name to be?: ");
                newName = Console.ReadLine();

                if (string.IsNullOrEmpty(newName))
                {
                    SpectreGeneric.PrintMessagePrompt("The new name cannot be empty, please enter a name.", "yellow");
                }            
            }
            
            // If new name already exists in any of the registered jsons, deny request
            if (AccountManager.RegisteredCoaches.Any(coach => coach.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)) 
                || AccountManager.RegisteredUsers.Any(coach => coach.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            {
                SpectreGeneric.PrintMessagePrompt("There is already a coach registered with this name! Try something different!", "red");
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
                JsonHandeler.SaveJson<List<Coach>>(AccountManager.RegisteredCoaches, "registeredCoaches.json");
                SpectreGeneric.PrintMessagePrompt($"Your Team's Name has changed to {newName}", "green");
            }
            else if ((yesOrNo?.Equals("No", StringComparison.OrdinalIgnoreCase) == true))
            {
                SpectreGeneric.PrintMessagePrompt("The old coach name has been keept!", "yellow");
                return;
            }
        }
        public void ChangeTeamName()
        {
            Console.WriteLine("Enter the new Name of your Team: ");
            string newName = Console.ReadLine();

            // If new name already exists in any of the registered jsons, deny request
            if (MatchGenerator.AllTeams.Any(team => team.TeamName.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            {
                SpectreGeneric.PrintMessagePrompt("There is already a team registered by this name! Try something different!", "red");
                return;
            }

            var yesOrNo = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
            .Title($"Do you want to pay 100 dollars to create this characeter")
            .AddChoices(
            "Yes", "No")
            );

            if (yesOrNo == "Yes")
            {
                CoachTeam.TeamName = newName;
                
                JsonHandeler.SaveJson<List<Coach>>(AccountManager.RegisteredCoaches, "registeredCoaches.json");

                if (MatchGenerator.AllTeams.Contains(this.CoachTeam))
                {
                    MatchGenerator.AllTeams.Remove(this.CoachTeam);
                }
                    MatchGenerator.AllTeams.Add(this.CoachTeam);
                
                AnsiConsole.WriteLine($"The new name of your team has been changed to {newName}");
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
            .Title("Choose an arrow-color")
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
                if(AppSettings.TeamThemes.ContainsKey(key))
                {
                    AppSettings.TeamThemes.Remove(key);
                }
                AppSettings.TeamThemes.Add(key, ourNewTheme);
                JsonHandeler.SaveJson(AppSettings.TeamThemes, "teamthemes.json");

                JsonHandeler.SaveJson<UserColors>(new AppSettings.UserColors()
                {
                    StoredMainColor = prime.ToString(),
                    StoredSubColor = secondary.ToString(),
                    StoredAccentColor = accent.ToHex()
                }, "userappsettings.json");

                // Save my team changes to json
                if (AccountManager.RegisteredCoaches.Contains(this))
                {
                    AccountManager.RegisteredCoaches.Remove(this);
                }
                AccountManager.RegisteredCoaches.Add(this);
                JsonHandeler.SaveJson<List<Coach>>(AccountManager.RegisteredCoaches, "registeredCoaches.json");
            }
            else
            {
                SpectreGeneric.PrintMessagePrompt("No changes were made.", "yellow");
            }

            Console.ReadLine();
            Console.Clear();
        }
    }
}
