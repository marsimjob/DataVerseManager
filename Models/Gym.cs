using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVerseManager.Services;
using Spectre.Console;

namespace DataVerseManager.Models
{
    public static class Gym
    {
        public static void RunGym(Coach coach)
        {
            List<Player> teamList = coach.CoachTeam.TeamPlayer;

            coach.UserWallet.GetMoney(10000);
            double cashAtHand = coach.UserWallet.ReturnWalletBalance();

        
            if (cashAtHand <= 0)
            {
                AnsiConsole.WriteLine("You're broke!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            if(teamList.Count <= 0)
            {
                SpectreGeneric.PrintMessagePrompt("You don't have any members on your team!", "red");
                return;
            }
            // Choose your player
            var selectedNames = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select one or more players to train: ")
                    .PageSize(10)
                    .AddChoices(teamList.Select(p => p.PlayerName))
            );

            Console.Clear();

            if(selectedNames.Count <= 0)
            {

                SpectreGeneric.PrintMessagePrompt("No players selected. Back to main menu", "red");
                return;
            }

            // After selection of players:
            var selectedPlayers = teamList
                .Where(p => selectedNames.Contains(p.PlayerName))
                .ToList();

            // For each selected player, or maybe one at a time, show the chart:
            foreach (var player in selectedPlayers)
            {
                AnsiConsole.Write(
                    new BarChart()
                        .Width(50)
                        .Label($"[bold]{player.PlayerName} Stats[/]")
                        .CenterLabel()
                        .AddItem("Speed", player.Speed, Color.Blue)
                        .AddItem("Defending", player.Defending, Color.Green)
                        .AddItem("Accuracy", player.Accuracy, Color.Yellow)
                        .AddItem("Power", player.Power, Color.Red)
                        .UseValueFormatter(val => $"{val:F1}")
                );
            }

            // Choose a stat to work out with Spectre.Console
            var statToUpgrade = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What area do you want to work out?")
                    .AddChoices("Speed", "Defending", "Accuracy", "Power")
            );

            // Get the base cost to multiply with selected players
            double baseCost = statToUpgrade switch
            {
                "Speed" => 1000,
                "Strength" => 1500,
                "Defense" => 1250,
                "Accuracy" => 2000,
                _ => 1000
            };

            // Set up a cost that will total all costs
            double totalCost = 0;
            
            // Go through each selected player and att their single costs to total
            foreach(var player in selectedPlayers)
            {
                // Reference what stat level is
                double currenStat = statToUpgrade switch
                {
                    "Speed" => player.Speed,
                    "Strength" => player.Power,
                    "Defending" => player.Defending,
                    "Accuracy" => player.Accuracy,
                    _ => 0
                };

                double singleCost = baseCost * (currenStat / 100.0);
             
                totalCost += singleCost;  
            }

            Console.Clear();

            // User confirms choice
            var yesOrNo = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"Upgrade {statToUpgrade} for player at cost ${totalCost:F2}?")
            .AddChoices(
            "Yes", "No")
            );

            if (yesOrNo == "Yes")
            {
                var yesOrNo2 = AnsiConsole.Prompt(
                         new SelectionPrompt<string>()
                         .Title($"Upgrade {statToUpgrade} for {selectedNames.Count} players at cost ${totalCost:F2}?")
                         .AddChoices(
                         "Yes", "No")
                         );

                if (yesOrNo2 == "Yes")
                {
                    if (totalCost > cashAtHand)
                    {
                        SpectreGeneric.PrintMessagePrompt($"Not enough cash.\n" +
                            $"You have: ${cashAtHand:F2}\n" +
                            $"Needed: ${totalCost:F2}.", "red");
                        return;
                    }

                    coach.UserWallet.UseMoney(totalCost);
                    Console.Clear();

                    // Apply new stars
                    foreach (string name in selectedNames)
                    {
                        var player = teamList.FirstOrDefault(p => p.PlayerName == name);
                        if (player != null)
                        {
                            player.UpdateStats(statToUpgrade); // Upgrades from Player class
                        }
                    }
                    SpectreGeneric.PrintMessagePrompt($"Cash left: ${coach.UserWallet.ReturnWalletBalance():F2}");

                    // Remove and replace each trained player
                    for (int i = 0; i < selectedPlayers.Count(); i++)
                    {
                        coach.CoachTeam.TeamPlayer.Remove(coach.CoachTeam.TeamPlayer.Find(teamPlayer => teamPlayer.PlayerName == selectedPlayers[i].PlayerName));
                        coach.CoachTeam.TeamPlayer.Add(selectedPlayers[i]);
                    }

                    // Remove and replace team for the updated one
                    if(MatchGenerator.AllTeams.Any(team => team.TeamName.Equals(coach.CoachTeam.TeamName)))
                    {
                        MatchGenerator.AllTeams.Remove(MatchGenerator.AllTeams.Find(team => team.TeamName.Equals(coach.CoachTeam.TeamName)));
                    }
                    MatchGenerator.AllTeams.Add(coach.CoachTeam);
                    JsonHandeler.SaveJson<List<Team>>(MatchGenerator.AllTeams, "allteams.json");
                }
                else
                {
                    SpectreGeneric.PrintMessagePrompt("Training cancelled.", "red");
                    return;
                }
            }
            else 
            {
                SpectreGeneric.PrintMessagePrompt("Training cancelled.", "red");
                return;
            }
        }
    }
}
