using System;
using DataVerseManager.Models;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Basketball RuleBook";
            bool running = true;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🏀 Welcome to the Basketball RuleBook!");
            Console.ResetColor();
            Console.WriteLine("You can search for rules by number or keyword.");
            Console.WriteLine();

            while (running)
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("1. Search for a rule");
                Console.WriteLine("2. Show all rule titles");
                Console.WriteLine("3. Exit");
                Console.WriteLine("------------------------------------------------");
                Console.Write("Choose an option (1–3): ");
                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        // Anropa din redan befintliga sökmetod i RuleBook
                        RuleBook.SearchRule();
                        break;

                    case "2":
                        // Visa en enkel lista med alla regler
                        Console.WriteLine("All rules in the RuleBook:");
                        Console.WriteLine("--------------------------");
                        foreach (var rule in RuleBook.ListOfRules)
                        {
                            Console.WriteLine($"{rule.RuleNr}. {rule.RuleName}");
                        }
                        Console.WriteLine();
                        break;

                    case "3":
                        running = false;
                        Console.WriteLine("Goodbye! 👋");
                        break;

                    default:
                        Console.WriteLine("Please choose a valid option (1, 2 or 3).");
                        break;
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.Write("Press ENTER to continue...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
    }
}

