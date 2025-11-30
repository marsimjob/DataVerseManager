using DataVerseManager.Services;
using Spectre.Console;
using Spectre.Console.Extensions;
using System;
namespace DataVerseManager.Models
{
    public static class AccountManager
    {
        // We created an AccountManager class that handles log in and register of accounts.
        // Additionally: We added a Password retriever for forgotten passwords for the users
        // Erasing accounts depending on their status is also available.
        // And a debug setting for us developers.
        // List of registered users -- I set it to our json, if the json fails we make a new list
        public static List<User> RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json") ?? new List<User>();
        // List of registered coaches -- same as above
        public static List<Coach> RegisteredCoaches = JsonHandeler.LoadJson<List<Coach>>("registeredCoaches.json") ?? new List<Coach>();

        // Selection array
        private static readonly string[] LogInMenuChoices = new string[]
        {
            "LOG-IN",
            "REGISTER",
            "FORGOT PASSWORD",
            "ERASE ACCOUNT",
            "[grey]DEBUG[/]",
            "EXIT APPLICATION"
        };

        // ENCODE TABLES ARE USED FOR PASSWORD ENCRYPTOR AND DECRYPTOR (almost like hashing but easier):
        // Encode table Dictionary that holds characters that will replace the Key characeter referenced
        private static readonly Dictionary<char, char> EncodeTable = new Dictionary<char, char>()
{
    // Uppercase
    {'A','Q'}, {'B','W'}, {'C','E'}, {'D','R'}, {'E','T'}, {'F','Y'}, {'G','U'}, {'H','I'}, {'I','O'}, {'J','P'},
    {'K','A'}, {'L','S'}, {'M','D'}, {'N','F'}, {'O','G'}, {'P','H'}, {'Q','J'}, {'R','K'}, {'S','L'}, {'T','Z'},
    {'U','X'}, {'V','C'}, {'W','V'}, {'X','B'}, {'Y','N'}, {'Z','M'},

    // Lowercase
    {'a','q'}, {'b','w'}, {'c','e'}, {'d','r'}, {'e','t'}, {'f','y'}, {'g','u'}, {'h','i'}, {'i','o'}, {'j','p'},
    {'k','a'}, {'l','s'}, {'m','d'}, {'n','f'}, {'o','g'}, {'p','h'}, {'q','j'}, {'r','k'}, {'s','l'}, {'t','z'},
    {'u','x'}, {'v','c'}, {'w','v'}, {'x','b'}, {'y','n'}, {'z','m'},

    // Numbers
    {'0','5'}, {'1','7'}, {'2','9'}, {'3','8'}, {'4','6'},
    {'5','0'}, {'6','4'}, {'7','1'}, {'8','3'}, {'9','2'},

    // Special characters
    {'!','@'}, {'@','#'}, {'#','$'}, {'$','%'}, {'%','^'}, {'^','&'}, {'&','*'}, {'*','('}, {'(',')'}, {')','!'},
    {'-','_'}, {'_','+'}, {'+','='}, {'=','-'},

    {'[',']'}, {']','{'}, {'{','}'}, {'}','['},
    {';',':'}, {':',';'}, {'\'','"'}, {'"','\''},

    {'<','>'}, {'>','?'}, {'?','/'}, {'/','<'}
};

        // Reverse table, we used LINQ to reverse the key and value to be the opposite of EncodeTable 
        private static readonly Dictionary<char, char> DecodeTable =
        EncodeTable.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        private static string PasswordEncryptor(string password)
        {
            char[] result = new char[password.Length];

            for (int i = 0; i < password.Length; i++)
            {
                char currentChar = password[i];  // Get the current character

                // Check if this character has a mapping in the EncodeTable
                if (EncodeTable.ContainsKey(currentChar))
                {
                    result[i] = EncodeTable[currentChar]; // Replace with mapped character
                }
                else
                {
                    result[i] = currentChar; // Keep the character as is if no mapping
                }
            }

            return new string(result);
        }

