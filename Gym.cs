using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager
{
    internal class Gym
    {
        public void PickAndTrainPlayer(Team teamList)
        {
            // Ask if user wants to pay by pulling money from their wallet

            // Show a selectable teamlist and choose a player (int)
            //   Player 1  << Selection 0
            //   Player 2  << Selection 1
            //   Player 3  << Selection 2
            // > Player 4  << ...
            //   Player 5

            // Choose what stat to upgrade
            // > Speed  << Selection 0
            //   Defending  << Selection 1
            //   Accuracy  << Selection 2
            //   Power << ...

            // If the different stats cost different amount to upgrade, ask
            // user here if they are willing to pay from their wallet. If
            // They don't want to pay, return to menu and return default switch.

            // EXAMPLE OF PRICING:
            // If stats are currently on the lower end (0-25) then the pricing is smaller.
            // (26-60) is medium so it is slightly higher.
            // (61-100) is the most expensive.

            // Depending on selection use the info to make a switch case
            // Switch statement
            // case "Speed":
            // teamList[SELECTION].UpdateStats("Speed");
            // case "Defending":
            // teamList[SELECTION].UpdateStats("Defending
            // ...
            // default:
            // Console.WriteLine("Training failed");
        }
    }
}
