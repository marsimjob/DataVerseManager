using Spectre.Console;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using DataVerseManager.Services;

namespace DataVerseManager.Models
{
    public class Player
    {
        private double height;
        private string country;
        private Team coachTeam;

        // Attributes
        public string PlayerName { get; set; }
        public int PlayerAge { get; set; }
        public double PlayerHeight { get; set; }
        public string PlayerCountry { get; set; }
        
        public Team PlayerTeam = null;
        public string PlayerInfo { get; set; }

        // image path for Canvas.Image
        public string ImageFile { get; set; } = "images/default.png";

        // Stats
        // 0 - 100
        public double Speed { get; set; }
        public double Defending {  get; set; }
        public double Accuracy { get; set; }
        public double Power {  get; set; }

        // Total Stat value of all the above to define total power of player
        public double TotalStat {  get; set; }

        // Constructor 
        // With parameters
        public Player(string name, int age, double height, string country, string imageFile,
                double speed, double defending, double accuracy, double power, string info = "", Team team = null)
        {
            PlayerName = name;
            PlayerAge = age;
            PlayerHeight = height;
            PlayerCountry = country;
            PlayerTeam = team;
            ImageFile = imageFile;
            Speed = speed;
            Defending = defending;
            Accuracy = accuracy;
            Power = power;
            PlayerInfo = info;
            TotalStat = CalculateTotalStat();
        }

        // Default Constructor
        public Player()
        {
        }

        public void AllocateTeam(Team teamToSet)
        {
            PlayerTeam = teamToSet;
        }
        public double CalculateTotalStat()
        {
            // Automatically calculate total
            TotalStat = (Speed + Defending + Accuracy + Power) / 4.0;
            return TotalStat;
        }
        // Methods
        public void ShowPlayerInformation()
        {
            // SET UP
            var playerImage = new CanvasImage(ImageFile).MaxWidth(40).PixelWidth(1);
          
            var teamImage = new CanvasImage("images/default.png").MaxWidth(30).PixelWidth(1);

            var chart = new BarChart().Width(90)
                                      .Label(Markup.Escape($"Stats for {PlayerName}"))
                                      .CenterLabel();

            SpectreGeneric.AddChartBarInColor(chart, Speed, "Speed");
            SpectreGeneric.AddChartBarInColor(chart, Defending, "Defending");
            SpectreGeneric.AddChartBarInColor(chart, Accuracy, "Accuracy");
            SpectreGeneric.AddChartBarInColor(chart, Power, "Power");

            // Text to put info into
            var dumpInfoText =
            $"Age: {PlayerAge}\n" +
            $"Height: {PlayerHeight} cm\n" +
            $"Team: {PlayerTeam.TeamName}\n" +
            $"Info: {PlayerInfo}";
            
            // Panel for info
            var dumpPanel = new Panel(dumpInfoText).Header("[bold]Details[/]")
                                                   .Padding(1, 1)  // some spacing inside
                                                   .BorderColor(Color.Grey);
            string rank;

            if (TotalStat < 20)
                rank = "Newbie";
            else if (TotalStat < 50)
                rank = "Upcomer";
            else if (TotalStat < 75)
                rank = "Pro";
            else
                rank = "Super Star";

            var totalScorePanel = new Panel($"{TotalStat}\n{rank}")
                .Header("[bold]Total Score[/]")
                .Padding(5, 2)
                .BorderColor(Color.Yellow)
                .Expand();

            // RENDER
            var layout = new Layout("Root")
            .SplitRows(
            new Layout("Top").Size(20), 
            new Layout("Bottom").Size(10)
            );

            layout["Top"].SplitColumns(
                new Layout("PlayerImage").Size(40),
                  new Layout("Details").Size(30),
                new Layout("TeamImage").Size(30)
            );

            layout["Bottom"].SplitColumns(
                new Layout("Chart"),
                new Layout("Score").Size(20)
            );

            
            layout["Chart"].Update(new Panel(chart).Header("[bold]Stats[/]"));
           
            layout["Score"].Update(totalScorePanel);

            AnsiConsole.Write(layout);
            Console.ReadLine();
        }


        public void UpdateStats(string statType)
        {
            // Get a random number of double 1-3 and add to stat 
            Random random = new Random();
            // Generate a double between (1.0, 3.0)
            double points = 1.0 + random.NextDouble() * (3.0 - 1.0);
            
            if (points > 0 && points <= 1.5)
            {
                Console.WriteLine($"{PlayerName} had a decent work out");
            }
            else if (points > 1.5 && points <= 2.5)
            {
                Console.WriteLine($"{PlayerName} had a great work out");
            }
            else if (points > 2.5 && points <= 3)
            {
                Console.WriteLine($"{PlayerName} had an AMAZING work out");
            }
            if (statType == "Speed")
            {
                Speed += points;
                string formattedPoints = points.ToString("F1");
                string formattedTotals = Speed.ToString("F1");
                Console.WriteLine($"{PlayerName}'s speed has increased by {formattedPoints} points to a total of {formattedTotals}!");
            }
            else if (statType == "Defending")
            {
                Defending += points;
                string formattedPoints = points.ToString("F1");
                string formattedTotals = Defending.ToString("F1");
                Console.WriteLine($"{PlayerName}'s defense has increased by {formattedPoints} points to a total of {formattedTotals}!");
            }
            else if (statType == "Accuracy")
            {
                Accuracy += points;
                string formattedPoints = points.ToString("F1");
                string formattedTotals = Accuracy.ToString("F1");
                Console.WriteLine($"{PlayerName}'s accuracy has increased by {formattedPoints} points to a total of {formattedTotals}!");
            }
            else if (statType == "Power")
            {
                Power += points;
                string formattedPoints = points.ToString("F1");
                string formattedTotals = Power.ToString("F1");
                Console.WriteLine($"{PlayerName}'s power has increased by {formattedPoints} points to a total of {formattedTotals}!");

            }
        }
    }
}