        public static string PasswordDecrypt(string encryptedPassword)
        {
            char[] result = new char[encryptedPassword.Length];

            for (int i = 0; i < encryptedPassword.Length; i++)
            {
                char c = encryptedPassword[i];
                result[i] = DecodeTable.ContainsKey(c) ? DecodeTable[c] : c;
            }

            return new string(result);
        }

        public static void ForgotPassword()
        {
            Console.Write("Please write your username: ");
            string searchUserName = Console.ReadLine();

            // Look for user in registered users list
            User thisUser = RegisteredUsers.Find(user =>
                user.Name.Equals(searchUserName));

            // If it doesn't find it there, look for it in the coach list
            // (Coach class inherits from User class, so it will work the same but upgraded)
            if (thisUser == null)
            {
                thisUser = RegisteredCoaches.Find(user =>
                user.Name.Equals(searchUserName));
            }

            // Stop the method if user doesn't exist
            if (thisUser == null)
            {
                SpectreGeneric.PrintMessagePrompt("User could not be find!", "red");
                return;
            }

            // Generate the reverse salt based on the user ID
            string reverseSalt = PasswordEncryptor("#" + thisUser.Id.ToString());

            // Decrypt the stored password
            string decryptedWithSalt = PasswordDecrypt(thisUser.Password);

            // Remove encrypted salt from password
            string decryptedPassword = decryptedWithSalt.Substring(0, decryptedWithSalt.Length - reverseSalt.Length);

            Console.Clear();
            Console.WriteLine("Your password is: " + decryptedPassword);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        public static User LoadLogInMenu()
        {
            User thisUser = null;

            // Welcome player
            SpectreGeneric.PresentTopTitle("LOG-IN", "white", "grey");
            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[#705050]Log-in to a pre-existing account or register a new account[/]")
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(LogInMenuChoices));

            switch (choice)
            {
                case "LOG-IN":
                    Console.Clear();
                    thisUser = LogInAccount();
                    break;
                case "REGISTER":
                    Console.Clear();
                    RegisterAccount();
                    break;
                case "FORGOT PASSWORD":
                    Console.Clear();
                    ForgotPassword();
                    break;
                case "ERASE ACCOUNT":
                    Console.Clear();
                    EraseAccount();
                    break;
                case "[grey]DEBUG[/]":
                    Console.Clear();
                    RunDebug();
                    break;
                case "EXIT APPLICATION":
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("No choice, return...I dunno!");
                    break;
            }

            Console.Clear();
            return thisUser;
        }

        private static void RunDebug()
        {
            Console.CursorVisible = false;
            string ourSecretPassword = "flyhigh";
            string message = "[red]Only authorized staff is allowed from this point-- " +
                $"\nPlease enter the Secret Password to confirm your identity...[/]";
            AnsiConsole.MarkupLine(message);
            AnsiConsole.Markup("[grey]Enter Password: [/]");

            string entry = ReadHiddenPasswordWithEsc(out bool cancelled);

            if (entry != ourSecretPassword)
            {
                SpectreGeneric.PrintMessagePrompt("Incorrect password! Booting back to menu!");
                return;
            }

            RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json");

            AnsiConsole.MarkupLine("[grey]----- REGISTERED USERS DEBUG INFO -----[/]");
            foreach (User user in RegisteredUsers)
            {
                Console.WriteLine(user.ReturnUserInformation());
            }
            AnsiConsole.MarkupLine("[grey]----- REGISTERED COACHES DEBUG INFO -----[/]");
            foreach (Coach coach in RegisteredCoaches)
            {
                Console.WriteLine(coach.ReturnUserInformation());
            }
            Console.ReadLine();
            Console.Clear();
        }

        public static User LogInAccount()
        {
            Console.CursorVisible = false;

            // Declare user or coach as null to look for possible matches
            User thisUser = null;
            Coach thisCoach = null;

            while (true)
            {
                AnsiConsole.Markup($"[grey]Please enter your Username: [/]");
                string nameInput = ReadLineWithEsc(out bool cancelled); // ReadLineWithEsc is our own method, it also outs a bool that cancels the process on ESC

                // If ESC was pressde in the ReadLineWithEsc() method, cancelled will be true and this triggers
                if (cancelled)
                {
                    SpectreGeneric.PrintMessagePrompt("Log-in Cancelled.", "red");
                    return null;
                }
                
                // Look up username with LINQ
                thisUser = RegisteredUsers
                    .FirstOrDefault(user => string.Equals(user.Name, nameInput, StringComparison.OrdinalIgnoreCase));

                // If no user is found, look at the coach list too with LINQ
                if (thisUser == null)
                {
                   thisCoach = RegisteredCoaches
                                        .FirstOrDefault(user => string.Equals(user.Name, nameInput, StringComparison.OrdinalIgnoreCase));
                }

                // If neither exists, inform the user and go back to loop start
                if (thisUser == null && thisCoach == null)
                {
                    SpectreGeneric.PrintMessagePrompt("Username doesn't exist, please try again!", "red");
                    continue; // Start loop over
                }

                break; // Found username
            }

            while (true)
            {
                AnsiConsole.Markup($"[grey]Please enter your Password : [/]");
                string passwordInput = ReadHiddenPasswordWithEsc(out bool cancelled);
                
                if (cancelled)
                {
                    SpectreGeneric.PrintMessagePrompt("Log-in Cancelled.", "red");
                    return null;
                }

                // Apply salt
                string salt;

                if (thisCoach != null)
                {
                    salt = "#" + thisCoach.OriginalId.ToString();
                }
                else
                {
                    salt = "#" + thisUser.Id.ToString();
                }
                string saltedPasswordInput = passwordInput + salt;

                // Encrypt
                string encryptedInput = PasswordEncryptor(saltedPasswordInput);
             
                if (thisCoach != null)
                {
                    if (thisCoach.Password != encryptedInput)
                    {
                        SpectreGeneric.PrintMessagePrompt("Incorrect password, please try again!", "red");
                        continue;
                    }
                    return thisCoach;
                }
                else
                {
                    if (thisUser.Password != encryptedInput)
                    {
                        SpectreGeneric.PrintMessagePrompt("Incorrect password, please try again!", "red");
                        continue;
                    }

                    return thisUser;
                }
             
            }

            SpectreGeneric.PrintMessagePrompt($"Loading main menu with user {thisUser.Name}");

        }
        public static void RegisterAccount()
        {
            //Ask user for their prefered password
            //Must be 6 symbols at least, 1 special character, 1 caps and one low
            // Confirm if the password is okay, specter console

            //Inform user of success or not
            //If succeeded, save to json as a User class

            Console.CursorVisible = false;

            // Declare a new user object to be registered
            User newUser = new User();

            //Ask user for what user name they would wish to have
            //Look in Json files if there is an account with the same name -- if there is we can suggest the same name but with numbers in the end (2 random numbers)
            // Confirm if its okay with yes or no, Specter Console
            while (true)
            {
                Console.Write("Please enter your preferred username: ");
                string newName = ReadLineWithEsc(out bool cancelled);
                if (cancelled)
                {
                    SpectreGeneric.PrintMessagePrompt("Registration cancelled.", "red");
                    return;
                }

                if (RegisteredUsers.Any(user => string.Equals(user.Name, newName, StringComparison.OrdinalIgnoreCase)) 
                    || RegisteredCoaches.Any(user => string.Equals(user.Name, newName, StringComparison.OrdinalIgnoreCase)))
                {
                    SpectreGeneric.PrintMessagePrompt("Username already exists, please choose another one!", "yellow");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(newName))
                {
                    SpectreGeneric.PrintMessagePrompt("Username cannot be empty or whitespace, please choose another one!", "red");
                    continue;
                }
                else
                {
                    var confirm = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title($"Is the username '{newName}' okay?")
                            .AddChoices("Yes", "No")
                    );

                    if (confirm == "Yes")
                    {
                        newUser.Name = newName;
                        SpectreGeneric.PrintMessagePrompt($"Username set to: {newUser.Name}", "green");
                        break;
                    }
                }
            }
            while (true)
            {
                AnsiConsole.MarkupLine($"[grey]Please set a password for user {newUser.Name}[/]");
                AnsiConsole.Markup("Enter a password: ");
                string newPassword = ReadLineWithEsc(out bool cancelled);

                if (cancelled)
                {
                    SpectreGeneric.PrintMessagePrompt("Registration Cancelled.", "red");
                    return;
                }

                // Check conditions for password vailibility
                // Preset to false, if all of them turn out true after the foreach loop, then the password is accepted
                bool hasLower = false;
                bool hasUpper = false;
                bool hasSpecial = false;
                int minLength = 6;
                int currentLength = 0;

                foreach (char characeter in newPassword)
                {
                    if (char.IsLower(characeter))
                        hasLower = true;
                    else if (char.IsUpper(characeter))
                        hasUpper = true;
                    else if (!char.IsLetterOrDigit(characeter))
                        hasSpecial = true;
                    // Counts how many number of chars in newPassword at each iteration 
                    currentLength++;
                }

                // If all conditions are met:
                if (hasLower && hasUpper && hasSpecial && currentLength >= minLength)
                {
                    var yesOrNo = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                    .Title($"Is the password {newPassword} okay?")
                    .AddChoices(
                    "Yes", "No"
                    )
                    );


                    if (yesOrNo == "Yes")
                    {
                        // We create our own custom salt, it takes the hashtag and combines it with
                        // the newUsers ID (which in this case it its position in the RegisterUser list count)
                        int newId = 0;

                        // Extract all existing IDs into a list for fast lookup
                        List<int> usedIds = new List<int>(RegisteredUsers.Select(used => used.Id));

                        // Find the first free ID, starting from 0 and increment. As long as it finds userd IDs it keeps going
                        while (usedIds.Contains(newId))
                        {
                            newId++;
                        }

                        // Get the increment it lands on after the latest contain and use it
                        newUser.Id = newId;

                        string salt = ("#" + newId);
                        // Password with added salt set:
                        string passwordWithSalt = (newPassword + salt);
                        // Encrypt password with salt:
                        newUser.Password = PasswordEncryptor(passwordWithSalt);
                        // Inform player of success:
                        SpectreGeneric.PrintMessagePrompt($"Your password has been set to: {newPassword}", "green");
                        break;
                    }
                    else if (yesOrNo == "No")  // Tries again
                    {
                        SpectreGeneric.PrintMessagePrompt("Please try again!", "yellow");
                        continue;
                    }
                }
                else  // One of the conditions aren't met
                {
                    SpectreGeneric.PrintMessagePrompt("Password must contain at least one lowercase letter, " +
                                                      "one upper case, one special character AND be 6 characters long!", "red");
                    continue;
                }

            }

