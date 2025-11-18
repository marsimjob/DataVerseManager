using Spectre.Console;
using Spectre.Console.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVerseManager.Services;

namespace DataVerseManager.Models
{
    static class MatchSimulator
    {

        // For randomization over the match objects life time
        private static Random rng = new Random();

        public static Team currentWinner = new Team();

        // Dialogue for matches (Stock up on random things to say that are positive for the commentators)
        // KICK-OFF
        static Dictionary<string, List<string>> kickOffLines = new Dictionary<string, List<string>>()
{
    {
        "High", new List<string>()
        {
            "The arena falls silent as tension builds up...and the game is on!",
            "The whistle blows! The clash of titans begins now!",
            "The battle begins-- this is it! Who will rise and who will fall?"
        }
    },
    {
        "Mid", new List<string>()
        {
            "The game is underway-- both teams looking to set the tone right from the start.",
            "The match begins, let's see how they open this match!",
            "Tip-off is complete, this first quarter will be crucial for momentum."
        }
    },
    {
        "Low", new List<string>()
        {
            "And we're rolling! Let's hope nobody forgot their warm-up today!",
            "Did someone call the fire department? These teams are on fire tonight!",
            "I hope both teams had their cup of coffee this morning! Game on!"
        }
    },
    {
        "Imbalance", new List<string>()
        {
            "Hold up—what kind of match is this? One team is clearly outmatched!",
            "Am I dreaming or are we being played? This match-up is all wrong but the game must go on!",
            "Will this be the chance for the little guys to glow, or will the big leaguers eat them for breakfast?"
        }
    }
};
        // MID-GAME
        static string[] offensive =
        {
        $" pushes forward aggressively!",
        $" showing excellent teamwork!",
        $" dominates the midfield!",
        $" is overwhelming the opponent!"
        };

        static string[] defensive =
        {
        $" launches a counterattack!",
        $" holding their ground!",
        $" are showing a burst of energy!",
        $" trying to regain control!"
        };

        static string[] fumbles =
        {
        "-- sloppy mistake! That could cost them!",
        ", oh no! terrible misplay—- the crowd gasps!",
        "! A fumble! The audience can't believe it!",
        " completely missed that opportunity!"
        };


