using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    public class Wallet
    {
        private double Balance;

        public Wallet()
        {
            Balance = 0;
        }
        public void UseMoney(double investment)
        {
            Balance -= investment;
        }
        public void GetMoney(double profit)
        {
            Balance += profit;
        }
        
        public double ReturnWalletBalance()
        {
            if(Balance == null)
            { return 0; }
            return Balance;
        }
        public void ShowWalletBalance()
        {
            Console.WriteLine(Balance + " in wallet currently!");
        }
    }
}
