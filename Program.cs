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
            
            
            // Put this in a wallet and make it track with json etc
            // User --- Wallet as attribute?
       
            Player newP = new Player();

         
            Team newT = new Team();
            newT.TeamName = "Apes";
            newT.ImageFile = "images/Nba2k26.png";
            newP.PlayerTeam = newT;
            newP.ShowPlayerInformation();

            Leaderboard ourLeaderBoard = new Leaderboard();

            TitleScreen.ShowSplashScreen();
           
            SpectreGeneric.LoadScreen();
           
            bool isRunning = true;
            while (isRunning)
            {
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
                            ourLeaderBoard.Run();
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
            // Clear Console
            Console.Clear();

            // This will be our coach in this menu
            Coach thisCoach = AccountManager.RegisteredCoaches.FirstOrDefault(user => user.Name == currentUser.Name);
            if(thisCoach == null)
            {
                // Exit back to the top menu if something goes wrong here
                return;
            }
            
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
                    (Team A, Team B) = MatchGenerator.GameGenerator();
                    while(B == thisCoach.CoachTeam)
                    {
                        (A, B) = MatchGenerator.GameGenerator();
                    }
                    Betting.PlaceBet(thisCoach.CoachTeam, B, thisCoach);
                    break;
                case "YOUR TEAM":
                    // Status screen for your players
                    break;
                case "PLAYER WORKOUT":
                    Gym.RunGym(thisCoach);
                    break;
                case "PLAYER MARKET":
                    // Buy and sell players, make custom new player
                    break;
                case "COACH SETTINGS":
                    // Change coach name, team name, make your own colors etc
                    thisCoach.ShowCoachSettings(thisCoach, thisCoach.GetAccentcolors());
                    break;
                case "RETURN TO TOP MENU":
                    // Return to top menu
                    return;
                
                default:
                    Console.Error.WriteLine("Invalid choice in coach menu");
                    break;
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
            

