using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager
{
    internal class Leaderboard
    {

        // *** WILL NEED A JSON PROCESS TO SAVE CURRENT LEADERBOARDS FOR FUTURE RUNS OF THE APP

        // Look at Table on SpectreConsole and use it for this display function
        List<Team> allTeams;

        public Leaderboard() 
        {
            // Add all teams here when we've finished all dummy data
            allTeams = new List<Team>(); 
        }

        public void DisplayLeaderBoard()
        {
            // Use the TeamList (should always be updated to newest) to do this:
            // Based on all the team's Wins or Loses show them from
            // Top to Bottom.
            // In games, you look at Kills-per-deaths. We look at wins-per-loss
            // So use the kills per death ratio to get a ranking order
            // Do they win more than they lose etc.
        }
    }
}