        public static Team SimulateMatch(Team teamA, Team teamB)
        {
            // Throw an error if any of the teams are invalid somehow
            if (teamA == null || teamB == null)
            {
                throw new InvalidOperationException("Match cannot be simulated with missing teams!");
            }

            // This protects for 0 divisions, it makes it so that it is impossible for a an outcome
            // where if any of the team has 0 win rate state the equation will crash the program
            const double eps = 1e-9;
            double pA = Math.Max(teamA.WinRate, eps);
            double pB = Math.Max(teamB.WinRate, eps);

            // Probability for teamA winning:
            double total = pA + pB;
            double probabilityA = pA / total;

            // Get random probability
            double randomValue = rng.NextDouble();

            if (randomValue < probabilityA)
            {
                // Small stat boosts for winning team
                foreach (var player in teamA.TeamPlayer)
                {
                    Random rng = new Random();

                    // Choose a random stat (0 = Speed, 1 = Power, 2 = Accuracy, 3 = Defending)
                    int statToBoost = rng.Next(0, 4); // 0,1,2,3

                    double boostAmount = rng.NextDouble() * 2.0; // small boost

                    switch (statToBoost)
                    {
                        case 0:
                            player.Speed += boostAmount;
                            break;
                        case 1:
                            player.Power += boostAmount;
                            break;
                        case 2:
                            player.Accuracy += boostAmount;
                            break;
                        case 3:
                            player.Defending += boostAmount;
                            break;
                    }
                }
                return teamA;
            }
            else
            { 
                // Opposite for losing team
                foreach (var player in teamA.TeamPlayer)
                {
                    Random rng = new Random();

                    // Choose a random stat (0 = Speed, 1 = Power, 2 = Accuracy, 3 = Defending)
                    int statToBoost = rng.Next(0, 4); // 0,1,2,3

                    double boostAmount = rng.NextDouble() * 2.0; // small decrease

                    switch (statToBoost)
                    {
                        case 0:
                            player.Speed -= boostAmount;
                            break;
                        case 1:
                            player.Power -= boostAmount;
                            break;
                        case 2:
                            player.Accuracy -= boostAmount;
                            break;
                        case 3:
                            player.Defending -= boostAmount;
                            break;
                    }
                }
                return teamB;
            }
        }
        public static void RunVisualMatch(Team teamA, Team teamB)
        {
            // Get a match time total so we can start a game clock that ticks down
            int matchDuration = 10;
            // Use remaining time to gauge the time when ticking down on each update
            int remainingTime = matchDuration;

            string currentMessage = GetIntroLine(teamA, teamB);

            // Use our SimulateMatch() function to get the winning team in advance
            // the idea of this method it's all just for show and pretend to play a match
            // despite knowing the outcome in advance!
            // Get winner
            Team winner = SimulateMatch(teamA, teamB);
            // Set winner to our static for reference
            currentWinner = winner;
            // Get Loser
            Team loser;
            if (winner == teamA)
            {
                loser = teamB;
            }
            else
            {
                loser = teamA;
            }

            // Set scores to zero
            int OneScore = 0;
            int TwoScore = 0;

            // Prep court layouts
            int courtHeight = 18;
            int courtLength = 113;
            var courts = GenerateRandomCourts(courtHeight, courtLength, count: 20);

            AnsiConsole.Live(new Panel(" "))  // initial placeholder
                .Start(ctx =>
                {
                    // Create layout once
                    var layout = new Layout("root")
                        .SplitRows(
                            new Layout("scoreboard").Size(8),
                            new Layout("court").Size(courtHeight),
                            new Layout("commentary")
                        );

                    while (remainingTime >= 0)
                    {
                        // Scoreboard Table
                        var table = new Table()
                            .Border(TableBorder.Rounded)
                            .Title($"[bold red]Match: {teamA.TeamName} vs {teamB.TeamName}[/]")
                            .Centered();

                        table.AddColumn(new TableColumn($"[bold][white]HOME[/]\n{teamA.TeamName}[/]").Centered());
                        table.AddColumn(new TableColumn("[bold]TIME[/]").Centered());
                        table.AddColumn(new TableColumn($"[bold][white]VISITOR[/]\n{teamB.TeamName}[/]").Centered());
                        table.AddRow(
                            $"[green]Score[/]: [bold red]{OneScore}[/]",
                            $"Remaining: [bold yellow]{remainingTime}[/]",
                            $"[green]Score[/]: [bold red]{TwoScore}[/]"
                        );

                        // Court View 
                        // Pick a random / changing court layout
                        var courtIndex = remainingTime % courts.Count;
                        var courtRows = courts[courtIndex];

                        // Style each row: wrap the spaces + emojis in markup with orange background
                        var styledRows = courtRows
                            .Select(row => $"[white on orange1]{row}[/]")
                            .ToArray();
                        var content = string.Join(Environment.NewLine, styledRows);

                        // Create a panel using that styled content
                        var courtPanel = new Panel(content)
                            .Padding(1, 1)
                            .Border(BoxBorder.Rounded)
                            .Header("[bold red]Court View[/]")
                            .Expand();

                        // Adjusting clipping here so it doesnt go over scoreboard table
                        var court = new Align(courtPanel, HorizontalAlignment.Center, VerticalAlignment.Top)
                            .Height(courtHeight);

                        // Commentary Pannel
                        var messagePanel = new Panel(currentMessage)
                            .Border(BoxBorder.Rounded)
                            .Header("[bold yellow]Commentator[/]")
                            .Expand();

                        // Update layout parts

                        layout["court"].Update(court);
                        layout["scoreboard"].Update(table);
                        layout["commentary"].Update(messagePanel);

                        // Update the live target
                        ctx.UpdateTarget(layout);
                        ctx.Refresh();

                        // Pause before next update and ticking down time
                        Thread.Sleep(1000);
                        remainingTime--;

                        // Finish message
                        if (remainingTime < 1)
                        {
                            currentMessage = $"[bold green]Game Set! {winner.TeamName} Win![/]";
                        }
                        else
                        {
                            // Every 5 seconds you get some commentary from the game
                            if (remainingTime % 5 == 0 && remainingTime != matchDuration)
                            {
                                // Roll for what team's progress to be shown
                                int roll = rng.Next(0, 10);

                                // 50% chance for a message for either team's performance
                                if (roll < 5)
                                {
                                    // The winning team has higher roles of success but they can fumble
                                    int roll2 = rng.Next(0, 10);

                                    // Low chance to fumble for the winning team, only 20%
                                    if (roll2 < 2)
                                        currentMessage = $"{winner.TeamName}{fumbles[rng.Next(fumbles.Length)]}";
                                    // 40% chance of some offensive performance
                                    else if (roll2 < 6)
                                        currentMessage = $"{winner.TeamName}{offensive[rng.Next(offensive.Length)]}";
                                    // Defensive perforamance message is 40%
                                    else
                                        currentMessage = $"{winner.TeamName}{defensive[rng.Next(defensive.Length)]}";
                                }
                                // 50% chance for the losing team
                                else
                                {
                                    // Losing team has a different spread of chances, higher to fumble and less offensively
                                    int roll2 = rng.Next(0, 10);

                                    // Higher rate of fumbling when a team is losing-- 40%
                                    if (roll2 < 4)
                                        currentMessage = $"{loser.TeamName}{fumbles[rng.Next(fumbles.Length)]}";
                                    // 20% chance of some offensive performance
                                    else if (roll2 < 6)
                                        currentMessage = $"{loser.TeamName}{offensive[rng.Next(offensive.Length)]}";
                                    // Defensive perforamance message is 40%
                                    else
                                        currentMessage = $"{loser.TeamName}{defensive[rng.Next(defensive.Length)]}";
                                }
                            }
                            // Commentary every 2 seconds, the winner team has higher percentage chance to get score 
                            // every other second
                            if (remainingTime % 2 == 0 && remainingTime != matchDuration)
                            {
                                // Winner gets higher probability
                                bool winnerScores = rng.NextDouble() < 0.70;  // 70% chance
                                bool loserScores = rng.NextDouble() < 0.20;  // 20% chance

                                if (winnerScores)
                                {
                                    if (winner == teamA) OneScore += rng.Next(1, 3); // +1 or +2
                                    else TwoScore += rng.Next(1, 3);
                                }

                                if (loserScores)
                                {
                                    if (winner == teamA) TwoScore += rng.Next(1, 3);
                                    else OneScore += rng.Next(1, 3);
                                }
                            }
                        }
                    }
                    Match thisMatch = new Match(DateTime.Now, teamA, teamB, OneScore, TwoScore, Matchboard.Matchboards.Count);
                    Matchboard.Matchboards.Add(thisMatch);
                    JsonHandeler.SaveJson<List<Match>>(Matchboard.Matchboards, "matchboards.json");
                });

        }
        private static string GetIntroLine(Team teamA, Team teamB)
        {
            string introComment = "";

            // Which lines should be use is decided by treshholds of teams power levels
            List<string> chosenLines = new List<string>();
            // Level key to access dictionary in DialogueContainer
            string levelKey = "";
            // Make treshholds
            if     // Too strong vs Too Weak Matches
               ((teamA.WinRate >= 50 && teamB.WinRate <= 35) ||
               ((teamA.WinRate <= 35 && teamB.WinRate >= 50)) ||
               (teamA.WinRate >= 70 && teamB.WinRate <= 50) ||
               ((teamA.WinRate <= 50 && teamB.WinRate >= 70)))
                levelKey = "Imbalance";
            else if // Top tier team matches
                (teamA.WinRate >= 70 && teamB.WinRate >= 70)
                levelKey = "High";
            else if // Mid tier team matches
                ((teamA.WinRate >= 70 && teamB.WinRate <= 70) ||
                (teamA.WinRate <= 70 && teamB.WinRate >= 70) ||
                (teamA.WinRate >= 35 && teamB.WinRate >= 35))
                levelKey = "Mid";
            else   // Low tier team matches
                levelKey = "Low";

            // Set the collection of lines to chosenLines
            chosenLines = kickOffLines[levelKey];
            // Random index from from chosenLines string count
            int index = rng.Next(chosenLines.Count);
            // Set one of the strings to introComment
            introComment = kickOffLines[levelKey][index];

            return introComment;

        }

