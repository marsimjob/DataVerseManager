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
                    SpectreGeneric.LoadScreen();

                    switch (choice)
                    {
                        case "FREE MATCH":
                            // Make it so that the user can select matches that are going on today
                            MatchGenerator.LiveMatch();
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
                                Coach.RunCoachMenu(currentUser);
                            }
                            else
                            {
                                currentUser.UpgradeToCoach();
                                if (currentUser.hasCoachStatus)
                                {
                                    Coach.RunCoachMenu(currentUser);
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

        static void SetUp()
        {
            foreach (Team team in MatchGenerator.AllTeams)
            {
                Team.BuildTeam(team);
            }
            JsonHandeler.SaveJson<List<Team>>(MatchGenerator.AllTeams, "allteams.json");

            MatchGenerator.AllTeams[1].TeamPlayer[2].ShowPlayerInformation();
            MatchGenerator.AllTeams[2].TeamPlayer[4].ShowPlayerInformation();
            MatchGenerator.AllTeams[3].TeamPlayer[3].ShowPlayerInformation();
            MatchGenerator.AllTeams[4].TeamPlayer[3].ShowPlayerInformation();
            MatchGenerator.AllTeams[5].TeamPlayer[1].ShowPlayerInformation();
            MatchGenerator.AllTeams[7].TeamPlayer[2].ShowPlayerInformation();
            MatchGenerator.AllTeams[9].TeamPlayer[4].ShowPlayerInformation();
        }
    
    }
}
            

