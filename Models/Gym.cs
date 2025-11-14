using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace DataVerseManager.Models
{
    internal class Gym
    {
        public void PickAndTrainPlayer(List<Player> teamList)
        {
            double cashAtHand = 1000000;

            if (cashAtHand <= 0)
            {
                AnsiConsole.WriteLine("You're broke!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            // Choose your player
            var selectedNames = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select one or more players to train: ")
                    .NotRequired()
                    .PageSize(10)
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a player, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(teamList.Select(p => p.PlayerName))
            );

            Console.Clear();

            if(selectedNames.Count <= 0)
            {
                Console.WriteLine("No players selected. Back to main menu");
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
                "Speed" => 10000,
                "Strength" => 15000,
                "Defense" => 12500,
                "Accuracy" => 20000,
                _ => 10000
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
            ConfirmationPrompt prompt;
            
            if (selectedNames.Count == 1)
            {
                prompt = new ConfirmationPrompt($"Upgrade {statToUpgrade} for player at cost {totalCost:C}?");
            }
            else
            {
                prompt = new ConfirmationPrompt($"Upgrade {statToUpgrade} for {selectedNames.Count} players at cost {totalCost:C}?");
            }

            var chooseYes = AnsiConsole.Prompt(prompt);

            if (!chooseYes)
            {
                AnsiConsole.WriteLine("Training cancelled!");
                return;
            }

            if (totalCost > cashAtHand)
            {
                AnsiConsole.WriteLine($"Not enough cash.\n" +
                    $"You have: {cashAtHand:C}\n" +
                    $"Needed: {totalCost:C}.");
                return;
            }

            cashAtHand -= totalCost;
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

            Console.WriteLine($"{cashAtHand:C} left");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
