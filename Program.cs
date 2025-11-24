using System;
using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Skapa två lag
            Team myTeam = new Team { TeamName = "Lakers" };
            Team opponentTeam = new Team { TeamName = "Warriors" };

            // 2. Bygg spelare i båda lagen
            Team.BuildTeam(myTeam);
            Team.BuildTeam(opponentTeam);

            // 3. Lista med alla lag i spelet
            List<Team> allTeams = new List<Team> { myTeam, opponentTeam };

            // 4. Skapa RandomMatch-objektet
            RandomMatch randomMatch = new RandomMatch();

            // 5. Starta matchen
            randomMatch.Play(myTeam, allTeams);
        }
    }
}
        







