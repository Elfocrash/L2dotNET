
namespace L2dotNET.Auth.basetemplate
{
    public class L2Account
    {
        public string name;
        public long password;
        public string address;
        public byte serverId;
        public int builder;
        public string lastlogin;
        public string lastAddress;
        public int id;
        public AccountType type;

        // '2009/02/26 18:37:58'
        public string timeend;
        public long points;
        public bool premium;

        public bool validatePassword(string p)
        {
            long summ = 0;
            foreach (char c in p.ToCharArray())
                summ += c.GetHashCode();
            summ *= p.Length * 9;
            string f = summ.ToString() + 32;

            return password == long.Parse(f);
        }
    }

    public enum AccountType
    {
        trial,
        limited,
        normal,
        ultra
    }
}
