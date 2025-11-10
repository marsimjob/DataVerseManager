using DataVerseManager.Models;
using DataVerseManager.Services;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
          

            // Save a list (or any other object to json)
            Matchboard newBoard = new Matchboard();
            JsonHandeler.SaveJson(newBoard.Matchboards, "matchboards.json");

            // Load a list (or any other object from json)
            List<Match> matches = new List<Match>();
            matches = JsonHandeler.LoadJson<List<Match>>("matchboards.json");



            // Skapar ett objekt av Leaderboard-klassen
            Leaderboard leaderboard = new Leaderboard();

            // Kör programmet
            leaderboard.Run();

                



        }
    }
}

