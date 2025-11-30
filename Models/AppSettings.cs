using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
namespace DataVerseManager;
         public static class AppSettings
{

    public static Dictionary<string, TeamTheme> TeamThemes = new Dictionary<string, TeamTheme>
    {
        ["Warriors"] = new TeamTheme(Color.Blue, Color.Yellow, Color.White),
        ["Lakers"] = new TeamTheme(Color.Purple, Color.Yellow, Color.White),
        ["Knicks"] = new TeamTheme(Color.Orange1, Color.Blue1, Color.White),
        ["Bulls"] = new TeamTheme(Color.Red, Color.Grey, Color.White),
        ["Celtics"] = new TeamTheme(Color.Green, Color.White, Color.Grey),
        ["Heat"] = new TeamTheme(Color.Red, Color.Grey, Color.Yellow),
        ["Nets"] = new TeamTheme(Color.Grey, Color.White, Color.Grey),
        ["Mavericks"] = new TeamTheme(Color.Blue, Color.White, Color.Silver),
        ["Clippers"] = new TeamTheme(Color.Red, Color.Blue, Color.White),
        ["Rockets"] = new TeamTheme(Color.Red, Color.White, Color.Grey),
    };

    // Using Spectre.Console mark up tags for colors
    public struct UserColors
    {
        public string StoredMainColor { get; set; }
        public string StoredSubColor { get; set; }
        public string StoredAccentColor { get; set; }
    }

    public static string MainColor = "white";
    public static string SubColor = "grey";
    public static Color AccentColor = Color.Orange1;

    public static void RunSettings()
    {

        bool inSettingsMenu = true;
        while (inSettingsMenu)
        {

            Console.Clear();

            SpectreGeneric.PresentTopTitle("APP SETTINGS", MainColor, SubColor);

            string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[{MainColor}]Settings Menu[/]")
            .PageSize(10)
            .HighlightStyle(new Style(foreground: AccentColor))
            .AddChoices(new[] {
                    "CHANGE MAIN COLOR", "CREDITS", "BACK TO TOP MENU"
            }));
            switch (choice)
            {
                case "CHANGE MAIN COLOR":
                    SelectTeamTheme();
                    break;
                case "CREDITS":
                    Credits.ScrollCredits();
                    break;
                case "BACK TO TOP MENU":
                    inSettingsMenu = false;
                    break;
                default:
                    break;
            }
        }
    }
    public static void SelectTeamTheme()
    {
        Console.Clear();
        SpectreGeneric.PresentTopTitle("SELECT APP THEME COLORS", MainColor, SubColor);

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[grey]Choose an NBA team to set its color theme:[/]")
                .PageSize(10)
                .HighlightStyle(new Style(foreground: AccentColor))
                .AddChoices(TeamThemes.Keys)
        );

        if (!TeamThemes.TryGetValue(choice, out var theme))
            return;

        MainColor = theme.Primary.ToString();
        SubColor = theme.Secondary.ToString();
        AccentColor = theme.Accent;

        // save these to user struct so we can load them from json later when user logs in
        JsonHandeler.SaveJson<UserColors>(new AppSettings.UserColors()
        {
            StoredMainColor = theme.Primary.ToString(),
            StoredSubColor = theme.Secondary.ToString(),
            StoredAccentColor = theme.Accent.ToHex()
        }, "userappsettings.json");
    }
    public static void LoadUserTheme()
    {
        UserColors data = JsonHandeler.LoadJson<UserColors>("userappsettings.json");

        MainColor = data.StoredMainColor;
        SubColor = data.StoredSubColor;
        AccentColor = Color.FromHex(data.StoredAccentColor);
    }
}



