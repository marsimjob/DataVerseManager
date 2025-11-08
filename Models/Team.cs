using System;
using System.Collections.Generic;
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

        // image path for Canvas.Image
        public string ImageFile { get; set; }

        // Constructor
        public Team()
        {
            //// Make a list with only the total powers of player
            //List<double> ListWinRate = new List<double>();
            //foreach (Player p in TeamPlayer)
            //{
            //    ListWinRate.Add(p.TotalStat);
            //}
            //// Each team's win rate is predicated on its Player's total stats
            //WinRate = ListWinRate.Average();
        }

        // Methods
        public void AddTeamMember(Player player)
        {
            if (TeamPlayer.Count < 5)
            {
                TeamPlayer.Add(player);
                player.PlayerTeam = this;
                Console.WriteLine($"{player.PlayerName} was added to {TeamName}");
            }
            else
            {
                // If team has over 5 members, choose which one to replace
            }
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
