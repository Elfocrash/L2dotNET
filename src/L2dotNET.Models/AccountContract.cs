namespace L2dotNET.DataContracts
{
    public class AccountContract
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public long LastActive { get; set; }

        public int AccessLevel { get; set; }

        public int LastServer { get; set; }

        public AccountContract()
        {
            Login = string.Empty;
            Password = string.Empty;
            LastActive = 0;
            AccessLevel = 0;
            LastServer = 1;
        }
    }
}