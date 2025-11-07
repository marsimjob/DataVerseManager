using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    internal class Betting
    {
        // Attributes
        public double Odds {  get; set; }

        // Methods
        public void PlaceBet()
        {
            // Guess total score - Bonus money
            // How much to bet - How much you bet
            // Who to be against - Depending on win rate,
            // you get different money back, but you still profit.
            // Lower for a team = more money back.
            // Higher rate = less money back.
            // Simple risk.
        }
        public void DisplayBettingTable()
        {
            // Show table for chances of winning etc
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("American League");
            Console.ResetColor();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Team Name");
            Console.ResetColor();
            Console.WriteLine();
        }

    }
}
