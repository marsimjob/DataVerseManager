using Spectre.Console;

namespace DataVerseManager.Models
{
    internal class Rule
    {
       // This is a rule object to be looked up in the rulebook

        List<string> KeyWordList = new List<string>();
        private Markup subtitle;

        public int RuleNr {  get; set; }

        public string RuleName { get; set; }

        public string RuleInfo { get; set; }

        // Constructor
        public Rule(int ruleNr, string ruleName, string ruleInfo, string key1, string key2, string key3)
        {
            RuleNr = ruleNr;
            RuleName = ruleName;
            RuleInfo = ruleInfo;

            KeyWordList.Add(key1);
            KeyWordList.Add(key2);
            KeyWordList.Add(key3);
        }

        public Rule(Markup subtitle)
        {
            this.subtitle = subtitle;
        }
    }
}