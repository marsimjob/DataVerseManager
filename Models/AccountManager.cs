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

        // List of registered users
        public static List<User> RegisteredUsers = new List<User>();

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
            RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json");

            Console.Write("Please write your username: ");
            string searchUserName = Console.ReadLine();

            User thisUser = RegisteredUsers.Find(user =>
                string.Equals(user.Name, searchUserName, StringComparison.OrdinalIgnoreCase));

            // Stop the method if user doesn't exist
            if (thisUser == null)
            {
                Console.WriteLine("Username not found!");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
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
        public static void LoadLogInMenu()
        {
            // Welcome player
            SpectreGeneric.PresentTopTitle("LOG IN", "white", "grey");
            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[#705050]Would you like to log-in to a pre-existing account or register a new account?[/]")
                .AddChoices(
                "Log-In", "Register", "Forgot Password", "Erase Account", "[#303030]Debug[/]", "Exit Application"
                ));

            switch (choice)
            {
                case "Log-In":
                    Console.Clear();
                    LogInAccount();
                    break;
                case "Register":
                    Console.Clear();
                    RegisterAccount();
                    break;
                case "Forgot Password":
                    Console.Clear();
                    ForgotPassword();
                    break;
                case "Erase Account":
                    Console.Clear();
                    EraseAccount();
                    break;
                case "Debug":
                    Console.Clear();
                    RunDebug();
                    break;
                case "Exit Application":
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("No choice, return...I dunno!");
                    break;
            }
        }

        private static void RunDebug()
        {
            string ourSecretPassword = "flyhigh";
            AnsiConsole.MarkupLine($"[red]Only authorized staff is allowed from this point, " +
                $"\nplease enter password to confirm your identity:  [/]");

            string entry = ReadHiddenPasswordWithEsc(out bool cancelled);

            if (entry != ourSecretPassword)
            {
                Console.Clear();
                Console.WriteLine("Incorrect password! Booting back to menu!");
                AnsiConsole.MarkupLine("[grey]Press to continue...[/]");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json");

            foreach (User user in RegisteredUsers)
            {
                Console.WriteLine(user.ReturnUserInformation());
            }
            Console.ReadLine();
            Console.Clear();
        }

        public static void LogInAccount()
        {
            // Load json
            RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json");

            // Declare user on top of the scope
            User thisUser = new User();

            while (true)
            {
                AnsiConsole.Markup($"[grey]Please enter your username: [/]");
                string nameInput = ReadLineWithEsc(out bool cancelled);
                if (cancelled)
                {
                    Console.WriteLine("\nLogin cancelled.");
                    return;
                }

                // Look up username

                thisUser = RegisteredUsers
                    .FirstOrDefault(u => string.Equals(u.Name, nameInput, StringComparison.OrdinalIgnoreCase));

                if (thisUser == null)
                {
                    Console.WriteLine("Username doesn't exist, please try again!");
                    AnsiConsole.MarkupLine("[grey]Press to continue...[/]");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                break; // found username
            }

            bool noInputPassword = true;
            while (true)
            {
                Console.Write("Please enter your password (ESC to cancel): ");
                string passwordInput = ReadHiddenPasswordWithEsc(out bool cancelled);
                if (cancelled)
                {
                    Console.WriteLine("\nLogin cancelled.");
                    return;
                }

                // Append salt
                string salt = "#" + thisUser.Id.ToString();
                string saltedPasswordInput = passwordInput + salt;

                // Encrypt
                string encryptedInput = PasswordEncryptor(saltedPasswordInput);

                if (thisUser.Password != encryptedInput)
                {
                    Console.WriteLine("Password is incorrect, please try again!");
                    continue;
                }
                break; // password correct
            }

            Console.WriteLine($"Loading main menu with user {thisUser.Name}");
        }
        public static void RegisterAccount()
        {
            //Ask user for their prefered password
            //Must be 6 symbols at least, 1 special character, 1 caps and one low
            // Confirm if the password is okay, specter console

            //Inform user of success or not
            //If succeeded, save to json as a User class

            // Declare a new user object to be registered
            User newUser = new User();

            // Prepare our json list
            RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json");

            //Ask user for what user name they would wish to have
            //Look in Json files if there is an account with the same name -- if there is we can suggest the same name but with numbers in the end (2 random numbers)
            // Confirm if its okay with yes or no, Specter Console
            while (true)
            {
                Console.Write("Please enter your preferred username: ");
                string newName = ReadLineWithEsc(out bool cancelled);
                if (cancelled)
                {
                    Console.WriteLine("Registration cancelled.");
                    AnsiConsole.MarkupLine("[grey]Press to continue...[/]");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }

                if (RegisteredUsers.Any(user => string.Equals(user.Name, newName, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Username already exists. Try another one.");
                    AnsiConsole.MarkupLine("[grey]Press to continue...[/]");
                    continue;
                }

                var confirm = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Is the username '{newName}' okay?")
                        .AddChoices("Yes", "No")
                );

                if (confirm == "Yes")
                {
                    newUser.Name = newName;
                    Console.WriteLine($"Username set to: {newName}");
                    break;
                }
            }
            while (true)
            {
                Console.Write("Enter a password: ");
                string newPassword = ReadLineWithEsc(out bool cancelled);
                if (cancelled)
                {
                    Console.WriteLine("Registration cancelled.");
                    AnsiConsole.MarkupLine("[grey]Press to continue...[/]");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }

                // Preset to false, if all of them turn out true after the foreach loop, then the password is accepted
                bool hasLower = false;
                bool hasUpper = false;
                bool hasSpecial = false;

                foreach (char c in newPassword)
                {
                    if (char.IsLower(c))
                        hasLower = true;
                    else if (char.IsUpper(c))
                        hasUpper = true;
                    else if (!char.IsLetterOrDigit(c))
                        hasSpecial = true;
                }
                if (hasLower && hasUpper && hasSpecial)
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

                        // Extract all existing IDs into a HashSet for fast lookup
                        HashSet<int> usedIds = new HashSet<int>(RegisteredUsers.Select(used => used.Id));

                        // Find the first free ID starting from 0
                        while (usedIds.Contains(newId))
                        {
                            newId++;
                        }

                        newUser.Id = newId;

                        string salt = ("#" + newId);
                        // Password with added salt set:
                        string passwordWithSalt = (newPassword + salt);
                        // Encrypt password with salt:
                        newUser.Password = PasswordEncryptor(passwordWithSalt);
                        // Inform player of success:
                        AnsiConsole.WriteLine($"Your password has been set to: {newPassword}");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    }
                    else if (yesOrNo == "No")
                    {
                        // Tries again
                        AnsiConsole.WriteLine($"Then try again...");
                        Console.ReadLine();
                        Console.Clear();
                        continue;
                    }
                }
                else
                {
                    Console.Write("Password must have at least one lowercase, one uppercase and one special character!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

            }
            if (RegisteredUsers.Count() == 0)
                newUser.Id = 0;
            else
            {
                int newId = 0;

                // Extract all existing IDs into a HashSet for fast lookup
                HashSet<int> usedIds = new HashSet<int>(RegisteredUsers.Select(u => u.Id));

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

        public static void EraseAccount()
        {
            RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json");
            Console.Write("Please enter the username of the password you wish to Delete: ");
            string readDelName = Console.ReadLine();
            
            User userToDelete = RegisteredUsers.Find(user => string.Equals(user.Name,
                        readDelName, StringComparison.OrdinalIgnoreCase));

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
               .Title("[red]This action cannot be undone if you proceed—are you sure?[/]")
               .AddChoices(
               "Yes", "No"
               )
               );

                    if (yesOrNo2 == "Yes")
                    {

                        Console.Write($"Please confirm by entering {userToDelete.Name}'s password: ");
                        string passwordInput = ReadHiddenPasswordWithEsc(out bool cancelled);

                        // set salt
                        string salt = ("#" + userToDelete.Id.ToString());

                        // Append the reverse salt to the input before encrypting
                        string saltedPasswordInput = passwordInput + salt;

                        // Encrypt the salted password input
                        string encryptedInput = PasswordEncryptor(saltedPasswordInput);

                        if (userToDelete.Password == encryptedInput)
                        {
                            Console.WriteLine($"{userToDelete.Name}'s account has bee successfully deleted!");
                            RegisteredUsers.Remove(userToDelete);
                            JsonHandeler.SaveJson(RegisteredUsers, "registeredUsers.json");
                        }
                        else
                        {
                            Console.WriteLine("Wrong password-- No actions were taken on the account!");
                        }
                    }
                    else if (yesOrNo2 == "No")
                    {
                            Console.WriteLine("No actions were taken on the account!");
                            return;
                    }
                }
            else if(yesOrNo == "No")
            {
                    Console.WriteLine("No actions were taken on the account!");
                    return;
            }
            }
            else
            {
                Console.WriteLine("The user was not found!");
            }
        }
        public static string ReadLineWithEsc(out bool cancelled)
        {
            // This does what the ReadHiddenPassword() method does without hidden char string but also allows for
            // Escaping the code if you press wrong options in the menu

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
                    passwordPoll += keyRead.KeyChar;
                    AnsiConsole.Markup("[yellow]★[/]");
                }
                // Erase char in password if the lenght is 1 and up
                else if (keyRead.Key == ConsoleKey.Backspace && passwordPoll.Length >= 1)
                {
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
            // reutrn the poll of chars -- passworPoll as the new Password
            return passwordPoll;
        }
    }
}

