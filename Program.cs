namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Crystal Coders!");
            Betting betSystem = new Betting();

            RuleBook.ShowRule(RuleBook.listOfRules[1]);
        }
    }
}
