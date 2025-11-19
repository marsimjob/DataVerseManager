using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console; // om du vill använda senare
using SixLabors.ImageSharp; // om du vill jobba med bilder senare

namespace DataVerseManager
{
    // Sköter ALLA menyer i programmet
    internal static class MenuSelect
    {
        // Alla lag i spelet
        private static readonly string[] Teams =
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

        // Poäng per lag (totalt, används för Leaderboards)
        private static readonly Dictionary<string, int> TeamPoints = new()
        {
            ["Warriors"] = 0,
            ["Lakers"] = 0,
            ["Knicks"] = 0,
            ["Bulls"] = 0,
            ["Celtics"] = 0,
            ["Heat"] = 0,
            ["Nets"] = 0,
            ["Mavericks"] = 0,
            ["Clippers"] = 0,
            ["Rockets"] = 0
        };

        // Färg + ikon per lag
        private static readonly Dictionary<string, (ConsoleColor Färg, string Ikon)> TeamStyles = new()
        {
            ["Warriors"] = (ConsoleColor.Yellow, "🗡️"),
            ["Lakers"] = (ConsoleColor.Magenta, "⭐"),
            ["Knicks"] = (ConsoleColor.DarkYellow, "🏙️"),
            ["Bulls"] = (ConsoleColor.Red, "🐂"),
            ["Celtics"] = (ConsoleColor.Green, "☘️"),
            ["Heat"] = (ConsoleColor.DarkRed, "🔥"),
            ["Nets"] = (ConsoleColor.Gray, "🕸️"),
            ["Mavericks"] = (ConsoleColor.Cyan, "🐎"),
            ["Clippers"] = (ConsoleColor.Blue, "⚓"),
            ["Rockets"] = (ConsoleColor.DarkRed, "🚀")
        };

        // Tema-lag som styr färg + ikon i menyn
        private static string CurrentThemeTeam = "Warriors";

        // Hemma- och bortalag (används för match)
        private static string HomeTeam = "Warriors";
        private static string AwayTeam = "Lakers";

        // Random-generator (för simulering)
        private static readonly Random RNG = new Random();

        // Hjälp: hämta tema-färg
        private static ConsoleColor GetThemeColor()
        {
            if (TeamStyles.TryGetValue(CurrentThemeTeam, out var style))
                return style.Färg;

            return ConsoleColor.White;
        }

        // Hjälp: hämta tema-ikon
        private static string GetThemeIcon()
        {
            if (TeamStyles.TryGetValue(CurrentThemeTeam, out var style))
                return style.Ikon;

            return "🏀";
        }

        // Skriv text i en viss färg
        private static void WriteColored(string text, ConsoleColor color, bool newLine = true)
        {
            var original = Console.ForegroundColor;
            Console.ForegroundColor = color;

            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);

            Console.ForegroundColor = original;
        }

        // Skriver ut ett lagnamn med dess temafärg och ikon
        private static void WriteTeamName(string? team, bool newLine = true)
        {
            if (string.IsNullOrWhiteSpace(team))
            {
                if (newLine) Console.WriteLine("(okänt lag)");
                else Console.Write("(okänt lag)");
                return;
            }

            var original = Console.ForegroundColor;

            if (TeamStyles.TryGetValue(team, out var style))
            {
                Console.ForegroundColor = style.Färg;
                if (newLine)
                    Console.WriteLine($"{style.Ikon} {team}");
                else
                    Console.Write($"{style.Ikon} {team}");
            }
            else
            {
                if (newLine)
                    Console.WriteLine(team);
                else
                    Console.Write(team);
            }

            Console.ForegroundColor = original;
        }

        // Header högst upp – använder tema-lagets färg och ikon
        private static void RenderHeader()
        {
            Console.Clear();

            var themeColor = GetThemeColor();
            var icon = GetThemeIcon();

            Console.ForegroundColor = themeColor;
            Console.WriteLine("======================================");
            Console.WriteLine($" {icon} NBA MANAGER {icon}");
            Console.WriteLine("======================================");
            Console.ResetColor();
            Console.WriteLine();

            WriteColored($"Tema-lag : {CurrentThemeTeam}", themeColor);
            WriteColored($"Hemma lag: {HomeTeam}", ConsoleColor.Green);
            WriteColored($"Borta lag: {AwayTeam}", ConsoleColor.Red);
            Console.WriteLine();
        }

