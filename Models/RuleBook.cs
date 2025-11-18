using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace DataVerseManager.Models;
internal static class RuleBook
{
    // Rulebook script is made to look up basketball rules by the user as a distraction and to study up!

    // We have 10 rules in total that we can show
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
"shoot", "shot", "three-pointer"),

new Rule(
8,
"Rebounding",
"When a shot is missed, players can jump to grab the ball. This is called a rebound.",
"rebound", "miss", "jump"),

new Rule(
9,
"Timeouts",
"Coaches may call timeouts to stop play for strategy or to rest players.",
"timeout", "coach", "break"),

new Rule(
10,
"Three-Second Rule",
"An offensive player cannot stay in the paint for more than three seconds while their team has the ball.",
"paint", "key", "violation"),

};

    /// <summary>
    /// Låter användaren skriva in ett regelnummer eller ett nyckelord
    /// och försöker hitta en matchande regel.
    /// </summary>

    public static void RunRuleBook()
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.Clear();

            SpectreGeneric.PresentTopTitle("RULE BOOK", AppSettings.MainColor, AppSettings.SubColor);

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[{AppSettings.MainColor}]Choose browsing method: [/]")
                    .PageSize(5)
                    .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                    .AddChoices(new[] {
                        "SEARCH RULE",
                        "LIST ALL RULES",
                        "RETURN TO TOP MENU" }));

            switch (choice)
            {
                case "SEARCH RULE":
                    SearchRuleInteractive();
                    break;
                case "LIST ALL RULES":
                    ListAllRules();
                    break;
                case "RETURN TO TOP MENU":
                    keepRunning = false;
                    break;
            }
        }
    }
    private static void SearchRuleInteractive()
    {
        bool searching = true;

        while (searching)
        {
            Console.Clear();
            SpectreGeneric.PresentTopTitle("RULE BOOK", AppSettings.MainColor, AppSettings.SubColor);

            AnsiConsole.MarkupLine("[grey]Press ESC to return to the menu.[/]");
            AnsiConsole.Markup("[green]Enter a rule number or keyword:[/] ");

            string input = "";
            ConsoleKeyInfo keyInfo;

            // --- ESC + typed-input reader ---
            while (true)
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    searching = false;
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                input += keyInfo.KeyChar;
                Console.Write(keyInfo.KeyChar);
            }

            if (!searching)
                break;

            // Unify and trim the input so the search is eaiser
            input = input.Trim().ToLower();

            // Loop if input is empty
            if (string.IsNullOrWhiteSpace(input))
                continue;

            // Look for rules that match
            List<Rule> foundRules = new List<Rule>();

            // If input is a number, try matching rule number
            if (input.All(char.IsDigit))
            {
                int number = int.Parse(input);
                var exact = ListOfRules.FirstOrDefault(r => r.RuleNr == number);
                if (exact != null)
                {
                    foundRules.Add(exact);
                }
            }

            // LEVEL 1...
            // If number isnt enough and it cant find any rules with them search keyword or title
            if (foundRules.Count == 0)
            {
                foreach (var r in ListOfRules)
                {
                    // if input match any of the rules at RuleName, KeyWord1, KeyWord2, KeyWord3

                    if (r.RuleName.ToLower().Contains(input) ||
                        r.KeyWord1.ToLower().Contains(input) ||
                        r.KeyWord2.ToLower().Contains(input) ||
                        r.KeyWord3.ToLower().Contains(input))
                    {
                        foundRules.Add(r);
                    }
                }
            }

            // LEVEL 2...
            // If no matches even with keywords or titles
            if (foundRules.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No matching rule found.[/]");
                AnsiConsole.MarkupLine("[grey]Press any key to search for a new rule...[/]");
                Console.ReadLine();
                // Loop to the top
                continue;
            }
            // If found matches
            // If only one match
            if (foundRules.Count == 1)
            {
                ShowRule(foundRules[0]);
                // Loop to the top
                continue;
            }

            // LEVEL 3...
            // Finally if none of the above work:
            // If multiple matches the MultiRuleSelection method will list them and let the user choose
            Rule selectedRule = MultiRuleSelection(foundRules);
            
            // The choice shows up here
            ShowRule(selectedRule);
        }
    }
    private static void ListAllRules()
    {
        // Create a table to organize rules
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Expand();

        // Define columns
        table.AddColumn(new TableColumn("[bold yellow]Rule #[/]").Centered());
        table.AddColumn(new TableColumn("[bold green]Title[/]").Centered());
        table.AddColumn(new TableColumn("[bold white]Description[/]").Centered());
        table.AddColumn(new TableColumn("[grey]Keywords[/]").Centered());

        // Add rows for each rule
        foreach (var rule in ListOfRules)
        {
            string keywords = rule.KeyWordList.Count > 0 ? string.Join(", ", rule.KeyWordList) : "-";
            table.AddRow(
                rule.RuleNr.ToString(),
                rule.RuleName.EscapeMarkup(),
                rule.RuleInfo.EscapeMarkup(),
                keywords.EscapeMarkup()
            );
        }

        // Render the table
        AnsiConsole.Write(table);

        // Wait for user input
        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
        Console.ReadKey(true);
    }
    private static Rule MultiRuleSelection(List<Rule> rules)
    {
        var prompt = new SelectionPrompt<Rule>()
            .Title("[yellow]Multiple rules matched. Select one:[/]")
            .UseConverter(r => $"Rule {r.RuleNr}: {r.RuleName}")
            .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
            .AddChoices(rules);

        return AnsiConsole.Prompt(prompt);
    }
    public static void ShowRule(Rule rule)
    {
        Console.Clear();
        SpectreGeneric.PresentTopTitle("RULE BOOK", AppSettings.MainColor, AppSettings.SubColor);

        AnsiConsole.MarkupLine($"[grey]Rule:[/] [bold yellow]#{rule.RuleNr}[/]");
        AnsiConsole.MarkupLine($"[grey]Title:[/] [bold blue]{rule.RuleName}[/]");
        AnsiConsole.MarkupLine("[grey]Description: [/]");
        AnsiConsole.MarkupLine($"[white]{rule.RuleInfo}[/]");

        if (rule.KeyWordList.Count > 0)
        { AnsiConsole.MarkupLine($"[grey]Keywords:[/] [cyan]{string.Join(", ", rule.KeyWordList)}[/]"); }
        Console.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to search for a new rule...[/]");
        Console.ReadLine();
    }
}
