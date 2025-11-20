using System;
using System.Collections.Generic;
using Spectre.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataVerseManager.Models
{
    public class Team
    {
        // Attributes
        public List<Player> TeamPlayer = new List<Player>();

        public string TeamName { get; set; }
        public int TeamWins {  get; set; }
        public int TeamLoses {  get; set; }

        public double WinRate { get; set; }

        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public Color AccentColor { get; set; }

        // image path for Canvas.Image
        public string ImageFile { get; set; }

        // Constructor
        public Team()
        {
            PrimaryColor = "white";
            SecondaryColor = "grey";
            AccentColor = Color.Yellow;
        }

        // Methods
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
            imageFile: "Images/content.png",
            speed: 88,
            defending: 69,
            accuracy: 19,
            power: 44,
            info: "Widely regarded as one of the greatest basketball players of all time. Known for his athleticism, leadership, and basketball IQ."
            );

            team.AddTeamMember(player2);
            team.AddTeamMember(player3);
            team.AddTeamMember(player4);
            team.AddTeamMember(player5);
            team.AddTeamMember(player6);
        }
        public void CalculateWinLossRate()
        {
            // Make a list with only the total powers of player
            List<double> ListWinRate = new List<double>();
            foreach (Player p in TeamPlayer)
            {
                ListWinRate.Add(p.TotalStat);
            }
            // Each team's win rate is predicated on its Player's total stats
            WinRate = ListWinRate.Average();
        }
        public void AddTeamMember(Player player)
        {

            TeamPlayer.Add(player);
            player.PlayerTeam = this;
            Console.WriteLine($"{player.PlayerName} was added to {TeamName}");
        }
           
        public void ShowTeamPlayers()
        {
            foreach(Player member in TeamPlayer)
            {
                Console.WriteLine(member.PlayerName);
            }
        }
       public Player GetPlayerFromTeamList(int playerIndex)
        {
            // Returns a Player object chosen by the user
            return TeamPlayer[playerIndex];
        }
    }
}
