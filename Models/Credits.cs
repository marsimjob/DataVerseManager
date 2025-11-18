using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    public static class Credits
    {
        static string[] lines = new[]
        {
            "NBA SHOWTIME 2K26 Credits",
            "",
            "",
            "Code Architecture: Bavel",
            "",
            "UI/UX Designer: Hager",
            "",
            "Project Lead: Mario",
            "",
            "Team Cordinator: Mazen",
            "",
            "Database Manager: Mohammed",
            "",
            "",
            "And a big thanks to you, the user!",
            "",
            "See you next time!"
        };

        public static void ScrollCredits()
        {
            Console.Clear();
            int consoleHeight = Console.WindowHeight;
            int blankLines = consoleHeight;

            for (int i = 0; i < lines.Length + blankLines; i++)
            {
                Console.Clear();
                var sb = new StringBuilder();

                // For each visible row in the window
                for (int row = 0; row < consoleHeight; row++)
                {
                    int lineIndex = i - (consoleHeight - row - 1);
                    if (lineIndex >= 0 && lineIndex < lines.Length)
                    {
                        sb.AppendLine(lines[lineIndex]);
                    }
                    else
                    {
                        sb.AppendLine("");
                    }
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(sb.ToString());

                Thread.Sleep(200);
            }
        }
    }
}
