using System;
using System.Collections.Generic;
using Spectre.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataVerseManager.Models.Leaderboard;
namespace DataVerseManager.Models
{
    public class Team
    {
        // Attributes
        public List<Player> TeamPlayer { get; set; } = new List<Player>();

        public string TeamName { get; set; }
        public int TeamWins { get; set; }
        public int TeamLoses { get; set; }

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

        // Team.BuildTeam(heat)
        // Builts team at initiation so that all team members in the team gets the right Team Object 
        public static void BuildTeam(Team team)
        {
            // A double that collects all the the Total Stats from players to then get avarage from later
            double totalPower = 0; 

            foreach (Player player in team.TeamPlayer)
            {
                // Give the players of the team this team as their team
                player.PlayerTeam = team;

                // each itteration adds on to the totalPower
                totalPower += player.CalculateTotalStat();
            }
            
            // Avarage the totalPower to get the WinRate for this team
            team.WinRate = totalPower/team.TeamPlayer.Count;
        }

        //heat.UpdateWinRate();
        // This will just update the Team WinRate when a match is over so that their wins and losses
        // are reflected and saved for later
        public void UpdateTeamWinRate()
        {
            // A double that collects all the the Total Stats from players to then get avarage from later
            double totalPower = 0;

            foreach (Player player in TeamPlayer)
            {
                // each itteration adds on to the totalPower
                totalPower += player.CalculateTotalStat();
            }

            // Avarage the totalPower to get the WinRate for this team
            WinRate = totalPower / TeamPlayer.Count;
        }
        //public void CalculateWinLossRate()
        //{
        //    // Make a list with only the total powers of player
        //    List<double> ListWinRate = new List<double>();
        //    foreach (Player p in TeamPlayer)
        //    {
        //        ListWinRate.Add(p.TotalStat);
        //    }
        //    // Each team's win rate is predicated on its Player's total stats
        //    WinRate = ListWinRate.Average();
        //}
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
