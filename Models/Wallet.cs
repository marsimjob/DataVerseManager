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
            return Balance;
        }
        public void ShowWalletBalance()
        {
            Console.WriteLine(Balance + " in wallet currently!");
        }
    }
}
