using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
          
            Team lakers = new Team
            {
                TeamName = "Los Angeles Lakers",
                ImageFile = "lakers.png"
            };  
            lakers.WinRate = 52;   


            Team Bulls = new Team
            {
                TeamName = "Chicago Bulls",
                ImageFile = "bulls.png"
            };  


            Bulls.WinRate = 48; 


            Betting betting = new Betting();

            var (lakersOdds, bullsOdds) = Betting.GetOdds(lakers , Bulls);

            betting.ShowBettingTable(lakers, Bulls);

            double mymoney = 10000;
          

            mymoney += betting.PlaceBet(lakers, Bulls);

            Console.WriteLine($"Remaining money after bet: {mymoney}");


          






        }
    }
}

