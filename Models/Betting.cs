using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataVerseManager.Models.Leaderboard;

namespace DataVerseManager.Models
{
    public class Betting

    {
        // Attributes
        public double Odds { get; set; }
        public string Team { get; set; }
        public double BetAmount { get; set; }
        public string Power { get; set; }

        // Lista med lag
        public List<Team> Teams { get; set; } = new List<Team>();

        // Constructor
        public Betting()
        {

        }

        // ---------- SKAPA STANDARDLAG ----------
        public void CreateTeam()
        {
            Teams.Add(new Team { TeamName = "Warriors" });
            Teams.Add(new Team { TeamName = "Lakers" });
            Teams.Add(new Team { TeamName = "Knicks" });
            Teams.Add(new Team { TeamName = "Bulls" });
            Teams.Add(new Team { TeamName = "Celtics" });
            Teams.Add(new Team { TeamName = "Heat" });
            Teams.Add(new Team { TeamName = "Nets" });
            Teams.Add(new Team { TeamName = "Mavericks" });
            Teams.Add(new Team { TeamName = "Clippers" });
            Teams.Add(new Team { TeamName = "Rockets" });
        }




        // ------------ BETTING FUNCTIONALITY ----------

        // Returnerar decimalodds för två lag, baserat på deras aktuella Team.Power.
        // "margin" är spelbolagets vinstpåslag (t.ex. 0.06 = 6%).
        // Om margin = 0 får man "rättvisa" odds (utan vinstpåslag).

        public static (double teamAOdds, double teamBOdds) GetOdds(Team a, Team b, double margin = 0.0)
        {
            // Säkerhetskontroll: om något av lagen saknas kastas ett felmeddelande.
            if (a == null || b == null)
                throw new ArgumentNullException("Team kan inte vara null.");

            // Säkerhetskontroll: om någon power är negativ kastas ett felmeddelande.
            if (a.WinRate < 0 || b.WinRate < 0)
                throw new ArgumentException("Power kan inte vara negativ.");

            // Skyddar mot att någon power är exakt 0 (för att undvika division med noll)
            const double eps = 1e-9;

            // Tar lag A:s och B:s power, men minst eps (för att aldrig bli 0)
            double pA = Math.Max(a.WinRate, eps);

            double pB = Math.Max(b.WinRate, eps);

            // Summerar båda lagens power 
            double total = pA + pB;

            // Beräknar sannolikheten att lag A och B vinner: deras power / total power
            double probA = pA / total;

            double probB = pB / total;

            // "overround" är spelbolagets vinstmarginal.
            // Om margin = 0 betyder det inga extra påslag.
            // Clamp01 ser till att margin hålls mellan 0 och 1.
            double overround = 1.0 + Clamp01(margin);

            // Räkna ut oddset för lag 
            // Oddset = 1 / (sannolikhet * överround)
            double oddsA = 1.0 / (probA * overround);

            double oddsB = 1.0 / (probB * overround);

            // Returnerar båda oddsen avrundade till 2 decimaler (t.ex. 1.53, 2.45)
            return (Round2(oddsA), Round2(oddsB));
        }

        private static double Clamp01(double value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }

        // Avrundar ett tal till 2 decimaler (används för oddsen)
        private static double Round2(double value)
        {
            return Math.Round(value, 2);
        }

        public static void PrintMoney(double money)
        {
            Console.WriteLine("Now printing money: " + money);
        }
        public static void PlaceBet(Team team1, Team team2, Coach better)
        {
            // Don't let player bet if they have too little money
            if(better.UserWallet.ReturnWalletBalance() <= 0)
            {
                AnsiConsole.WriteLine("You don't have any cash to bet with! Maybe you should look into your gambling addiction...");
                Console.ReadLine();
                return;
            }

            ShowBettingTable(team1, team2);

            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.ShowHeaders = true;
            table.AddColumn($"[yellow]Wallet Balance: [/]");
            table.AddColumn($"[yellow]{better.UserWallet.ReturnWalletBalance()}[/]");
 
            var panel = new Panel(table)
                .Header($"[white]{better.CoachName}[/]", Justify.Center)
                .Border(BoxBorder.Rounded)
                .Padding(1, 1, 1, 1);

            AnsiConsole.Write(panel);

            var (team1Odds, team2Odds) = GetOdds(team1, team2);

            // User Wallet Balance
            double walletBalance = better.UserWallet.ReturnWalletBalance();

            // Fråga om insats med Spectre.Console
            double betAmount = AnsiConsole.Ask<double>("[green]Enter your bet amount ($):[/]");

            // Can only bet if bet amount is less than you wallet balance
            if (walletBalance >= betAmount)
            {
                // Remove money that has been bet from the wallet
                better.UserWallet.GetMoney(-betAmount);

                // Låt användaren välja lag via meny i stället för att skriva
                string teamName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Which team do you want to bet on?[/]")
                        .AddChoices(team1.TeamName, team2.TeamName)
                );

                double odds = 0;

                if (teamName == team1.TeamName)
                {
                    odds = team1Odds;
                }
                else if (teamName == team2.TeamName)
                {
                    odds = team2Odds;
                }

                AnsiConsole.MarkupLine($"\nPlacing a bet of [green]${betAmount}[/] on [blue]{teamName}[/]...");
             
                Team Winningteam = Match.SimulateMatch(team1, team2);

                if (Winningteam.TeamName == teamName)
                {
                    double potentialWin = betAmount * odds;
                    AnsiConsole.MarkupLine(
                        $"[bold green]You won![/] If {teamName} wins, you get [bold]${Math.Round(potentialWin, 2)}[/]"
                    );
                    // Add to wallet
                    better.UserWallet.GetMoney(potentialWin);
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Sorry,[/] {teamName} lost the match.");
                }
            }
            else
            {
                Console.WriteLine("You dont have enough money to bet this much!");
                return;
            }
        }

        public static void ShowBettingTable(Team teamA, Team teamB, double margin = 0.06)
        {
            var (oddsA, oddsB) = GetOdds(teamA, teamB);

            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.ShowHeaders = true;


            table.AddColumn($"[green]{teamA.TeamName} vs {teamB.TeamName}[/]");
            table.AddColumn("[green]1[/]");
            table.AddColumn("[green]2[/]");


            table.AddRow(
                "[green]Odds[/]",
                $"[yellow]{oddsA:0.00}[/]",
                $"[yellow]{oddsB:0.00}[/]"
            );

            var panel = new Panel(table)
                .Header("[bold green]LIVE MATCH[/]", Justify.Center)
                .Border(BoxBorder.Rounded)
                .Padding(1, 1, 1, 1);

            AnsiConsole.Write(panel);
        }
    }
}
