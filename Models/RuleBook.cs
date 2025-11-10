using System;
using System.Collections.Generic;
using System.Linq;
using DataVerseManager.Models;

namespace DataVerseManager.Models
{
   /// <summary>
   /// RuleBook är "regelboken" – den innehåller en lista med alla regler
   /// och metoder för att söka och visa dem.
   /// </summary>
    internal static class RuleBook
    {
       /// <summary>
       /// Här ligger alla våra 15 regler.
       /// </summary>
        public static List<Rule> ListOfRules { get; } = new List<Rule>
{
new Rule(
1,
"Team and Game Setup",
"Each team has 5 players on the court. The goal is to score points by shooting the ball through the opponent's hoop.",
"team", "players", "setup"),

new Rule(
2,
"Scoring",
"A field goal is worth 2 or 3 points depending on distance. A free throw is worth 1 point.",
"points", "score", "shot"),

new Rule(
3,
"Game Duration",
"The game is divided into 4 quarters. The team with the highest score at the end wins.",
"time", "quarters", "duration"),

new Rule(
4,
"Starting Play",
"The game begins with a jump ball at the center circle between two opposing players.",
"start", "jump", "tipoff"),

new Rule(
5,
"Dribbling",
"A player must bounce the ball while moving. Running without dribbling is called traveling.",
"dribble", "bounce", "move"),

new Rule(
6,
"Passing",
"Players can pass the ball to teammates using chest, bounce, or overhead passes.",
"pass", "assist", "teamwork"),

new Rule(
7,
"Shooting",
"A player shoots the ball to score. Shots taken from beyond the three-point line are worth 3 points.",
"shoot", "basket", "three-pointer"),

new Rule(
8,
"Rebounding",
"When a shot is missed, players can jump to grab the ball. This is called a rebound.",
"rebound", "miss", "jump"),

new Rule(
9,
"Traveling",
"Traveling happens when a player moves their feet illegally while holding the ball.",
"travel", "steps", "violation"),

new Rule(
10,
"Double Dribble",
"A player cannot start dribbling again after stopping, or dribble with both hands at the same time.",
"double dribble", "mistake", "violation"),

new Rule(
11,
"Personal Fouls",
"Personal fouls involve illegal contact such as pushing, hitting, or holding.",
"foul", "contact", "defense"),

new Rule(
12,
"Free Throws",
"After certain fouls, the player gets free throws worth 1 point each.",
"free throw", "line", "foul shot"),

new Rule(
13,
"Three-Second Rule",
"An offensive player cannot stay in the paint for more than three seconds while their team has the ball.",
"paint", "key", "violation"),

new Rule(
14,
"Backcourt Violation",
"After crossing mid-court, the offensive team cannot return the ball to the backcourt.",
"backcourt", "midcourt", "rule"),

new Rule(
15,
"Timeouts",
"Coaches may call timeouts to stop play for strategy or to rest players.",
"timeout", "coach", "break")
};

     /// <summary>
     /// Låter användaren skriva in ett regelnummer eller ett nyckelord
     /// och försöker hitta en matchande regel.
     
        
     
        /// </summary>
        public static void SearchRule()
        {
            Console.Write("Write a rule number or a keyword to search: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("You must write something to search for.");
                return;
            }

            Rule? foundRule = null;

            // 1. Försök först tolka det användaren skrev som ett regelnummer
            if (int.TryParse(input, out int ruleNumber))
            {
                foundRule = ListOfRules.FirstOrDefault(r => r.RuleNr == ruleNumber);
            }

            // 2. Om ingen regel hittades med nummer, sök på text/nyckelord
            if (foundRule is null)
            {
                string searchText = input.Trim().ToLower();

                foundRule = ListOfRules.FirstOrDefault(r =>
                r.RuleName.ToLower().Contains(searchText) ||
                r.RuleInfo.ToLower().Contains(searchText) ||
                r.KeyWord1.ToLower().Contains(searchText) ||
                r.KeyWord2.ToLower().Contains(searchText) ||
                r.KeyWord3.ToLower().Contains(searchText));
            }

            // 3. Skriv ut resultat
            if (foundRule is null)
            {
                Console.WriteLine("No rule found that matches your search.");
            }
            else
            {
                ShowRule(foundRule);
            }
        }

        /// <summary>
        /// Skriver ut en regel på ett tydligt sätt.
        /// </summary>
        public static void ShowRule(Rule rule)
        {
            Console.WriteLine();
            Console.WriteLine($"Rule number : {rule.RuleNr}");
            Console.WriteLine($"Title : {rule.RuleName}");
            Console.WriteLine("Description :");
            Console.WriteLine(rule.RuleInfo);

            if (rule.KeyWordList.Count > 0)
            {
                Console.WriteLine($"Keywords : {string.Join(", ", rule.KeyWordList)}");
            }

            Console.WriteLine();
        }
    }
}