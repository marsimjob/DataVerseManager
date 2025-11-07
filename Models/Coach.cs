using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    internal class Coach
    {
        // Attribute
        public string CoachName { get; set; }

        Wallet CoachWallet { get; set; }
    
        Team CoachTeam { get; set; }

        // Constructor
        public Coach() 
        {
            CoachWallet.GetMoney(1000);
        }

        // Method
        public void ChangeCoachName()
        {
            // Change TeamCoachname variable with user readLine()
        }

    }
}