            // Hand out a user ID, we made it so that it looks for the lowester possible ID
            if (RegisteredUsers.Count() == 0)
                newUser.Id = 0;
            else
            {
                int newId = 0;

                // Extract all existing IDs into a list for fast lookup
                List<int> usedIds = new List<int>(RegisteredUsers.Select(user => user.Id));

                // Find the first free ID starting from 0
                while (usedIds.Contains(newId))
                {
                    newId++;
                }

                newUser.Id = newId;
            }

            RegisteredUsers.Add(newUser);
            JsonHandeler.SaveJson(RegisteredUsers, "registeredUsers.json");
        }

        // Looks at Json list and erases chosen account
        public static void EraseAccount()
        {
            Console.CursorVisible = false;

            Console.Write("Please enter the username of the password you wish to Delete: ");

            string readDelName = Console.ReadLine();

            User userToDelete = RegisteredUsers.Find(user => string.Equals(user.Name,
                        readDelName, StringComparison.OrdinalIgnoreCase));

            // If there isn't a User named this,  handle coaches specifically
            if (userToDelete == null)
            {
                Coach coachToDelete = RegisteredCoaches.Find(user => string.Equals(user.Name,
                                      readDelName, StringComparison.OrdinalIgnoreCase));

                if(coachToDelete == null)
                {
                    SpectreGeneric.PrintMessagePrompt("Username not found-- No actions were taken!", "red");
                    return;
                }

                if (RegisteredCoaches.Contains(coachToDelete))
                {
                    var yesOrNo = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
              .Title($"[yellow]Coach {coachToDelete.Name} and their proceeding Team will be deleted-- is this okay?[/]")
              .AddChoices(
              "Yes", "No"
              )
              );

                    if (yesOrNo == "Yes")
                    {
                        var yesOrNo2 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                   .Title("[red]This action cannot be undone if you proceed, the Coach and Team Status will be completely deleted—- are you sure?[/]")
                   .AddChoices(
                   "Yes", "No"
                   )
                   );

                        if (yesOrNo2 == "Yes")
                        {

                            Console.Write($"Please confirm by entering {coachToDelete.Name}'s password: ");
                            string passwordInput = ReadHiddenPasswordWithEsc(out bool cancelled);

                            // set salt
                            string salt = ("#" + coachToDelete.OriginalId.ToString());

                            // Append the reverse salt to the input before encrypting
                            string saltedPasswordInput = passwordInput + salt;

                            // Encrypt the salted password input
                            string encryptedInput = PasswordEncryptor(saltedPasswordInput);

                            if (coachToDelete.Password == encryptedInput)
                            {
                                RegisteredCoaches.Remove(coachToDelete);
                                JsonHandeler.SaveJson(RegisteredCoaches, "registeredCoaches.json");
                                // Also remove their team from the allTeams list
                                MatchGenerator.AllTeams.Remove(coachToDelete.CoachTeam);
                                JsonHandeler.SaveJson(MatchGenerator.AllTeams, "allteams.json");
                                SpectreGeneric.PrintMessagePrompt($"Coach {coachToDelete.Name}'s account and Team have been successfully deleted!", "green");
                            }
                            else
                            {
                                SpectreGeneric.PrintMessagePrompt("Wrong password-- No actions were taken on the account!", "red");
                            }
                        }
                        else if (yesOrNo2 == "No")
                        {
                            SpectreGeneric.PrintMessagePrompt("No actions were taken on the account!", "yellow");
                            return;
                        }
                    }
                    else if (yesOrNo == "No")
                    {
                        SpectreGeneric.PrintMessagePrompt("No actions were taken on the account!", "yellow");
                        return;
                    }
                    else
                    {
                        SpectreGeneric.PrintMessagePrompt("Username not found-- No actions were taken!", "red");
                    }
                }
            }
            else
            {

                if (RegisteredUsers.Contains(userToDelete))
                {
                    var yesOrNo = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                   .Title($"[yellow]{userToDelete.Name} will be deleted-- is this okay?[/]")
                   .AddChoices(
                   "Yes", "No"
                   )
                   );

                    if (yesOrNo == "Yes")
                    {
                        var yesOrNo2 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                   .Title("[red]This action cannot be undone if you proceed—- are you sure?[/]")
                   .AddChoices(
                   "Yes", "No"
                   )
                   );

                        if (yesOrNo2 == "Yes")
                        {

                            Console.Write($"Please confirm by entering {userToDelete.Name}'s password: ");
                            string passwordInput = ReadHiddenPasswordWithEsc(out bool cancelled);
                            if(cancelled)
                            {
                                SpectreGeneric.PrintMessagePrompt("Wrong password-- No actions were taken on the account!", "red");
                                return;
                            }

                            // set salt
                            string salt = ("#" + userToDelete.Id.ToString());

                            // Append the reverse salt to the input before encrypting
                            string saltedPasswordInput = passwordInput + salt;

                            // Encrypt the salted password input
                            string encryptedInput = PasswordEncryptor(saltedPasswordInput);

                            if (userToDelete.Password == encryptedInput)
                            {
                                RegisteredUsers.Remove(userToDelete);
                                JsonHandeler.SaveJson(RegisteredUsers, "registeredUsers.json");
                                SpectreGeneric.PrintMessagePrompt($"{userToDelete.Name}'s account has been successfully deleted!", "green");
                            }
                            else
                            {
                                SpectreGeneric.PrintMessagePrompt("Wrong password-- No actions were taken on the account!", "red");
                            }
                        }
                        else if (yesOrNo2 == "No")
                        {
                            SpectreGeneric.PrintMessagePrompt("No actions were taken on the account!", "yellow");
                            return;
                        }
                    }
                    else if (yesOrNo == "No")
                    {
                        SpectreGeneric.PrintMessagePrompt("No actions were taken on the account!", "yellow");
                        return;
                    }
                }
                else
                {
                    SpectreGeneric.PrintMessagePrompt("Username not found-- No actions were taken!", "red");
                }
            }

        }

        public static string ReadLineWithEsc(out bool cancelled)
        {
            // This does what the ReadHiddenPassword() method does without hidden char string but also allows for
            // You can press ESC to cancel your inputs and return to the log-in screen

            // Add substrings into an empty string object
            string stringPoll = "";
            // Make a new key state reader
            ConsoleKeyInfo keyRead;
            // Cacnel bool for menu naviagation
            cancelled = false;

            bool isReadingString = true;
            while (isReadingString)
            {
                // Read keys, if true it doesn't show the input by the user
                keyRead = Console.ReadKey(true);

                if (keyRead.Key == ConsoleKey.Escape)
                {
                    cancelled = true;
                    return null;
                }
                // Save the key read to stringPoll and show the char each time as "★"
                if (!char.IsControl(keyRead.KeyChar))
                {
                    stringPoll += keyRead.KeyChar;
                    Console.Write(keyRead.KeyChar);
                }
                // Erase char in password if the lenght is 1 and up
                else if (keyRead.Key == ConsoleKey.Backspace && stringPoll.Length >= 1)
                {
                    stringPoll = stringPoll.Substring(0, stringPoll.Length - 1);
                    // \b is a back space that moves the cursor in Console back,
                    // makes a empty space (in other words replace the previous char with empty),
                    // and sets another step back to be at the position of the char before it to write ahead
                    AnsiConsole.Markup("\b \b");
                }
                // Press enter to confirm the current string
                else if (keyRead.Key == ConsoleKey.Enter)
                {
                    AnsiConsole.WriteLine();
                    Console.Clear();
                    isReadingString = false;
                }

            }
            // reutrn the poll of chars -- passworPoll as the new Password
            return stringPoll;
        }
        public static string ReadHiddenPasswordWithEsc(out bool cancelled)
        {
            // Read the password into a string but show the user only "★" for each char

            // Add substrings into an empty string object
            string passwordPoll = "";
            // Make a new key state reader
            ConsoleKeyInfo keyRead;
            // Cacnel bool for menu naviagation
            cancelled = false;

            bool isReadingPassword = true;
            while (isReadingPassword)
            {
                // Read keys, if true it doesn't show the input by the user
                keyRead = Console.ReadKey(true);

                if (keyRead.Key == ConsoleKey.Escape)
                {
                    cancelled = true;
                    return null;
                }
                
                // Save the key read to passwordPoll and show the char each time as "★"
                if (!char.IsControl(keyRead.KeyChar))
                {
                    // this one saves the key to my poll of string
                    passwordPoll += keyRead.KeyChar;
                    // this next one gives a visual write-out to the console
                    AnsiConsole.Markup("[yellow]★[/]");
                }
                // Erase char in password if the lenght is 1 and up
                else if (keyRead.Key == ConsoleKey.Backspace && passwordPoll.Length >= 1)
                {
                    // You replace your string with the current string minus 1 in the length
                    passwordPoll = passwordPoll.Substring(0, passwordPoll.Length - 1);
                    // \b is a back space that moves the cursor in Console back,
                    // makes a empty space (in other words replace the previous char with empty),
                    // and sets another step back to be at the position of the char before it to write ahead
                    AnsiConsole.Markup("\b \b");
                }
                // Press enter to confirm the current string
                else if (keyRead.Key == ConsoleKey.Enter)
                {
                    AnsiConsole.WriteLine();
                    Console.Clear();
                    isReadingPassword = false;
                }

            }
            // reutrn the poll of chars-- passworPoll as the new Password
            return passwordPoll;
        }
    }
}

