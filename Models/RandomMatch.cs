using System;
using System.Collections.Generic;
using System.Linq;

namespace DataVerseManager.Models
{
    // Klass som hanterar en slumpad match mot ett annat lag 
    internal class RandomMatch
    {
        // Används för att slumpa motståndare och poäng
        private readonly Random _random = new Random();

        // Startar en match mellan ditt lag och ett slumpat motståndarlag
        // myTeam = ditt lag
        // allTeams = alla lag som finns i spelet (inklusive ditt lag)
        public void Play(Team myTeam, List<Team> allTeams)
        {
            Console.Clear();

            // Visa reglerna för spelläget (matchar din bild)
            Console.WriteLine("Play A Game/Match");
            Console.WriteLine("-----------------");
            Console.WriteLine("* Start a match with your team against random other team");
            Console.WriteLine("* If you win a match you can get a member from their team and replace yours.");
            Console.WriteLine();

            // Hitta ett slumpat motståndarlag som inte är ditt eget
            Team opponent = GetRandomOpponent(myTeam, allTeams);

            // Slumpa poäng 0–5 för båda lagen
            int myScore = _random.Next(0, 6);
            int opponentScore = _random.Next(0, 6);

            // Visa själva matchresultatet
            Console.WriteLine($"{myTeam.TeamName} vs {opponent.TeamName}");
            Console.WriteLine($"Result: {myScore} - {opponentScore}");
            Console.WriteLine();

            // Om du vinner matchen
            if (myScore > opponentScore)
            {
                Console.WriteLine("You won the match!");
                Console.WriteLine("According to the rules, you may get a member from their team and replace yours.");
                Console.WriteLine();

                // Försök stjäla en slumpad spelare från motståndarlaget
                Player? gainedPlayer = StealRandomPlayer(opponent, myTeam);

                if (gainedPlayer != null)
                {
                    Console.WriteLine(
                    $"You got {gainedPlayer.PlayerName} from {opponent.TeamName} and added them to {myTeam.TeamName}!");
                }
                else
                {
                    // Om motståndarlaget inte hade några spelare
                    Console.WriteLine("However, the opponent had no players to take.");
                }

                // Uppdatera vinst/förlust-räknare
                myTeam.TeamWins++;
                opponent.TeamLoses++;
            }
            // Om du förlorar matchen
            else if (myScore < opponentScore)
            {
                Console.WriteLine("You lost the match.");
                Console.WriteLine("Because you did NOT win, you do NOT get a member from the other team.");

                opponent.TeamWins++;
                myTeam.TeamLoses++;
            }
            // Oavgjort
            else
            {
                Console.WriteLine("The match ended in a draw.");
                Console.WriteLine("Because you did NOT win, you do NOT get a member from the other team.");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        // Väljer ett slumpat motståndarlag som inte är ditt lag
        private Team GetRandomOpponent(Team myTeam, List<Team> allTeams)
        {
            // Skapa en lista med alla lag förutom ditt eget
            List<Team> opponents = allTeams
            .Where(team => team != myTeam)
            .ToList();

            // Om det inte finns några andra lag att möta
            if (opponents.Count == 0)
            {
                throw new InvalidOperationException("There are no other teams to play against.");
            }

            // Välj ett slumpmässigt lag ur listan
            int index = _random.Next(opponents.Count);
            return opponents[index];
        }

        // Stjäl en slumpad spelare från fromTeam och flyttar den till toTeam
        // Om toTeam är fullt får användaren välja vilken spelare som ska bytas ut
        // Returnerar spelaren som flyttades, eller null om fromTeam saknar spelare
        private Player? StealRandomPlayer(Team fromTeam, Team toTeam)
        {
            // Om motståndarlaget inte har några spelare
            if (fromTeam.TeamPlayer.Count == 0)
            {
                return null;
            }

            // Välj en slumpad spelare att ta från motståndarlaget
            int index = _random.Next(fromTeam.TeamPlayer.Count);
            Player stolenPlayer = fromTeam.TeamPlayer[index];

            // Ta bort spelaren från motståndarlaget
            fromTeam.TeamPlayer.RemoveAt(index);

            // Om ditt lag har plats (t.ex. färre än 5 spelare)
            if (toTeam.TeamPlayer.Count < 5)
            {
                toTeam.AddTeamMember(stolenPlayer);
                stolenPlayer.PlayerTeam = toTeam;
                return stolenPlayer;
            }

            // Om laget är fullt: låt spelaren välja vem som ska ersättas
            Console.WriteLine();
            Console.WriteLine($"Your team \"{toTeam.TeamName}\" is full.");
            Console.WriteLine($"Choose a player to replace with {stolenPlayer.PlayerName}:");
            Console.WriteLine();

            // Visa alla spelare i ditt lag med nummer
            for (int i = 0; i < toTeam.TeamPlayer.Count; i++)
            {
                Player p = toTeam.TeamPlayer[i];
                Console.WriteLine($"{i + 1}. {p.PlayerName} (TotalStat: {p.TotalStat:F1})");
            }

            Console.WriteLine();
            int choice;

            // Be användaren skriva ett giltigt nummer
            while (true)
            {
                Console.Write($"Enter a number (1-{toTeam.TeamPlayer.Count}): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out choice) &&
                choice >= 1 && choice <= toTeam.TeamPlayer.Count)
                {
                    break;
                }

                Console.WriteLine("Invalid input, try again.");
            }

            // Index i listan (0-baserat)
            int indexToReplace = choice - 1;
            Player oldPlayer = toTeam.TeamPlayer[indexToReplace];

            // Byt ut den gamla spelaren mot den nya
            toTeam.TeamPlayer[indexToReplace] = stolenPlayer;
            stolenPlayer.PlayerTeam = toTeam;

            Console.WriteLine();
            Console.WriteLine($"{stolenPlayer.PlayerName} has replaced {oldPlayer.PlayerName} in {toTeam.TeamName}!");

            return stolenPlayer;
        }
    }
}