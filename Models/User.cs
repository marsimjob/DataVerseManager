using DataVerseManager.Models;

public class User
{
    // Attributes
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public Wallet UserWallet = new Wallet();

    // Constructor
    public User()
    {
        UserWallet.GetMoney(1000);
    }

    // Methods
    public string ReturnUserInformation()
    {
        string salt = ("c" + Id.ToString());
        string info =   $"ID: #{Id} " +
                        $"|| Name: {Name} " +
                        $"|| Wallet Balance: {UserWallet.ReturnWalletBalance()} " +
                        $"|| Decoded Password: {AccountManager.PasswordDecrypt(Password)
                                                  .Substring(0, Password.Length - salt.Length)}";
        return info;
    }

}
