using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.UI
{
    public static class SpectreGeneric
    {
        public static void AddChartBarInColor(BarChart bar, double value, string valueName)
        {
            Color color;

            if (value <= 20)
            {
                color = Color.Grey;
            }
            else if (value <= 50)
            {
                color = Color.Yellow;
            }
            else if (value <= 75)
            {
                color = Color.Blue;
            }
            else // > 75
            {
                color = Color.Green;
            }

            bar.AddItem(valueName, value, color);
        }
    }
}