        // Generates courtRows in random string array layouts for fake basketball
        public static List<string[]> GenerateRandomCourts(int rows, int collumns, int count = 10)
        {
            var courtRows = new List<string[]>(count);

            for (int i = 0; i < count; i++)
            {
                // A string array that is empty, with court height
                var court = new string[rows];
                for (int row = 0; row < rows; row++)
                {
                    court[row] = new string(' ', collumns);
                }

                // Function to place an emoji at a random position in court
                void PlaceAt(string emoji)
                {
                    int rowPos = rng.Next(rows);
                    int heightPos = rng.Next(collumns);

                    // Convert the row to a char array / string builder to modify it
                    var fullArray = court[rowPos].ToCharArray();

                    // Build string with substrings;
                    string originalRow = court[rowPos];

                    // Part before insertion
                    string before = originalRow.Substring(0, heightPos);

                    // The emoji to insert
                    string toInsert = emoji;

                    // Part after insertion point
                    string afterString;
                    int insertEnd = heightPos + emoji.Length;
                    if (insertEnd < originalRow.Length)
                    {
                        afterString = originalRow.Substring(insertEnd);
                    }
                    else
                    {
                        afterString = "";  // nothing after if emoji would go beyond end
                    }

                    // Combine them
                    string newRow = before + toInsert + afterString;

                    // Write it back
                    court[rowPos] = newRow;
                }

                // Place the 10 players
                for (int player = 0; player < 10; player++)
                {
                    PlaceAt("⛹🏽");
                }

                // Place balls
                for (int ball = 0; ball < 1; ball++)
                {
                    PlaceAt("🏀");
                }

                courtRows.Add(court);
            }

            return courtRows;
        }
    }
}
