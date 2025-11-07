using Spectre.Console;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using DataVerseManager.UI;

namespace DataVerseManager.Models
{
    internal class Player
    {
        // Attributes
        public string PlayerName { get; set; }
        public int PlayerAge { get; set; }
        public double PlayerHeight { get; set; }
        public string PlayerCountry { get; set; }
        public Team PlayerTeam { get; set; }

        public string PlayerInfo { get; set; }

        // image path for Canvas.Image
        public string ImageFile { get; set; }

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
        public Player(string name, int age, double height, string country, Team team, string imageFile,
                double speed, double defending, double accuracy, double power, string info = "")
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

            // Automatically calculate total
            TotalStat = (Speed + Defending + Accuracy + Power) / 4.0;
        }
       
        // Default Constructor
        public Player()
        {
            PlayerName = "Unknown Player";
            PlayerAge = 18;
            PlayerHeight = 170;
            PlayerCountry = "Unknown";
            PlayerTeam = new Team();
            ImageFile = "default.png";
            Speed = 0;
            Defending = 0;
            Accuracy = 0;
            Power = 0;
            PlayerInfo = "No information available.";
            TotalStat = 0;
        }


        // Methods
        public void ShowPlayerInformation()
        {
            // SET UP
            var playerImage = new CanvasImage(ImageFile).MaxWidth(90);
            var teamImage = new CanvasImage(PlayerTeam.ImageFile).MaxWidth(30);

            var chart = new BarChart().Width(90)
                                      .Label($"Stats for {PlayerName}")
                                      .CenterLabel();
                                               

            SpectreGeneric.AddChartBarInColor(chart, Speed, "Speed");
            SpectreGeneric.AddChartBarInColor(chart, Defending, "Defending");
            SpectreGeneric.AddChartBarInColor(chart, Accuracy, "Accuracy");
            SpectreGeneric.AddChartBarInColor(chart, Power, "Power");

            // Build a text block for the “dumping info”
            var dumpInfoText =
                $"Age: {PlayerAge}\n" +
                $"Height: {PlayerHeight} cm\n" +
                $"Team: {PlayerTeam.TeamName}\n" +
                "\n" +
                $"Information: {PlayerInfo}";

            // Wrap it in a Panel (optional) so it has a border / header
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

            layout["PlayerImage"].Update(new Panel(playerImage).Header(PlayerName));
            layout["TeamImage"].Update(new Panel(teamImage).Header("Team"));
            layout["Chart"].Update(new Panel(chart).Header("[bold]Stats[/]"));
            layout["Details"].Update(dumpPanel);
            layout["Score"].Update(totalScorePanel);

            AnsiConsole.Write(layout);

            // I used a grid first:
            //var grid = new Grid()
            //   .AddColumn()
            //   .AddColumn()
            //   .AddColumn();

            //grid.AddRow(
            //    new Panel(playerImage).Header(PlayerName),
            //    dumpPanel,
            //    new Panel(teamImage).Header("Team")
            //);

            //grid.AddRow(
            //     new Panel(chart).Header("[bold]Stats[/]"),
            //     new Markup(" "),
            //     totalScorePanel
            //);

            //grid.Columns[0].Width(45);
            //grid.Columns[1].Width(22);

            //for (int i = 0; i < 3; i++)
            //{
            //    grid.Columns[i].Centered();
            //}
            //AnsiConsole.Write(grid);
        }

        public void ChangeOrSetTeam()
        {
            // Set new team for current player
            // Looks if player is any of the other team lists- remove it from there
            // Add to a new team of choice
        }
         
        public void UpdateStats(string statType)
        {
            // Get a random number of double 1-3 and add to stat 
            Random random = new Random();

            // Generate a double between (1.0, 3.0)
            double points = 1.0 + random.NextDouble() * (3.0 - 1.0);

            if (statType == "Speed")
            {
                Speed += points;
                Console.WriteLine($"{PlayerName}'s speed has increased by {points} points!");
            }
            else if(statType == "Defending")
            {
                Defending += points;
                Console.WriteLine($"{PlayerName}'s defense has increased by {points} points!");
            }
            else if(statType == "Accuracy")
            {
                Accuracy += points;
                Console.WriteLine($"{PlayerName}'s accuracy has increased by {points} points!");
            }
            else if( statType == "Power")
            {
                Power += points;
                Console.WriteLine($"{PlayerName}'s power has increased by {points} points!");
            }
        }
    }
}
