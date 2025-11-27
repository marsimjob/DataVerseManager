namespace DataVerseManager.Models
{
        public class LeaderBoardStat
        {
            public string TeamName { get; set; } = string.Empty; // Name of the team
            public int TeamWins { get; set; }                     // Number of wins
            public int TeamLoses { get; set; }                    // Number of losses
            public double WinRate { get; set; }                   // Win/Loss ratio (wins per loss)
        }
}
