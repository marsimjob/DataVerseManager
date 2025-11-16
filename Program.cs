using DataVerseManager.Models;
using DataVerseManager.Services;
using DataVerseManager.UI;
using Spectre.Console;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //AccountManager.RegisteredUsers = JsonHandeler.LoadJson("registeredUsers.json");
            //Console.OutputEncoding = Encoding.UTF8;
            //AccountManager.LoadLogInMenu();
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            BettingFunctionTest();
      

        }

        static void BettingFunctionTest()
        {
            Team Lakers = new Team();
            Team Bulls = new Team();
            Team Heat = new Team();
            Team Warriors = new Team();
            Team Knicks = new Team();

            Heat.TeamName = "Heat";
            Lakers.TeamName = "Lakers";
            Bulls.TeamName = "Bulls";
            Knicks.TeamName = "Knicks";
            Warriors.TeamName = "Warriors";

            Console.WriteLine(Lakers.WinRate);
            Console.ReadLine();

            Team.BuildTeam(Lakers);
            Lakers.CalculateWinLossRate();
            Console.WriteLine(Lakers.WinRate);
            Console.ReadLine();

            Heat.WinRate = 73;
            Knicks.WinRate = 90;
            Warriors.WinRate = 52;
            Bulls.WinRate = 31;

            // Put this in a wallet and make it track with json etc
            // User --- Wallet as attribute?
            double bettingCash = 10000;

            Coach myCoach = new Coach();
            Coach yourCoach = new Coach();

            myCoach.UserWallet.GetMoney(bettingCash);
            myCoach.CoachName = "Nemo";
            myCoach.CoachTeam.WinRate = 67;
            myCoach.CoachTeam.TeamName = ".NETters";

            yourCoach.UserWallet.GetMoney(bettingCash);
            yourCoach.CoachName = "Bavel";
            yourCoach.CoachTeam.WinRate = 27;
            yourCoach.CoachTeam.TeamName = "Crystals";

            // LOW RANKERS
            Match.RunVisualMatch(Bulls, yourCoach.CoachTeam);
            SpectreGeneric.LoadScreen();
            // IMBALANCED MATCH
            Match.RunVisualMatch(myCoach.CoachTeam, Bulls);
            SpectreGeneric.LoadScreen();
            // MID RANKERS
            Match.RunVisualMatch(Heat, Warriors);
            SpectreGeneric.LoadScreen();
            // HIGH RANKERS
            Match.RunVisualMatch(Lakers, Knicks);
            SpectreGeneric.LoadScreen();
        }
    }
}

