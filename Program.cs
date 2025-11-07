using DataVerseManager.Models;
using DataVerseManager.Services;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Try catch stuff:
            Console.WriteLine("Type something with int");
            string input = Console.ReadLine();

            try
            {
                int inputToInt = int.Parse(input);
                Console.WriteLine("This is an int!");
            }
            catch (Exception ex) 
            {
                Console.WriteLine("This is NOT an int!");
            }

            // Save a list (or any other object to json)
            Matchboard newBoard = new Matchboard();
            JsonHandeler.SaveJson(newBoard.Matchboards, "matchboards.json");

            // Load a list (or any other object from json)
            List<Match> matches = new List<Match>();
            matches = JsonHandeler.LoadJson<List<Match>>("matchboards.json");
                
        }
    }
}

