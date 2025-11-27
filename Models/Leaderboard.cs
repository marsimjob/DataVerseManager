using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataVerseManager.Models
{
    public static class Leaderboard
    {
        // A list that holds all teams in the leaderboard
        private static List<LeaderBoardStat> teams = JsonHandeler.LoadJson<List<LeaderBoardStat>>("leaderboard.json");

        private static void CreateDefaultTeams()
        {
            // Create 10 teams with 0 wins and 0 losses
            teams.Add(new LeaderBoardStat { TeamName = "Warriors" });
            teams.Add(new LeaderBoardStat { TeamName = "Lakers" });
            teams.Add(new LeaderBoardStat { TeamName = "Knicks" });
            teams.Add(new LeaderBoardStat { TeamName = "Bulls" });
            teams.Add(new LeaderBoardStat { TeamName = "Celtics" });
            teams.Add(new LeaderBoardStat { TeamName = "Heat" });
            teams.Add(new LeaderBoardStat { TeamName = "Nets" });
            teams.Add(new LeaderBoardStat { TeamName = "Mavericks" });
            teams.Add(new LeaderBoardStat { TeamName = "Clippers" });
            teams.Add(new LeaderBoardStat { TeamName = "Rockets" });

            JsonHandeler.SaveJson(teams, "leaderboard.json");
        }

        public static void RecordGame(string winnerTeamName, string loserTeamName)
        {
            // Find the winning team
            Team winner = MatchGenerator.AllTeams.FirstOrDefault(
                t => t.TeamName.Equals(winnerTeamName, StringComparison.OrdinalIgnoreCase)
            );

            // Find the losing team
            Team loser = MatchGenerator.AllTeams.FirstOrDefault(
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
            winner.UpdateTeamWinRate();
            loser.UpdateTeamWinRate();

            LeaderBoardStat winnerTeam = new LeaderBoardStat();
            LeaderBoardStat loserTeam = new LeaderBoardStat();

            winnerTeam.TeamName = winner.TeamName;
            winnerTeam.TeamWins = winner.TeamWins;
            winnerTeam.TeamLoses = winner.TeamLoses;

            double winnerTotalmatches = (double)(winner.TeamLoses + winner.TeamWins);
            winnerTeam.WinRate = (winner.TeamWins / winnerTotalmatches) * 100;

            loserTeam.TeamName = loser.TeamName;
            loserTeam.TeamWins = loser.TeamWins;
            loserTeam.TeamLoses = loser.TeamLoses;

            double loserTotalmatches = (double)(loser.TeamWins + loser.TeamLoses);
            loserTeam.WinRate =  (loser.TeamWins / loserTotalmatches) * 100;

            // Remove teams with same name
            if(teams.Any(team => team.TeamName.Equals(winnerTeam.TeamName)))
            { 
                teams.Remove(teams.Find(team => team.TeamName.Equals(winnerTeam.TeamName)));
            }
            if (teams.Any(team => team.TeamName.Equals(loserTeam.TeamName)))
            {
                teams.Remove(teams.Find(team => team.TeamName.Equals(loserTeam.TeamName)));
            }
            // Then add
            teams.Add( winnerTeam );
            teams.Add( loserTeam );

            JsonHandeler.SaveJson(teams, "leaderboard.json");
        }

        // ---------- DISPLAY LEADERBOARD AS TABLE ----------
        public static void DisplayLeaderBoard()
        {
            // Sort the teams:
            // 1. Highest W/L first
            // 2. Most wins
            // 3. Fewest losses
            List<LeaderBoardStat> sortedTeams = teams
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
            table.AddColumn(new TableColumn("[blue]Win Rate[/]").Centered());       // Win/Loss ratio

            int rank = 1;

            // Add a row for each team
            foreach (LeaderBoardStat team in sortedTeams)
            {
                table.AddRow(
                    rank.ToString(),                    // Rank
                    "[white]" + team.TeamName + "[/]",  // Team name
                    "[green]" + team.TeamWins + "[/]",  // Wins
                    "[red]" + team.TeamLoses + "[/]",   // Losses
                    "[blue]" + team.WinRate.ToString("F1") +"% [/]" // W/L ratio (2 decimals)
                );

                rank++;
            }

            // Create a panel around the table with a header
            Panel panel = new Panel(table)
                .Header("[bold]Leaderboard[/]") // Header
                .Padding(1, 1, 1, 1);           // Padding around table

            // Render the panel
            AnsiConsole.Write(panel);

            Console.ReadLine();
        }
    }
}
