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
           Gym gym = new Gym();
            var Lakers = new Team();
            Lakers.TeamName = "Los Angeles Lakers";
            Lakers.ImageFile = "images/Los_Angeles_Lakers_logo.svg.png";
            BuildTeam(Lakers);
            Console.ReadLine();
            Console.Clear();

            Lakers.TeamPlayer[0].ShowPlayerInformation();
            Console.ReadLine();
            Console.Clear();
            gym.PickAndTrainPlayer(Lakers.TeamPlayer);

            Lakers.TeamPlayer[0].ShowPlayerInformation();
            Console.ReadLine();
            Console.Clear();
        }

        public static void BuildTeam(Team team)
        {
            var player3 = new Player(
    name: "Anthony Davis",
    age: 31,
    height: 208, // cm
    country: "USA",
    team: team,
    imageFile: "Images/lebron.jpg",
    speed: 82,
    defending: 94,
    accuracy: 85,
    power: 90,
    info: "Dominant big man with elite rim protection, rebounding, and versatile scoring around the basket."
);

            var player4 = new Player(
                name: "D’Angelo Russell",
                age: 28,
                height: 193,
                country: "USA",
                team: team,
                imageFile: "Images/lebron.jpg",
                speed: 88,
                defending: 70,
                accuracy: 90,
                power: 75,
                info: "Skilled guard known for his shooting, ball-handling, and ability to create offense."
            );

            var player5 = new Player(
                name: "Austin Reaves",
                age: 26,
                height: 193,
                country: "USA",
                team: team,
                imageFile: "Images/lebron.jpg",
                speed: 85,
                defending: 75,
                accuracy: 88,
                power: 70,
                info: "Energetic forward/guard hybrid with good shooting touch and hustle on both ends."
            );

            var player6 = new Player(
                name: "Jarred Vanderbilt",
                age: 25,
                height: 203,
                country: "USA",
                team: team,
                imageFile: "Images/lebron.jpg",
                speed: 84,
                defending: 92,
                accuracy: 70,
                power: 86,
                info: "Athletic forward with intense defensive presence, rebounding ability, and physicality in the paint."
            );

            var player2 = new Player(
            name: "LeBron James",
            age: 39,
            height: 206, // cm
            country: "USA",
            team: team,
            imageFile: "Images/lebron.jpg",
            speed: 88,
            defending: 80,
            accuracy: 90,
            power: 92,
            info: "Widely regarded as one of the greatest basketball players of all time. Known for his athleticism, leadership, and basketball IQ."
            );

            team.AddTeamMember(player2);
            team.AddTeamMember(player3);
            team.AddTeamMember(player4);
            team.AddTeamMember(player5);
            team.AddTeamMember(player6);
        }

        public void PrototypeRun()
        {

            Team team1 = new Team();
            team1.TeamName = "Chicago Bulls";
            team1.ImageFile = "images/Chicago_Bulls_logo.svg.png";

            Team team2 = new Team();
            team1.TeamName = "Chicago Lakers";
            team2.ImageFile = "images/Los_Angeles_Lakers_logo.svg.png";

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

        }
    }
}

