using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    public static class Matchboard
    {
        /// <summary>
        /// UPPGIFT: SKAPA EN MATCHBOARD SOM VISAR VILKA MATCHER SOM HAR HÄNT SENAST MED SPECTRE CONSOLE TABELL
        /// SPARA TILL JSON OCH ÅTERANVÄND SAMMA LISTA NÄSTA GÅNG DU KÖR PROGRAMMET. ANVÄND LINQ FÖR ATT FILTRERA.
        /// KOLLA MATCH CLASS, ALLA VARIABLER STÅR DÄR.
        /// </summary> 

        /// *** WILL USE A JSON FILE TO SAVE ALL MATCHES IN THE FINAL APP

        // Search for matches through Date with LINQ, Or any other variable in the Match class (Score, who played etc)
        public static List<Match> Matchboards = JsonHandeler.LoadJson<List<Match>>("matchboards.json");
  
        public static void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                SpectreGeneric.PresentTopTitle("MATCHBOARD", AppSettings.MainColor, AppSettings.SubColor);

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[{AppSettings.MainColor}]What would you like to do?[/]")
                        .PageSize(5)
                        .HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                        .AddChoices(new[]
                        {
                    "VIEW LATEST MATCHES",
                    "SEARCH BY TEAM",
                    "RETURN TO TOP MENU"
                        })
                );

                switch (choice)
                {
                    case "VIEW LATEST MATCHES":
                        DisplayLatestMatches();
                        break;

                    case "SEARCH BY TEAM":
                        SearchByTeam();
                        break;

                    case "RETURN TO TOP MENU":
                        running = false;
                        break;
                }

                if (running)
                {
                    AnsiConsole.WriteLine();
                    Console.ReadLine();
                }
            }
        }

        public static void DisplayLatestMatches(int daysBack = 7)
        {
            var latest = Matchboards
                .Where(m => m.MatchTime >= DateTime.Today.AddDays(-daysBack))
                .OrderByDescending(m => m.MatchTime)
                .ToList();

            var table = new Table();
            table.Border(TableBorder.DoubleEdge);

            table.AddColumn("Date");
            table.AddColumn("Home");
            table.AddColumn("Away");
            table.AddColumn("Result");

            foreach (var m in latest)
            {
                string result;

                if (m.OneScore > m.TwoScore)
                {
                    // TeamOne wins → OneScore green, TwoScore red
                    result = $"[green]{m.OneScore}[/] - [red]{m.TwoScore}[/]";
                }
                else if (m.OneScore < m.TwoScore)
                {
                    // TeamTwo wins → TwoScore green, OneScore red
                    result = $"[red]{m.OneScore}[/] - [green]{m.TwoScore}[/]";
                }
                else
                {
                    // Draw
                    result = $"[yellow]{m.OneScore} - {m.TwoScore}[/]";
                }

                table.AddRow(
                    m.MatchTime.ToShortDateString(),
                    m.TeamOne,
                    m.TeamTwo,
                    result
                );
            }

            AnsiConsole.Write(table);
        }

        public static void SearchByTeam()
        {
            // Lista med tillgängliga lag
            var teams = new List<string>
    {
        "Warriors",
        "Lakers",
        "Knicks",
        "Bulls",
        "Celtics",
        "Heat",
        "Nets",
        "Mavericks",
        "Clippers",
        "Rockets"
    };

            // Spectre.Console selection prompt
            string team = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a team to search for:[/]")
                    .PageSize(10)
                    .AddChoices(teams)
            );

            var matches = Matchboards
                .Where(m =>
                    m.TeamOne.Contains(team, StringComparison.OrdinalIgnoreCase) ||
                    m.TeamTwo.Contains(team, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(m => m.MatchTime)
                .ToList();

            var table = new Table();
            table.Border(TableBorder.DoubleEdge);

            table.AddColumn("Date");
            table.AddColumn("Home");
            table.AddColumn("Away");
            table.AddColumn("Result");

            foreach (var m in matches)
            {
                string result;

                if (m.OneScore > m.TwoScore)
                {
                    // TeamOne wins >> OneScore green othewise TwoScore red
                    result = $"[green]{m.OneScore}[/] - [red]{m.TwoScore}[/]";
                }
                else if (m.OneScore < m.TwoScore)
                {
                    // TeamTwo wins >> TwoScore green/OneScore red
                    result = $"[red]{m.OneScore}[/] - [green]{m.TwoScore}[/]";
                }
                else
                {
                    // Draw
                    result = $"[yellow]{m.OneScore} - {m.TwoScore}[/]";
                }
                table.AddRow(
                    m.MatchTime.ToShortDateString(),
                    m.TeamOne,
                    m.TeamTwo,
                    result

                );
            }

            AnsiConsole.Write(table);
        }
    }
}
