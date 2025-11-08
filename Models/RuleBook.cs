using System;
using System.Collections.Generic;
using System.Linq;

namespace DataVerseManager.Models
{
    internal static class RuleBook
    {
        // Denna lista är själva "uppslagsboken" – här ligger alla reglerna.
        public static List<Rule> ListOfRules = new List<Rule>
{
new Rule(
1,
"Team and Game Setup",
"Each team has 5 players on the court. The goal is to score points by shooting the ball through the opponent's hoop.",
"Setup", "Team", "Players"),

new Rule(
2,
"Scoring",
"A field goal inside the three-point line is worth 2 points. Behind the three-point line it is worth 3 points. A made free throw is worth 1 point.",
"Points", "Three-point", "Free throw"),

new Rule(
3,
"Game Duration",
"The game is divided into periods (quarters or halves). The team with the most points at the end of the game wins.",
"Time", "Periods", "Score"),

new Rule(
4,
"Tip-off",
"The game starts with a jump ball at center court between two players, one from each team.",
"Start", "Jump ball", "Center"),

new Rule(
5,
"Dribbling",
"A player must bounce (dribble) the ball while moving. Running without dribbling is not allowed.",
"Dribble", "Move", "Ball control"),

new Rule(
6,
"Travelling",
"A travelling violation happens when a player takes too many steps without dribbling the ball.",
"Travel", "Steps", "Violation"),

new Rule(
7,
"Double Dribble",
"A player may not stop dribbling, hold the ball, and then start dribbling again.",
"Double", "Dribble", "Violation"),

new Rule(
8,
"Personal Foul",
"Illegal physical contact with an opponent, such as hitting, pushing or blocking unfairly, is a personal foul.",
"Foul", "Contact", "Defense"),

new Rule(
9,
"Shooting Foul",
"If a defender fouls a shooter while they are taking a shot, the shooter is awarded free throws.",
"Shooting", "Foul", "Free throws"),

new Rule(
10,
"Free Throws",
"Free throws are taken from the free-throw line with no defense, after certain fouls.",
"Free throw", "Line", "Foul"),

new Rule(
11,
"Three-Second Rule",
"An offensive player may not stay in the key (paint) area for more than three seconds while their team has the ball.",
"Three seconds", "Key", "Paint"),

new Rule(
12,
"Backcourt Violation",
"Once the offensive team has brought the ball over the midcourt line, they may not return the ball to the backcourt.",
"Backcourt", "Midcourt", "Violation"),

new Rule(
13,
"Out of Bounds",
"The ball is out of bounds when it touches a player or the floor outside the boundary lines.",
"Sideline", "Baseline", "Out"),

new Rule(
14,
"Substitutions",
"Players may be substituted during stoppages in play when allowed by the referee.",
"Subs", "Bench", "Players"),

new Rule(
15,
"Time-outs",
"Coaches may request time-outs to stop the game clock and talk to their team.",
"Timeout", "Coach", "Clock")
};

        // ===========================
        // SÖK EFTER EN REGEL
        // ===========================
        public static void SearchRule()
        {
            try
            {
                Console.WriteLine("Write a rule number, name or keyword to search for:");
                Console.Write("> ");
                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("You must write something to search for.");
                    return;
                }

                Rule foundRule = null;

                // 1) Försök först tolka som ett regelnummer
                if (int.TryParse(input, out int ruleNumber))
                {
                    foundRule = ListOfRules
                    .FirstOrDefault(r => r.RuleNr == ruleNumber);
                }
                else
                {
                    // 2) Annars sök på namn, info eller nyckelord (case-insensitive)
                    string search = input.Trim().ToLower();

                    foundRule = ListOfRules.FirstOrDefault(r =>
                    r.RuleName.ToLower().Contains(search) ||
                    r.RuleInfo.ToLower().Contains(search) ||
                    r.KeyWord1.ToLower().Contains(search) ||
                    r.KeyWord2.ToLower().Contains(search) ||
                    r.KeyWord3.ToLower().Contains(search));
                }

                if (foundRule != null)
                {
                    ShowRule(foundRule);
                }
                else
                {
                    Console.WriteLine("No rule matched your search.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong with your input. Please try again.");
            }
        }

        // ===========================
        // VISA EN REGEL
        // ===========================
        public static void ShowRule(Rule rule)
        {
            Console.WriteLine();
            Console.WriteLine($"Nr: {rule.RuleNr}");
            Console.WriteLine($"Name: {rule.RuleName}");
            Console.WriteLine($"Info: {rule.RuleInfo}");
            Console.WriteLine();
        }
    }
}