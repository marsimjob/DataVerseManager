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
       public DateTime MatchTime {  get; set; }

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
    }
}
