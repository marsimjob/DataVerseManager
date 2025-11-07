using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    internal static class RuleBook
    {
        /// <summary>
        /// UPPGIFT: GÖR EN UPPLSAGSBOK FÖR REGLER MED ETT SÖKFÄLLT SOM SÖKER EFTER ANTINGEN REGELNAMNET, REGELNUMRET ELLER REGEL KEY WORDS.
        /// NÄR MAN SLÅR UPP REGELN SKA MAN KUNNA LÄSA OCH FÖRSÄTTA SÖKA VIDARE. KOLLA RULE CLASS FÖR ATT SEE INFORMATION OM REGEL OBJEKTEN.
        /// </summary> 

        // This class should work as a dictionary for rules of Basketball.
        // The user should write in a keyword that, if it has matches, should bring them to
        // a page of information on the rule.

        // Create a rule book with all the rules necessary - 15 rules for now
        public static List<Rule> listOfRules = new List<Rule>
        {
            new Rule (0, "", "", "", "", ""),
            
            new Rule (1, // Rule Number
                "Team and Game Setup", // Rule Name
                "Each team has 5 players on the court. The goal is to score poitns by shooting the ball through the oppentent's hoop", // Rule Info
                "Setup", "Shooting", "Team"), // 3 Key Words
            
            new Rule (2, "", "", "", "", ""),
            new Rule (3, "", "", "", "", "")
        };

        // Method
        public static void SearchRule()
        {
            //** USE TRY CATCH TO MAKE SURE THE USER DOESNT USE THE WRONG INPUT TYPES --- WEIRD SYMBOLS ETC

            // Ask the user to write in a search word
            // The search word can be either the rule Number, rule Name
            // Or search for any of the key words
            // Use LINQ Where any rule in the ListOfRules matches the written word
            // If firstorDefault found, use the ShowRule() method to display the rule 
        }

        public static void ShowRule(Rule rule)
        {
            Console.WriteLine($"Nr:  {rule.RuleNr}");
            Console.WriteLine($"Name:  {rule.RuleName}");
            Console.WriteLine($"Info:  {rule.RuleInfo}");
        }
    }
}
