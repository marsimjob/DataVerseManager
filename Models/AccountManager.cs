using DataVerseManager.Services;
using Spectre.Console;

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

            string searchUserName;
            Console.Write("Please write your username: ");
            searchUserName = Console.ReadLine();

            User thisUser = RegisteredUsers.Find(user =>
                string.Equals(user.Name, searchUserName, StringComparison.OrdinalIgnoreCase));

            // Generate the reverse salt based on the user ID
            string reverseSalt = PasswordEncryptor("#" + thisUser.Id.ToString());

            // Decrypt the stored password
            string decryptedWithSalt = PasswordDecrypt(thisUser.Password);

            // Remove encrypted salt from password
            string decryptedPassword;
            // Start at char 0 in the string array and remove the lenght of characeter from reverse salt
            decryptedPassword = decryptedWithSalt.Substring(0, decryptedWithSalt.Length - reverseSalt.Length);

            Console.WriteLine("Your password is: " + decryptedPassword);
        }
        public static void LoadLogInMenu()
        {
            // Welcome player
            AnsiConsole.MarkupLine("[#ffa500]🏀 -- NBA ShowTime 2K26 -- 🏀[/]");
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

            string entry = ReadHiddenPassword();

            if(entry != ourSecretPassword)
            {
                Console.Clear();
                Console.WriteLine("Incorrect password! Booting back to menu!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            RegisteredUsers = JsonHandeler.LoadJson<List<User>>("registeredUsers.json");
            
            foreach(User user in RegisteredUsers)
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

            bool noInputName = true;

            while (noInputName)
            {
                string nameInput;
                // Prompt username input
                Console.Write("Please enter your username: ");
                nameInput = Console.ReadLine();

                // Look at our json and match the input when logging in to our register account
                if (!RegisteredUsers.Any(user => user.Name == nameInput))
                {
                    // If username doesnt exist put us back to the top
                    Console.WriteLine("Username doesn't exist, please try again!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                else
                {
                    try
                    {
                        // Set user object to listed object
                        thisUser = RegisteredUsers.Find(user => string.Equals(user.Name,
                            nameInput, StringComparison.OrdinalIgnoreCase));
                        // Else successfull and exist the loop
                        noInputName = false;
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }
                }
            }
            bool noInputPassword = true;
            while (noInputPassword)
            {

                // Prompt the user to enter their password
                Console.Write("Please enter your password: ");
                string passwordInput = ReadHiddenPassword();

                // set salt
                string salt = ("#" + thisUser.Id.ToString());

                // Append the reverse salt to the input before encrypting
                string saltedPasswordInput = passwordInput + salt;

                // Encrypt the salted password input
                string encryptedInput = PasswordEncryptor(saltedPasswordInput); // Nemo_100#0


                // Compare with the stored password
                if (thisUser.Password != encryptedInput)
                {

                    // If password doesnt exist put us back to the top
                    Console.WriteLine("Password is incorrect, please try again!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                else
                {
                    // Go to menu with this account
                    Console.WriteLine("Loading main menu with user " + thisUser.Name);
                    // Else successfull and exist the loop
                    noInputPassword = false;
                }
            }
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

            bool noNameSet = true;

            while (noNameSet)
            {
                Console.WriteLine("Please enter your prefered username: ");
                string newName = Console.ReadLine();

                if (RegisteredUsers.Any(user => user.Name == newName))
                {
                    Console.WriteLine("Username already exists, please try another username!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                var yesOrNo = AnsiConsole.Prompt(
                 new SelectionPrompt<string>()
                .Title($"Is the name {newName} okay?")
                .AddChoices(
                "Yes", "No"
                )
                );

                if (yesOrNo == "Yes")
                {
                    newUser.Name = newName;
                    AnsiConsole.WriteLine($"Your name has been set to: {newName}");
                    noNameSet = false;
                    Console.ReadLine();
                    Console.Clear();
                }
                else if (yesOrNo == "No")
                {
                    Console.Clear();
                }
            }

            bool noPasswordSet = true;
            while (noPasswordSet)
            {
                Console.Write("Please enter a password you would like to use: ");
                string newPassword = Console.ReadLine();

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
                        HashSet<int> usedIds = new HashSet<int>(RegisteredUsers.Select(u => u.Id));

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
                        noPasswordSet = false;
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else if (yesOrNo == "No")
                    {
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Write("Password must have at least one lowercase, one uppercase and one special character!");
                    Console.ReadLine();
                    Console.Clear();
                }

            }
            if(RegisteredUsers.Count() == 0)
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
                        string passwordInput = ReadHiddenPassword();

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
        public static string ReadHiddenPassword()
        {
            // Read the password into a string but show the user only "★" for each char

            // Add substrings into an empty string object
            string passwordPoll = "";
            // Make a new key state reader
            ConsoleKeyInfo keyRead;

            bool isReadingPassword = true;
            while (isReadingPassword)
            {
                // Read keys, if true it doesn't show the input by the user
                keyRead = Console.ReadKey(true);

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

