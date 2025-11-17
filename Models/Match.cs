using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    public class Match
    {
        // Attributes
        // Match date, when did the match play?
       public  DateTime MatchTime {  get; set; }

        // What teams played that match?
        public string TeamOne { get; set; }
        public string TeamTwo {  get; set; }

        // What was the final scores?
        public int OneScore { get; set; }
        public int TwoScore { get; set; }

        // Constructor
        public Match(DateTime matchTime, string teamOne, string teamTwo, int oneScore, int twoScore)
        {
            MatchTime = matchTime;
            TeamOne = teamOne;
            TeamTwo = teamTwo;
            OneScore = oneScore;
            TwoScore = twoScore;

        }

        // Det här en funktion som simulerar en match mellan två lag och talar om vilket lag som vann.
        // WinRate påverkar sannolikheten för att ett lag vinner.
        // Winrate ligger i team klassen

        public static Team SimulateMatch(Team teamA, Team teamB)
        {
            if (teamA == null || teamB == null)
                throw new ArgumentNullException("Team kan inte vara null.");

            // Skydda mot 0-winrate
            const double eps = 1e-9;
            double pA = Math.Max(teamA.WinRate, eps);
            double pB = Math.Max(teamB.WinRate, eps);

            // Räkna ut sannolikheten för att lag A vinner
            double total = pA + pB;
            double probabilityA = pA / total;

            // Skapa en slumpgenerator
            Random random = new Random();
            double randomValue = random.NextDouble(); // mellan 0 och 1

            // Om randomvärdet är mindre än sannolikheten → teamA vinner
            if (randomValue < probabilityA)
            {
                Console.WriteLine($"{teamA.TeamName} vann matchen!");
                return teamA;
            }
            else
            {
                Console.WriteLine($"{teamB.TeamName} vann matchen!");
                return teamB;
            }
        }





    }
}
