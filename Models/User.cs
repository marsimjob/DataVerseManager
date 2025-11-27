using DataVerseManager.Models;
using DataVerseManager.Services;
using Spectre.Console;
using System.Xml.Linq;
using static DataVerseManager.Models.Leaderboard;
namespace DataVerseManager.Models;

public class User
{
    // Attributes
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public Wallet UserWallet = new Wallet();

    public bool hasCoachStatus { get; set; }

    // Constructor
    public User()
    {
        hasCoachStatus = false;
        UserWallet.GetMoney(1000);
    }

    // Methods
    public virtual string ReturnUserInformation()
    {
        string salt = ("c" + Id.ToString());
        string info = $"ID: #{Id} " +
                        $"|| Name: {Name} " +
                        $"|| Wallet Balance: {UserWallet.ReturnWalletBalance()} " +
                        $"|| Decoded Password: {AccountManager.PasswordDecrypt(Password)
                                                  .Substring(0, Password.Length - salt.Length)}";
        return info;
    }

    public void UpgradeToCoach()
    {
        string yesOrNo = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                   .Title($"Would you like to upgrade {Name} from Standard User status to Coach?")
                   .AddChoices(
                   "Yes", "No"
                   )
                   );

        if (yesOrNo == "Yes")
        {
            // Create a new coach, this will remove the User object later and replace it with a Coach object
            Coach newCoach = new Coach();
            newCoach.Name = this.Name;
            newCoach.Password = this.Password;
            newCoach.UserWallet = this.UserWallet;
            newCoach.OriginalId = this.Id;
            hasCoachStatus = true;


            // Looks for first valid ID number, in case somebody has deleted their account and the ID number is open to use
            // Extract all existing IDs into a list for fast lookup
            List<int> usedIds = new List<int>(AccountManager.RegisteredCoaches.Select(used => used.Id));

            // Find the first free ID, starting from 0 and increment. As long as it finds userd IDs it keeps going
            int newId = 0;

            while (usedIds.Contains(newId))
            {
                newId++;
            }

            // Get the increment it lands on after the latest contain and use it
            newCoach.Id = newId;

            // Make a caoch list the gmae can keep track of so it knows if you are coach or not when you enter the menu
            // Make a coach team and give them a name and id
            newCoach.CoachTeam = new Team();

            //// This is just temporary but lets fill the TeamPlayer list with players for now
            newCoach.CoachTeam.TeamPlayer = new List<Player>();

            // Set a default, non special logo for the coach team
            newCoach.CoachTeam.ImageFile = "images/Nba2k26.png";
            // Let user set coach name default to their "user name + Team"
            newCoach.CoachTeam.TeamName = $"{this.Name}'s Team";
            
            // Generate 5 free players for the new Coach, loop this 5 times
            for(int i = 5; i > 0; i--)
            {
                // Generate a new player
                Player newPlayer = PlayerMarket.GeneratePlayer();
                newCoach.CoachTeam.TeamPlayer.Add(newPlayer);
                AnsiConsole.WriteLine($"{newPlayer.PlayerName} was added to {newCoach.Name}'s team"); 
            }

            // Add newCoach's team to to the team list and save to json
            MatchGenerator.AllTeams.Add(newCoach.CoachTeam);
            Team.BuildTeam(newCoach.CoachTeam);
            JsonHandeler.SaveJson<List<Team>>(MatchGenerator.AllTeams, "allteams.json");

            string changeOrNot = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                   .Title($"Would you like to change your team name from '{newCoach.CoachTeam.TeamName}'?").HighlightStyle(new Style(foreground: AppSettings.AccentColor))
                   .AddChoices(
                   "Yes", "No"
                   )
                   );
            if (changeOrNot == "Yes")
            { 
               AnsiConsole.Markup("[grey]Enter your new team name:[/] ");
               string newTeamName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newTeamName)) // if not null
                {
                   newCoach.CoachTeam.TeamName = newTeamName;
                   SpectreGeneric.PrintMessagePrompt($"Team name changed to '{newCoach.CoachTeam.TeamName}'", "green");
                }
                else
                {
                    SpectreGeneric.PrintMessagePrompt($"Standard name '{newCoach.CoachTeam.TeamName}' has been kept", "yellow");
                }
            }
            else if (changeOrNot == "No")
            {
                SpectreGeneric.PrintMessagePrompt($"Standard name '{newCoach.CoachTeam.TeamName}' has been kept", "yellow");
            }

            AccountManager.RegisteredCoaches.Add(newCoach); // Add new coach to coach list
            AccountManager.RegisteredUsers.Remove(this); // Remove user from standard user list
            JsonHandeler.SaveJson<List<Coach>>(AccountManager.RegisteredCoaches, "registeredCoaches.json");
            JsonHandeler.SaveJson<List<User>>(AccountManager.RegisteredUsers, "registeredUsers.json");
            SpectreGeneric.PrintMessagePrompt($"{Name} has been upgraded to Coach status", "green");

        }
        else if (yesOrNo == "No")
        {
            SpectreGeneric.PrintMessagePrompt($"{Name} remains a Standard User");
        }
    }
}
