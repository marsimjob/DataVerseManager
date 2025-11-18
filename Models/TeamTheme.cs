using Spectre.Console;
namespace DataVerseManager
{
        public class TeamTheme
        {
            public Color Primary { get; set; }
            public Color Secondary { get; set; }
            public Color Accent { get; set; }

            public TeamTheme(Color primary, Color secondary, Color accent)
            {
                Primary = primary;
                Secondary = secondary;
                Accent = accent;
            }
        }
}
            