        // 🔹 HUVUDMENY med PIL (↑/↓ + Enter)
        public static void ShowMainMenu()
        {
            string[] menuItems =
            {
               "Watch a Match",
              "Look at Match Board",
              "Look at Leaderboards",
                 "Team Manager",
                 "Theme Options",
                "Choose Home Team",
                "Choose Away Team",
                  "Exit"
};

            int selectedIndex = 0;

            while (true)
            {
                RenderHeader();

                Console.WriteLine("Använd ↑ och ↓ för att välja, Enter för att bekräfta.\n");

                var themeColor = GetThemeColor();

                // Rita ut menyn med pil
                for (int i = 0; i < menuItems.Length; i++)
                {
                    bool isSelected = (i == selectedIndex);

                    if (isSelected)
                    {
                        Console.BackgroundColor = themeColor;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(" ");
                        Console.ForegroundColor = themeColor;
                    }

                    Console.WriteLine(menuItems[i]);
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = menuItems.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= menuItems.Length) selectedIndex = 0;
                }
                else if (key == ConsoleKey.Enter)
                {
                    switch (selectedIndex)
                    {
                        case 0:
                            WatchMatch();
                            break;
                        case 1:
                            ShowMatchBoard();
                            break;
                        case 2:
                            ShowLeaderboards();
                            break;
                        case 3:
                            TeamManagerMenu();
                            break;
                        case 4:
                            ThemeOptions();
                            break;
                        case 5:
                            ChooseHomeTeam();
                            break;
                        case 6:
                            ChooseAwayTeam();
                            break;
                        case 7:
                            WriteColored("\nProgrammet avslutas. Hej då! 🌿", ConsoleColor.Magenta);
                            return;
                    }
                }
            }
        }

        // 🔹 Menyval 1 – Watch a Match (match + manuell + simulering)
        private static void WatchMatch()
        {
            Console.Clear();
            WriteColored("🏀 WATCH A MATCH 🏀", GetThemeColor());
            Console.WriteLine();

            WriteColored("Du spelar en match mellan:", ConsoleColor.White);
            Console.Write("Hemma: ");
            WriteTeamName(HomeTeam, false);
            Console.WriteLine();
            Console.Write("Borta: ");
            WriteTeamName(AwayTeam, false);
            Console.WriteLine("\n");

            WriteColored("Tryck Enter för att starta matchen...", ConsoleColor.Cyan);
            Console.ReadLine();

            RunMatch(HomeTeam, AwayTeam);
        }

        // Själva match-loopen
        private static void RunMatch(string homeTeam, string awayTeam)
        {
            int homeScore = 0;
            int awayScore = 0;
            bool matchOver = false;

            while (!matchOver)
            {
                Console.Clear();
                WriteColored("🏀 LIVE MATCH 🏀", GetThemeColor());
                Console.WriteLine();

                Console.Write("Hemma: ");
                WriteTeamName(homeTeam, false);
                Console.WriteLine($" ({homeScore} poäng)");

                Console.Write("Borta: ");
                WriteTeamName(awayTeam, false);
                Console.WriteLine($" ({awayScore} poäng)");

                Console.WriteLine();
                WriteColored("Välj ett alternativ:", ConsoleColor.Yellow);
                Console.WriteLine("1. +2 poäng till hemmalag");
                Console.WriteLine("2. +3 poäng till hemmalag");
                Console.WriteLine("3. +2 poäng till bortalag");
                Console.WriteLine("4. +3 poäng till bortalag");
                Console.WriteLine("5. Visa scoreboard");
                Console.WriteLine("6. Simulate rest of match");
                Console.WriteLine("0. Avsluta matchen");
                Console.WriteLine();
                WriteColored("Ditt val: ", ConsoleColor.Cyan, false);

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        homeScore += 2;
                        break;
                    case "2":
                        homeScore += 3;
                        break;
                    case "3":
                        awayScore += 2;
                        break;
                    case "4":
                        awayScore += 3;
                        break;
                    case "5":
                        ShowScoreboard(homeTeam, awayTeam, homeScore, awayScore);
                        break;
                    case "6":
                        SimulateRestOfMatch(ref homeScore, ref awayScore);
                        ShowScoreboard(homeTeam, awayTeam, homeScore, awayScore, isFinal: true);
                        matchOver = true;
                        break;
                    case "0":
                        matchOver = true;
                        break;
                    default:
                        WriteColored("Ogiltigt val. Tryck Enter.", ConsoleColor.Red);
                        Console.ReadLine();
                        break;
                }
            }

            // När matchen är slut – spara resultatet i TeamPoints
            TeamPoints[homeTeam] += homeScore;
            TeamPoints[awayTeam] += awayScore;

            Console.WriteLine();
            WriteColored("Matchen är slut! Resultatet har sparats till leaderboards.", ConsoleColor.Green);
            WriteColored("Tryck Enter för att återgå till huvudmenyn.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        private static void ShowScoreboard(string homeTeam, string awayTeam, int homeScore, int awayScore, bool isFinal = false)
        {
            Console.Clear();
            WriteColored(isFinal ? "🏁 FINAL SCORE 🏁" : "📊 SCOREBOARD 📊", GetThemeColor());
            Console.WriteLine();

            Console.Write("Hemma: ");
            WriteTeamName(homeTeam, false);
            Console.WriteLine($" → {homeScore} poäng");

            Console.Write("Borta: ");
            WriteTeamName(awayTeam, false);
            Console.WriteLine($" → {awayScore} poäng");

            Console.WriteLine();

            if (isFinal)
            {
                if (homeScore > awayScore)
                    WriteColored($"{homeTeam} vann matchen! 🎉", ConsoleColor.Green);
                else if (awayScore > homeScore)
                    WriteColored($"{awayTeam} vann matchen! 🎉", ConsoleColor.Green);
                else
                    WriteColored("Matchen slutade oavgjort.", ConsoleColor.Yellow);
            }

            WriteColored("Tryck Enter för att fortsätta.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        // Enkel simulering – lägger till slumpade poäng
        private static void SimulateRestOfMatch(ref int homeScore, ref int awayScore)
        {
            int extraHome = RNG.Next(5, 31); // 5–30 poäng
            int extraAway = RNG.Next(5, 31);

            homeScore += extraHome;
            awayScore += extraAway;
        }

        // 🔹 Menyval 2 – Match Board (visar sparade TeamPoints)
        private static void ShowMatchBoard()
        {
            Console.Clear();
            WriteColored("🌿 MATCH BOARD 🌿", GetThemeColor());
            Console.WriteLine();

            int homeScore = TeamPoints[HomeTeam];
            int awayScore = TeamPoints[AwayTeam];

            WriteColored("Aktuell säsongsställning mellan:", ConsoleColor.White);
            Console.Write("Hemma: ");
            WriteTeamName(HomeTeam, false);
            Console.WriteLine($" – {homeScore} poäng");

            Console.Write("Borta: ");
            WriteTeamName(AwayTeam, false);
            Console.WriteLine($" – {awayScore} poäng");

            Console.WriteLine();
            WriteColored("Tryck Enter för att gå tillbaka.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        // 🔹 Menyval 3 – Leaderboards
        private static void ShowLeaderboards()
        {
            Console.Clear();
            WriteColored("🌿 LEADERBOARDS 🌿", GetThemeColor());
            Console.WriteLine();

            var sorted = TeamPoints
            .OrderByDescending(t => t.Value)
            .ToList();

            int rank = 1;
            foreach (var team in sorted)
            {
                WriteColored($"{rank}. ", ConsoleColor.White, false);
                WriteTeamName(team.Key, false);
                Console.WriteLine($" – {team.Value} poäng");
                rank++;
            }

            Console.WriteLine();
            WriteColored("Tryck Enter för att gå tillbaka.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        // 🔹 Menyval 4 – Team Manager
        private static void TeamManagerMenu()
        {
            while (true)
            {
                Console.Clear();
                WriteColored("🌿 TEAM MANAGER 🌿", GetThemeColor());
                Console.WriteLine();

                WriteColored("1. Visa alla lag och deras poäng", ConsoleColor.Yellow);
                WriteColored("2. Lägg till poäng till ett lag", ConsoleColor.Yellow);
                WriteColored("0. Tillbaka till huvudmenyn", ConsoleColor.Yellow);
                Console.WriteLine();
                WriteColored("Välj ett alternativ: ", ConsoleColor.Cyan, false);

                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    WriteColored("Ogiltigt val. Tryck Enter.", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }

                switch (choice)
                {
                    case 0:
                        return;

                    case 1:
                        ShowAllTeamsWithPoints();
                        break;

                    case 2:
                        AddPointsToTeam();
                        break;

                    default:
                        WriteColored("Ogiltigt val. Tryck Enter.", ConsoleColor.Red);
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void ShowAllTeamsWithPoints()
        {
            Console.Clear();
            WriteColored("🌿 ALLA LAG OCH POÄNG 🌿", GetThemeColor());
            Console.WriteLine();

            foreach (var team in TeamPoints)
            {
                WriteTeamName(team.Key, false);
                Console.WriteLine($": {team.Value} poäng");
            }

            Console.WriteLine();
            WriteColored("Tryck Enter för att gå tillbaka.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        private static void AddPointsToTeam()
        {
            Console.Clear();
            WriteColored("🌿 LÄGG TILL POÄNG TILL ETT LAG 🌿", GetThemeColor());
            Console.WriteLine();

            for (int i = 0; i < Teams.Length; i++)
            {
                Console.Write($"{i + 1}. ");
                WriteTeamName(Teams[i], false);
                Console.WriteLine($" (nuvarande poäng: {TeamPoints[Teams[i]]})");
            }

            Console.WriteLine();
            WriteColored("Skriv numret på laget: ", ConsoleColor.Cyan, false);
            string inputTeam = Console.ReadLine();

            if (!int.TryParse(inputTeam, out int teamChoice) ||
            teamChoice < 1 || teamChoice > Teams.Length)
            {
                WriteColored("Ogiltigt val. Tryck Enter.", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            string selectedTeam = Teams[teamChoice - 1];

            WriteColored($"Hur många poäng vill du lägga till för {selectedTeam}? ", ConsoleColor.Cyan, false);
            string inputPoints = Console.ReadLine();

            if (!int.TryParse(inputPoints, out int pointsToAdd))
            {
                WriteColored("Du skrev inte en siffra. Tryck Enter.", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            TeamPoints[selectedTeam] += pointsToAdd;

            Console.WriteLine();
            WriteColored($"{pointsToAdd} poäng har lagts till för {selectedTeam}.", ConsoleColor.Green);
            WriteColored($"Nytt total: {TeamPoints[selectedTeam]} poäng.", ConsoleColor.Green);
            Console.WriteLine();
            WriteColored("Tryck Enter för att gå tillbaka.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        // 🔹 Menyval 5 – Theme Options
        private static void ThemeOptions()
        {
            Console.Clear();
            WriteColored("🌿 THEME OPTIONS 🌿", GetThemeColor());
            Console.WriteLine();
            WriteColored("Välj ett lag som tema:\n", ConsoleColor.White);

            for (int i = 0; i < Teams.Length; i++)
            {
                Console.Write($"{i + 1}. ");
                WriteTeamName(Teams[i]);
            }

            Console.WriteLine();
            WriteColored("Skriv numret på laget: ", ConsoleColor.Cyan, false);
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice) ||
            choice < 1 || choice > Teams.Length)
            {
                WriteColored("Ogiltigt val. Tryck Enter.", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            CurrentThemeTeam = Teams[choice - 1];

            Console.WriteLine();
            WriteColored($"Tema-laget är nu: {CurrentThemeTeam}", ConsoleColor.Green);
            WriteColored("Tryck Enter för att gå tillbaka.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        // 🔹 Menyval 6 – Choose Home Team
        private static void ChooseHomeTeam()
        {
            Console.Clear();
            WriteColored("🌿 CHOOSE HOME TEAM 🌿", GetThemeColor());
            Console.WriteLine();
            WriteColored("Välj hemmalag:\n", ConsoleColor.White);

            for (int i = 0; i < Teams.Length; i++)
            {
                Console.Write($"{i + 1}. ");
                WriteTeamName(Teams[i]);
            }

            Console.WriteLine();
            WriteColored("Skriv numret på laget: ", ConsoleColor.Cyan, false);
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice) ||
            choice < 1 || choice > Teams.Length)
            {
                WriteColored("Ogiltigt val. Tryck Enter.", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            HomeTeam = Teams[choice - 1];

            Console.WriteLine();
            WriteColored($"Hemma-laget är nu: {HomeTeam}", ConsoleColor.Green);
            WriteColored("Tryck Enter för att gå tillbaka.", ConsoleColor.Cyan);
            Console.ReadLine();
        }

        // 🔹 Menyval 7 – Choose Away Team
        private static void ChooseAwayTeam()
        {
            Console.Clear();
            WriteColored("🌿 CHOOSE AWAY TEAM 🌿", GetThemeColor());
            Console.WriteLine();
            WriteColored("Välj bortalag:\n", ConsoleColor.White);

            for (int i = 0; i < Teams.Length; i++)
            {
                Console.Write($"{i + 1}. ");
                WriteTeamName(Teams[i]);
            }

            Console.WriteLine();
            WriteColored("Skriv numret på laget: ", ConsoleColor.Cyan, false);
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice) ||
            choice < 1 || choice > Teams.Length)
            {
                WriteColored("Ogiltigt val. Tryck Enter.", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            AwayTeam = Teams[choice - 1];

            Console.WriteLine();
            WriteColored($"Borta-laget är nu: {AwayTeam}", ConsoleColor.Green);
            WriteColored("Tryck Enter för att gå tillbaka.", ConsoleColor.Cyan);
            Console.ReadLine();
        }
    }
}
