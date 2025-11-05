using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager
{
    internal class Wallet
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
    }
}
