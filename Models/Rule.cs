using System.Collections.Generic;
using Spectre.Console;
namespace DataVerseManager.Models
{
    /// <summary>
    /// Representerar EN regel i regelboken.
    /// Håller reda på nummer, namn, beskrivning och nyckelord.
    /// </summary>
    internal class Rule
    {
        // Regelns nummer, t.ex. 1, 2, 3...
        public int RuleNr { get; set; }

        // Regelns rubrik, t.ex. "Team and Game Setup"
        public string RuleName { get; set; } = string.Empty;

        // Text som förklarar regeln
        public string RuleInfo { get; set; } = string.Empty;

        // Tre nyckelord som används för sökning
        public string KeyWord1 { get; internal set; } = string.Empty;
        public string KeyWord2 { get; internal set; } = string.Empty;
        public string KeyWord3 { get; internal set; } = string.Empty;

        // Lista med alla nyckelord (bra när vi vill visa dem)
        public List<string> KeyWordList { get; } = new List<string>();

        /// <summary>
        /// Skapar en ny regel.
        /// </summary>
        public Rule(int ruleNr, string ruleName, string ruleInfo,
        string key1, string key2, string key3)
        {
            RuleNr = ruleNr;
            RuleName = ruleName;
            RuleInfo = ruleInfo;

            KeyWord1 = key1;
            KeyWord2 = key2;
            KeyWord3 = key3;

            // Lägg bara till nyckelord som inte är tomma
            if (!string.IsNullOrWhiteSpace(key1)) KeyWordList.Add(key1);
            if (!string.IsNullOrWhiteSpace(key2)) KeyWordList.Add(key2);
            if (!string.IsNullOrWhiteSpace(key3)) KeyWordList.Add(key3);
        }
    }
}