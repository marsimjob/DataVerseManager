using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Services
{
    public static class SpectreGeneric
    {
        public static void PresentTopTitle(string contents, string mainColor, string subColor)
        {
            AnsiConsole.MarkupLine($"[{mainColor}]🏀 -- NBA SHOWTIME 2K26 -- 🏀[/]");
            var ruler = new Spectre.Console.Rule(contents);
            ruler.Style = Style.Parse(subColor);
            AnsiConsole.Write(ruler);
        }
        public static void LoadScreen()
        {
            Console.Clear();
            AnsiConsole.Status()
           .Spinner(Spinner.Known.Aesthetic)       
           .SpinnerStyle(Style.Parse("grey"))              
           .Start("[grey]Loading, please wait…[/]", ctx =>        
           {
               // Top
               AnsiConsole.MarkupLine("[#ffa500]🏀 -- NBA ShowTime 2K26 -- 🏀[/]");
              
               // I'm going to make a basketball roll over the screen
               // String that I will update in the loop
               string basketLine = "";
               // Rolling ball intervals
               int rollIntervals = 45;
               for (int i = 0; i < rollIntervals; i++)
               {
                   if (i < rollIntervals)
                   {
                       // Rooling ball, refreshing the basketLine string
                       basketLine += "\b\b" + " 🏀";
                       AnsiConsole.Markup(basketLine);
                       Thread.Sleep(50);
                   }
                   else
                   {
                       // Final touch, make the basketball pang in the end of the loop
                       basketLine += "\b\b" + " 💥";
                       AnsiConsole.Markup(basketLine);
                       Thread.Sleep(100);
                   }
               }
               // Updating the final text!
               // The text should end up saying Play Ball!:
               string playText = "Play Ball!";
               // Making a string to store my updating text into:
               string refreshedText = "";
               for (int i = 0; i < playText.Length; i++)
               {
                   // Write out the white and grey parts and combine them
                   // Start at zero, then for each iteration end the substring at i + 1
                   string whitePart = playText.Substring(0, i + 1);
                   // String is the rest of the playText substring so i + 1
                   string greyPart = playText.Substring(i + 1);
                   // Combine and give color to each sides of the refreshedText string object
                   refreshedText = $"[white]{whitePart}[/][grey]{greyPart}[/]";

                   // Print it
                   ctx.Status(refreshedText)
                   .Spinner(Spinner.Known.Aesthetic)
                   .SpinnerStyle(Style.Parse("white"));
                   
                   Thread.Sleep(100); 
               }

               // End finally as white
               ctx.Status("[white]" + playText + "[/]");

               // Pause for a while
               Thread.Sleep(1000);
           });
        }
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

            bar.AddItem(valueName, value, color).UseValueFormatter(val => $"{val:F1}") ;
        }

        public static void PrintMessagePrompt(string message, string color = "white")
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[{color}]{message}[/]");
            AnsiConsole.Markup("[grey]Press any key to continue...[/]");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
