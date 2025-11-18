using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataVerseManager.Models
{
    public class Leaderboard
    {
        // A list that holds all teams in the leaderboard
        private List<Team> teams = new List<Team>();

        // ---------- INNER CLASS THAT DESCRIBES A TEAM ----------
        public class Team
        {
            public string TeamName { get; set; } = string.Empty; // Name of the team
            public int TeamWins { get; set; }                     // Number of wins
            public int TeamLoses { get; set; }                    // Number of losses
            public double WinRate { get; set; }                   // Win/Loss ratio (wins per loss)
        }

        // ---------- START POINT FOR THE LEADERBOARD ----------
        public void Run()
        {
            // 1. Create all teams (with 0 wins and 0 losses)
            CreateDefaultTeams();

            // 2. Add some example games to show functionality (dummy data)
            RecordGame("Lakers", "Celtics");   // Lakers win against Celtics
            RecordGame("Warriors", "Heat");    // Warriors win against Heat
            RecordGame("Knicks", "Bulls");     // Knicks win against Bulls
            RecordGame("Warriors", "Lakers");  // Warriors win against Lakers

            // 3. Display the leaderboard
            DisplayLeaderBoard();

            // 4. Wait for key press before closing console
            AnsiConsole.MarkupLine("\n[grey]Press any key to exit...[/]");
            Console.ReadKey();
        }

        // ---------- CREATE DEFAULT TEAMS ----------
        private void CreateDefaultTeams()
        {
            // Create 10 teams with 0 wins and 0 losses
            teams.Add(new Team { TeamName = "Warriors" });
            teams.Add(new Team { TeamName = "Lakers" });
            teams.Add(new Team { TeamName = "Knicks" });
            teams.Add(new Team { TeamName = "Bulls" });
            teams.Add(new Team { TeamName = "Celtics" });
            teams.Add(new Team { TeamName = "Heat" });
            teams.Add(new Team { TeamName = "Nets" });
            teams.Add(new Team { TeamName = "Mavericks" });
            teams.Add(new Team { TeamName = "Clippers" });
            teams.Add(new Team { TeamName = "Rockets" });

            JsonHandeler.SaveJson(teams, "leaderboard.json");
        }

        // ---------- RECORD A GAME ----------
        private void RecordGame(string winnerTeamName, string loserTeamName)
        {
            // Find the winning team
            Team winner = teams.FirstOrDefault(
                t => t.TeamName.Equals(winnerTeamName, StringComparison.OrdinalIgnoreCase)
            );

            // Find the losing team
            Team loser = teams.FirstOrDefault(
                t => t.TeamName.Equals(loserTeamName, StringComparison.OrdinalIgnoreCase)
            );

            // If one or both teams don't exist, show error and exit the function
            if (winner == null || loser == null)
            {
                AnsiConsole.MarkupLine("[red]Error: One or both teams do not exist.[/]");
                return;
            }

            // Increase wins for the winner
            winner.TeamWins++;

            // Increase losses for the loser
            loser.TeamLoses++;

            // Update WinRate for all teams after the change
            RecalculateWinRatio();
        }

        // ---------- CALCULATE W/L FOR EACH TEAM ----------
        private void RecalculateWinRatio()
        {
            foreach (Team team in teams)
            {
                // If the team has no losses → use number of wins as WinRate
                if (team.TeamLoses == 0)
                {
                    team.WinRate = (double)team.TeamWins;
                }
                else
                {
                    // Otherwise calculate wins divided by losses
                    team.WinRate = (double)team.TeamWins / team.TeamLoses;
                }
            }
        }

        // ---------- DISPLAY LEADERBOARD AS TABLE ----------
        private void DisplayLeaderBoard()
        {
            // Sort the teams:
            // 1. Highest W/L first
            // 2. Most wins
            // 3. Fewest losses
            List<Team> sortedTeams = teams
                .OrderByDescending(t => t.WinRate)
                .ThenByDescending(t => t.TeamWins)
                .ThenBy(t => t.TeamLoses)
                .ToList();

            // Create a table
            Table table = new Table
            {
                Border = TableBorder.Rounded // Rounded borders for nicer appearance
            };

            // Add columns to the table
            table.AddColumn(new TableColumn("[grey]Rank[/]").Centered());       // Placement (1st, 2nd, etc.)
            table.AddColumn(new TableColumn("[bold]Team[/]"));                 // Team name
            table.AddColumn(new TableColumn("[green]Wins[/]").Centered());     // Number of wins
            table.AddColumn(new TableColumn("[red]Losses[/]").Centered());     // Number of losses
            table.AddColumn(new TableColumn("[blue]W/L[/]").Centered());       // Win/Loss ratio

            int rank = 1;

            // Add a row for each team
            foreach (Team team in sortedTeams)
            {
                table.AddRow(
                    rank.ToString(),                    // Rank
                    "[white]" + team.TeamName + "[/]",  // Team name
                    "[green]" + team.TeamWins + "[/]",  // Wins
                    "[red]" + team.TeamLoses + "[/]",   // Losses
                    "[blue]" + team.WinRate.ToString("F2") + "[/]" // W/L ratio (2 decimals)
                );

                rank++;
            }

            // Create a panel around the table with a header
            Panel panel = new Panel(table)
                .Header("[bold]Leaderboard[/]") // Header
                .Padding(1, 1, 1, 1);           // Padding around table

            // Render the panel
            AnsiConsole.Write(panel);
        }
    }
}
