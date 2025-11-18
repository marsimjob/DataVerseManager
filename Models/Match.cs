using DataVerseManager.Services;
using NJsonSchema.Validation.FormatValidators;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using Spectre.Console;
using Spectre.Console.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static DataVerseManager.Models.Leaderboard;
namespace DataVerseManager.Models
{
    public class Match
    {
        // Attributes
        // Match date, when did the match play?
        public DateTime MatchTime { get; set; }

        // What teams played that match?
        public string TeamOne { get; set; }
        public string TeamTwo { get; set; }

        // What was the final scores?
        public int OneScore { get; set; }
        public int TwoScore { get; set; }

        public int MatchID { get; set; }
        // Constructor
        public Match(DateTime time, Team teamOne, Team teamTwo,  int oneScore, int twoScore, int id)
        {
            MatchTime = time;
            TeamOne = teamOne.TeamName;
            TeamTwo = teamTwo.TeamName;
            OneScore = oneScore;
            TwoScore = twoScore;
            MatchID = id;
        }
        // Parameterless constructor for JSON deserialization
        public Match() { }
    }
}
       
