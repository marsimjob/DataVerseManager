using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DataVerseManager
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            foreach (Team team in MatchGenerator.AllTeams)
            {
                Team.BuildTeam(team);
            }
            JsonHandeler.SaveJson<List<Team>>(MatchGenerator.AllTeams, "allteams.json");

            Launch();
        }

        private static void Launch()
        {
            TitleScreen.ShowSplashScreen();
            SpectreGeneric.LoadScreen();

            bool isRunning = true;
            while (isRunning)
            {
                // Clear Console
                Console.Clear();

                // Set up a user and loop log-in until a valid user is found

                User currentUser = null;

                while (currentUser == null)
                {
                    currentUser = AccountManager.LoadLogInMenu(); // From here get the logged in user's name
                }

                // Set user theme for now
                AppSettings.LoadUserTheme();

                // Top Meny
                bool onTopMenu = true;
                while (onTopMenu)
                {
                    // Clear Console
                    Console.Clear();
                    // Fake Loading
                    SpectreGeneric.LoadScreen();
                    string userName = "";

                    if (currentUser.hasCoachStatus)
                    {
                        userName = "Coach " + currentUser.Name + "⭐";
                    }
                    else
                    {
                        userName = currentUser.Name;
                    }

                    // Clear Console
                    Console.Clear();

                    // Selection
                    SpectreGeneric.PresentTopTitle("SELECT SCREEN", AppSettings.MainColor, AppSettings.SubColor);
                    string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title($"Make your selection, {userName}")
                    .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                    .PageSize(10)
                    .AddChoices(new[] {
                    "FREE MATCH","BETTING MATCH", "LEADERBOARD",
                    "MATCHBOARD", "RULE BOOK", "COACH MENU",
                    "SETTINGS", "LOG OUT"
                    }));

                    // Fake Loading
                    //SpectreGeneric.LoadScreen();

                    switch (choice)
                    {
                        case "FREE MATCH":
                            // Make it so that the user can select matches that are going on today
                            LiveMatch();
                            break;
                        case "BETTING MATCH":
                            // Make it so that the user can select matches that are going on today
                            (Team A, Team B) = MatchGenerator.GameGenerator();
                            Betting.PlaceBet(A, B, currentUser);
                            break;
                        case "LEADERBOARD":
                            Leaderboard.DisplayLeaderBoard();
                            break;
                        case "MATCHBOARD":
                            Matchboard.ShowMenu();
                            break;
                        case "RULE BOOK":
                            RuleBook.RunRuleBook();
                            break;
                        case "COACH MENU":
                            if (currentUser.hasCoachStatus)
                            {
                                RunCoachMenu(currentUser);
                            }
                            else
                            {
                                currentUser.UpgradeToCoach();
                                if (currentUser.hasCoachStatus)
                                {
                                    RunCoachMenu(currentUser);
                                }
                            }
                            break;
                        case "SETTINGS":
                            AppSettings.RunSettings();
                            break;
                        case "LOG OUT":
                            Console.Clear();
                            onTopMenu = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void RunCoachMenu(User currentUser)
        {
            // This will be our coach in this menu
            Coach thisCoach = AccountManager.RegisteredCoaches.FirstOrDefault(user => user.Name == currentUser.Name);

            if(thisCoach == null)
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
            //SpectreGeneric.LoadScreen();

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

        static void LiveMatch()
        {
            (Team A, Team B) = MatchGenerator.GameGenerator();
            MatchSimulator.RunVisualMatch(A, B);
            Console.ReadLine();
        }


    
    }
}
            

