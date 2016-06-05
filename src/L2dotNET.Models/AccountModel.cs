namespace L2dotNET.Models
{
    public class AccountModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public long LastActive { get; set; }

        public int AccessLevel { get; set; }

        public int LastServer { get; set; }

        public AccountModel()
        {
            Login = string.Empty;
            Password = string.Empty;
            LastActive = 0;
            AccessLevel = 0;
            LastServer = 1;
        }
    }
}