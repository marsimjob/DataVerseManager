using DataVerseManager.Models;
using DataVerseManager.Services;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonPath = "matchboards.json";

            Matchboard board = new Matchboard();

            
            // Försök ladda JSON,0
            board.Matchboards = JsonHandeler.LoadJson<List<Match>>(jsonPath);

            // Visa tabell
            board.DisplayLatestMatches();

            board.SearchByTeam();

        }
    }
}

