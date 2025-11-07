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
            Team team1 = new Team();
            team1.TeamName = "Chicago Bulls";
            team1.ImageFile = "images/Chicago_Bulls_logo.svg.png";
           
            Team team2 = new Team();
            team1.TeamName = "Chicago Lakers";
            team2.ImageFile = "images/Los_Angeles_Lakers_logo.svg.png";

            var player2 = new Player(
            name: "LeBron James",
            age: 39,
            height: 206, // cm
            country: "USA",
            team: team2,
            imageFile: "Images/lebron.jpg",
            speed: 88,
            defending: 80,
            accuracy: 90,
            power: 92,
            info: "Widely regarded as one of the greatest basketball players of all time. Known for his athleticism, leadership, and basketball IQ."
            );
            var player1 = new Player();

            player1.ImageFile = "images/31475477.jpg";
            player1.PlayerName = "Nemanja 'Nemo' Johnson";
            player1.PlayerInfo = "A tough as nail teacher that goes by the life motto of \"Never stop learning\"" +
                "He has a heart of gold, loves conspiration theories and doesn't like woke stuff. Incidentally he is good" +
                "at basketball.";
            player1.PlayerTeam = team1;
            player1.PlayerHeight = 189;
            player1.PlayerAge = 31;
            player1.Speed = 45;
            player1.Power = 100;
            player1.Defending = 39;
            player1.Accuracy = 67;
            player1.TotalStat = (player1.Speed + player1.Power + player1.Defending + player1.Accuracy) / 4.0;

            player1.ShowPlayerInformation();
        }
    }
}

