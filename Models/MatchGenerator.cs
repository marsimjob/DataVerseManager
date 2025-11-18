
using DataVerseManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    static class MatchGenerator
    {
        public static List<Team> AllTeams = JsonHandeler.LoadJson<List<Team>>("allteams.json");

        public static (Team teamA, Team teamB) GameGenerator()
        {
            // Hur ska vi generera matcher som sparas så vi kan betta eller så på dem?

            // allteams json filen har alla team
            // Slumpa fram två lag som ska spela
            Random random = new Random();
            Team randomTeamA = AllTeams[random.Next(AllTeams.Count)];
            Team randomTeamB = AllTeams[random.Next(AllTeams.Count)];
            
            return (randomTeamA, randomTeamB);
        }
    }
}
