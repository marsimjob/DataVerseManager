using DataVerseManager;
using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using System.Linq;

public static class LookMembers
{
    // Visar "My Team" – alla spelare i coachens team
    public static void ShowMyTeam(Team team)
    {
        Console.Clear();

        //  Om coachen inte har något team än
        if (team == null)
        {
            AnsiConsole.MarkupLine("[red]You don't have a team yet.[/]");
            Console.ReadLine();
            return;
        }

        //  Om teamet finns men inga spelare är tillagda ännu
        if (team.TeamPlayer == null || team.TeamPlayer.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]Your team has no players yet.[/]");
            Console.ReadLine();
            return;
        }

        // Titel överst på sidan
        // Visar lagets namn och stylad med AppSettings-färger
        SpectreGeneric.PresentTopTitle(
            $"{team.TeamName} – MY TEAM",
            AppSettings.MainColor,
            AppSettings.SubColor
        );

        bool running = true;

        while (running)
        {
            Console.Clear();
            // Bygger en lista med spelarnas namn + en "RETURN"
            // Detta skapar menyval i SpectreConsole
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Choose a player to view details[/] (or RETURN):")
                    .PageSize(10) // hur många som syns på skärmen
                    .AddChoices(
                        team.TeamPlayer.Select(p => p.PlayerName)
                        .Append("RETURN TO COACH MENU") // lägger till en retur-knapp
                    )
            );

            //  Om användaren väljer att gå tillbaka
            if (choice == "RETURN TO COACH MENU")
            {
                running = false;
                return;
            }

            //  Hitta den spelare som valdes i listan
            var selectedPlayer = team.TeamPlayer
                .FirstOrDefault(p => p.PlayerName == choice);

            //  Om spelaren finns – visa detaljer
            if (selectedPlayer != null)
            {
                Console.Clear();
                selectedPlayer.ShowPlayerInformation();  
            }
        }
    }
}