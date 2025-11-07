using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    internal class Team
    {
        // Attributes
        public List<Player> TeamPlayer = new List<Player>();
        
        public string TeamName { get; set; }
        public int TeamWins {  get; set; }
        public int TeamLoses {  get; set; }

        public double WinRate { get; set; }

        // Constructor
        public Team()
        {
            // Make a list with only the total powers of player
            List<double> ListWinRate = new List<double>();
            foreach(Player p in TeamPlayer)
            {
                ListWinRate.Add(p.TotalStat);
            }
            // Each team's win rate is predicated on its Player's total stats
            WinRate = ListWinRate.Average();
        }

        // Methods
        public void AddTeamMember(Player player)
        {
            
            TeamPlayer.Add(player);
            // If team has over 5 members, choose which one to replace
        }
        public void ShowTeamPlayers()
        {
            // Show a list of your team
        }
       public Player GetPlayerFromTeamList(int playerIndex)
        {
            // Returns a Player object chosen by the user
            return TeamPlayer[playerIndex];
        }
    }
}
