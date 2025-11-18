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
            
            // For randomization over the match objects life time
            Random rng = new Random();
            List<Team> AllTeams = MatchGenerator.AllTeams;

            // Put this in a wallet and make it track with json etc
            // User --- Wallet as attribute?
            double bettingCash = 10000;
            Coach myCoach = new Coach();

            myCoach.CoachTeam = AllTeams.FirstOrDefault(t => t.TeamName == "Crystal");
            Leaderboard ourLeaderBoard = new Leaderboard();

            TitleScreen.ShowSplashScreen();
            SpectreGeneric.LoadScreen();
           
            bool isRunning = true;
            while (isRunning)
            {
                AccountManager.LoadLogInMenu(); // From here get the logged in user's name
                
                // Set user theme for now
                AppSettings.LoadUserTheme();
                User currentUser = new User(); // and put the name of the current user here
                currentUser.Name = ".Net25";

                // Top Meny
                bool onTopMenu = true;
                while (onTopMenu)
                {
                    // Fake Loading
                    SpectreGeneric.LoadScreen();

                    // Clear Console
                    Console.Clear();

                    // Selection
                    SpectreGeneric.PresentTopTitle("SELECT SCREEN", AppSettings.MainColor, AppSettings.SubColor);
                    string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title($"Make your selection, {currentUser.Name}")
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
                            Betting.PlaceBet(A, B, myCoach);
                            break;
                        case "LEADERBOARD":
                            ourLeaderBoard.Run();
                            break;
                        case "MATCHBOARD":
                            Matchboard.ShowMenu();
                            break;
                        case "RULE BOOK":
                            RuleBook.RunRuleBook();
                            break;
                        case "COACH MENU":
                            Console.WriteLine("Show coach menu");
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
        static void LiveMatch()
        {
            (Team A, Team B) = MatchGenerator.GameGenerator();
            MatchSimulator.RunVisualMatch(A, B);
            Console.ReadLine();
        }


    
    }
}
            

