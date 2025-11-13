using System;
using DataVerseManager.Models;
﻿using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

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
          
            Team lakers = new Team
            {
                TeamName = "Los Angeles Lakers",
                ImageFile = "lakers.png"
            };  
            lakers.WinRate = 52;   

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
            // Load a list (or any other object from json)
            List<Match> matches = new List<Match>();
            matches = JsonHandeler.LoadJson<List<Match>>("matchboards.json");


            Bulls.WinRate = 48; 


            Betting betting = new Betting();

            var (lakersOdds, bullsOdds) = Betting.GetOdds(lakers , Bulls);

            betting.ShowBettingTable(lakers, Bulls);

            double mymoney = 10000;
          

            mymoney += betting.PlaceBet(lakers, Bulls);

            Console.WriteLine($"Remaining money after bet: {mymoney}");


          






        }
    }
}

