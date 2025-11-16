using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    public class Coach : User
    {
        // Attribute
        public string CoachName { get; set; }
        public Team CoachTeam { get; set; }

        // Constructor
        public Coach() 
        {
            UserWallet = new Wallet();
            CoachTeam = new Team();
            UserWallet.GetMoney(1000);
        }

        // Method
        public void ChangeCoachName()
        {
            Console.WriteLine("What would you like your new Coach Name to be?: ");
            string newName = Console.ReadLine();

            var yesOrNo = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title($"Is the name {newName} okay?")
        .AddChoices(
          "Yes", "No"
        )
        );

            yesOrNo = yesOrNo?.Trim();
            if (yesOrNo?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true)
            {
                CoachName = newName;
                AnsiConsole.WriteLine($"Your name has changed to {newName}");
                Console.ReadLine();
                Console.Clear();
            }
            else if ((yesOrNo?.Equals("No", StringComparison.OrdinalIgnoreCase) == true))
            {
                AnsiConsole.WriteLine("Your old name has been keept!");
                Console.ReadLine();
                Console.Clear();
                return;
            }
        }
        public void ChangeTeamName()
        {
            Console.WriteLine("Enter the new Name of your Team: ");
            string newName = Console.ReadLine();

            var yesOrNo = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
            .Title($"Is the name {newName} okay?")
            .AddChoices(
            "Yes", "No")
            );

            yesOrNo = yesOrNo?.Trim();
            if (yesOrNo?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true)
            {
                CoachTeam.TeamName = newName;
                AnsiConsole.WriteLine($"Your Team's Name has changed to {newName}");
                Console.ReadLine();
                Console.Clear();
            }
            else if ((yesOrNo?.Equals("No", StringComparison.OrdinalIgnoreCase) == true))
            {
                AnsiConsole.WriteLine("Your old Team Name has been keept!");
                Console.ReadLine();
                Console.Clear();
                return;
            }
        }
    }
}
