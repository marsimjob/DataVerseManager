using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataVerseManager.Models
{
    public class Leaderboard // Klassen som sköter allt i programmet
    {


        // En lista som håller alla lag i leaderboarden
        private System.Collections.Generic.List<TeaM> teams = new System.Collections.Generic.List<TeaM>();

        // ---------- INRE KLASS SOM BESKRIVER ETT LAG ----------

        public class TeaM
        {
            public string TeamName { get; set; } = string.Empty; // Namnet på laget
            public int TeamWins { get; set; }                     // Antal vinster
            public int TeamLoses { get; set; }                    // Antal förluster
            public double WinRate { get; set; }                   // W/L-förhållande (vinster per förlust)
        }

        // ---------- STARTPUNKT FÖR LEADERBOARD ----------

        public void Run()
        {
            // 1. Skapa alla lag (med 0 vinster och förluster)
            CreateDefaultTeams();

            // 2. Lägg till några exempelmatcher för att visa funktionen (Dummy data)
            RecordGame("Lakers", "Celtics");   // Lakers vinner mot Celtics
            RecordGame("Warriors", "Heat");    // Warriors vinner mot Heat
            RecordGame("Knicks", "Bulls");     // Knicks vinner mot Bulls
            RecordGame("Warriors", "Lakers");  // Warriors vinner mot Lakers

            // 3. Visa leaderboarden
            DisplayLeaderBoard();

            // 4. Vänta på tangenttryckning innan konsolen stängs
            AnsiConsole.MarkupLine("\n[grey]Tryck valfri tangent för att avsluta...[/]");
            System.Console.ReadKey();
        }

        // ---------- SKAPA STANDARDLAG ----------

        private void CreateDefaultTeams()
        {
            // Skapar de 10 lagen med 0 vinster och 0 förluster
            teams.Add(new TeaM { TeamName = "Warriors" });
            teams.Add(new TeaM { TeamName = "Lakers" });
            teams.Add(new TeaM { TeamName = "Knicks" });
            teams.Add(new TeaM { TeamName = "Bulls" });
            teams.Add(new TeaM { TeamName = "Celtics" });
            teams.Add(new TeaM { TeamName = "Heat" });
            teams.Add(new TeaM { TeamName = "Nets" });
            teams.Add(new TeaM { TeamName = "Mavericks" });
            teams.Add(new TeaM { TeamName = "Clippers" });
            teams.Add(new TeaM { TeamName = "Rockets" });

            JsonHandeler.SaveJson(teams, "leaderboard.json");



        }

        // ---------- REGISTRERA MATCH ----------

        private void RecordGame(string winnerTeamName, string loserTeamName)
        {
            // Hitta laget som vann 
            TeaM winner = teams.FirstOrDefault(
                t => t.TeamName.Equals(winnerTeamName, System.StringComparison.OrdinalIgnoreCase)
            );

            // Hitta laget som förlorade
            TeaM loser = teams.FirstOrDefault(
                t => t.TeamName.Equals(loserTeamName, System.StringComparison.OrdinalIgnoreCase)
            );

            // Om något av lagen inte finns, skriv felmeddelande och avsluta funktionen
            if (winner == null || loser == null)
            {
                AnsiConsole.MarkupLine("[red]Fel: Ett eller båda lag finns inte.[/]");
                return;
            }

            // Öka vinstantal för vinnaren
            winner.TeamWins++;

            // Öka förlustantal för förloraren
            loser.TeamLoses++;

            // Uppdatera WinRate för alla lag efter ändringen
            RecalculateWinRatio();
        }

        // ---------- BERÄKNA W/L FÖR VARJE LAG ----------

        private void RecalculateWinRatio()
        {
            // Gå igenom alla lag
            foreach (TeaM team in teams)
            {
                // Om laget inte har några förluster → använd antalet vinster som WinRate
                if (team.TeamLoses == 0)
                {
                    team.WinRate = (double)team.TeamWins;
                }
                else
                {
                    // Annars beräkna vinster delat med förluster
                    team.WinRate = (double)team.TeamWins / (double)team.TeamLoses;
                }
            }
        }

        // ---------- VISA LEADERBOARD SOM TABELL ----------

        private void DisplayLeaderBoard()
        {
            // Sortera lagen:
            // 1. Högst W/L först
            // 2. Flest vinster
            // 3. Färst förluster
            System.Collections.Generic.List<TeaM> sortedTeams = teams
                .OrderByDescending(t => t.WinRate)
                .ThenByDescending(t => t.TeamWins)
                .ThenBy(t => t.TeamLoses)
                .ToList();

            // Skapa en tabell
            Table table = new Table();

            // Lägg till rundade kanter för snyggare utseende
            table.Border = TableBorder.Rounded;

            // Skapa kolumner i tabellen
            table.AddColumn(new TableColumn("[grey]NR[/]").Centered());        // Placering (1:a, 2:a, 3:e)
            table.AddColumn(new TableColumn("[bold]Lag[/]"));                 // Lagnamn
            table.AddColumn(new TableColumn("[green]Vinster[/]").Centered()); // Antal vinster
            table.AddColumn(new TableColumn("[red]Förluster[/]").Centered()); // Antal förluster
            table.AddColumn(new TableColumn("[blue]W/L[/]").Centered());    // Win/Loss-ratio

            // Håller reda på placeringen (1:a, 2:a, osv.)
            int rank = 1;

            // Lägg till en rad för varje lag i sorteringsordning
            foreach (TeaM team in sortedTeams)
            {
                // Lägg till en rad med färgade värden
                table.AddRow(
                    rank.ToString(),                                    // Placering
                    "[white]" + team.TeamName + "[/]",                  // Lagnamn
                    "[green]" + team.TeamWins + "[/]",                  // Vinster
                    "[red]" + team.TeamLoses + "[/]",                   // Förluster
                    "[blue]" + team.WinRate.ToString("F2") + "[/]"    // W/L-ratio (två decimaler)
                );

                // Gå till nästa placering
                rank++;
            }

            // Skapa en panel (ram) runt tabellen med rubriken "Leaderboard"
            Panel panel = new Panel(table)
                .Header("[bold]Leaderboard[/]") // Rubrik överst
                .Padding(1, 1, 1, 1);            // Marginal runt tabellen

            // Skriv ut panelen (och tabellen) i konsolen
            AnsiConsole.Write(panel);
        }
    }
}