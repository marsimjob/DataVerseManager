using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVerseManager.Services;

namespace DataVerseManager.Models
{
    internal class Matchboard
    {
        /// <summary>
        /// UPPGIFT: SKAPA EN MATCHBOARD SOM VISAR VILKA MATCHER SOM HAR HÄNT SENAST MED SPECTRE CONSOLE TABELL
        /// SPARA TILL JSON OCH ÅTERANVÄND SAMMA LISTA NÄSTA GÅNG DU KÖR PROGRAMMET. ANVÄND LINQ FÖR ATT FILTRERA.
        /// KOLLA MATCH CLASS, ALLA VARIABLER STÅR DÄR.
        /// </summary> 

        /// *** WILL USE A JSON FILE TO SAVE ALL MATCHES IN THE FINAL APP

        // Search for matches through Date with LINQ, Or any other variable in the Match class (Score, who played etc)
        public List<Match> Matchboards = new List<Match>
        { new Match(DateTime.Today, "Bulls", "Lakers", 90, 120),
          new Match(DateTime.Today, "Team 1", "Team 2", 84, 130) }; // Dummy data, make some Matches like this, then create a Matchboard application

    }
}
