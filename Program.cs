using DataVerseManager.Models;
using DataVerseManager.Services;
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
            Console.OutputEncoding = Encoding.UTF8;
            AccountManager.LoadLogInMenu();
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

            Heat.WinRate = 51;
            Knicks.WinRate = 29;
            Warriors.WinRate = 52;
            Bulls.WinRate = 37;

            // Put this in a wallet and make it track with json etc
            // User --- Wallet as attribute?
            double bettingCash = 10000;

            Coach myCoach = new Coach();

            myCoach.UserWallet.GetMoney(bettingCash);
            myCoach.CoachName = "Nemo";
            myCoach.CoachTeam.WinRate = 67;
            myCoach.CoachTeam.TeamName = ".NETters";

            Betting.PlaceBet(Lakers, Bulls, myCoach);
            myCoach.UserWallet.ShowWalletBalance();
            Console.ReadLine();
            Console.Clear();

            Betting.PlaceBet(Heat, Warriors, myCoach);
            myCoach.UserWallet.ShowWalletBalance();
            Console.ReadLine();
            Console.Clear();

            Betting.PlaceBet(Bulls, myCoach.CoachTeam, myCoach);
            myCoach.UserWallet.ShowWalletBalance();
            Console.ReadLine();
            Console.Clear();

            Betting.PlaceBet(Knicks, Heat, myCoach);
            myCoach.UserWallet.ShowWalletBalance();
            Console.ReadLine();
            Console.Clear();

            Betting.PrintMoney(myCoach.UserWallet.ReturnWalletBalance());

        }
    }
}

