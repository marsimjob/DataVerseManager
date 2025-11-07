using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    internal class Player
    {
        // Attributes
        public string PlayerName { get; set; }
        public int PlayerAge { get; set; }
        public double PlayerHeight { get; set; }
        public string PlayerCountry { get; set; }
        public string PlayerTeam { get; set; }

        public string PlayerInfo { get; set; }

        // Stats
        // 0 - 100
        public double Speed { get; set; }
        public double Defending {  get; set; }
        public double Accuracy { get; set; }
        public double Power {  get; set; }

        // Total Stat value of all the above to define total power of player
        public double TotalStat {  get; set; }

        // Constructor 
        public Player()
        {
            // TotalStat max is 400, 100 from each of the 4 stats
            TotalStat = Speed + Defending + Power + Accuracy;
        }

        // Methods
        public void ShowPlayerInformation()
        {
            // Shows a screen of current player information
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
