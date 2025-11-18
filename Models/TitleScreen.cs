using Spectre.Console;
using System;
using System.Threading;
namespace DataVerseManager.Models
{
    public static class TitleScreen
    {
        public static void ShowSplashScreen()
        {
            AnsiConsole.Clear();

            var image = new Spectre.Console.CanvasImage("images/Nba2k26.png")
                .MaxWidth(50).NearestNeighborResampler();

            var title = new FigletText("Nba ShowTime")
                .Centered()
                .Color(Color.Yellow);

           
            AnsiConsole.Write(title);
           
            AnsiConsole.Write(image);
            

            var subtitle = "[bold Yellow italic]Press any key to continue[/]";
            var y = Console.CursorTop; // spara var vi börjar skriva

            bool visible = true;

            while (!Console.KeyAvailable)
            {
                // Gå till samma plats varje gång
                AnsiConsole.Cursor.SetPosition(0, y);

                if (visible)
                {
                    AnsiConsole.Write(new Align(new Markup(subtitle), HorizontalAlignment.Center));
                }
                else
                {
                    // Rita samma längd med mellanslag så texten "försvinner"
                    var spaces = new string(' ', Console.WindowWidth);
                    AnsiConsole.Markup(spaces);
                }

                visible = !visible;
                Thread.Sleep(500);
            }

            Console.ReadKey(true);
        }
    }
}
