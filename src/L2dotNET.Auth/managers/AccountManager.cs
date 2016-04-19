using System;
using System.Collections.Generic;
using System.Data;
using L2dotNET.Auth.basetemplate;
using MySql.Data.MySqlClient;

namespace L2dotNET.Auth.data
{
    class AccountManager
    {
        private static AccountManager acm = new AccountManager();
        public static AccountManager getInstance()
        {
            return acm;
        }

        protected SortedList<string, L2Account> _accounts = new SortedList<string, L2Account>();

        public bool accountExists(string user)
        {
            return _accounts.ContainsKey(user.ToLower());
        }

        public AccountManager()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();

            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "SELECT * FROM accounts";
            cmd.CommandType = CommandType.Text;

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                L2Account account = new L2Account();
                account.id = reader.GetInt32("id");
                account.name = reader.GetString("account");
                account.password = reader.GetInt64("password");
                account.serverId = (byte)reader.GetInt32("serverId");
                account.builder = reader.GetInt32("builder");
                account.type = (AccountType)Enum.Parse(typeof(AccountType), reader.GetString("type"));
                account.timeend = reader.GetString("timeEnd");
                account.lastlogin = reader.GetString("lastlogin");
                account.lastAddress = reader.GetString("lastAddress");
                account.premium = reader.GetInt16("premium") == 1;
                account.points = reader.GetInt64("points");
                _accounts.Add(account.name.ToLower(), account);
            }

            reader.Close();
            connection.Close();

            CLogger.extra_info("ACM: loaded " + _accounts.Count + " accounts");
        }

        public L2Account createAccount(string user, string password, string address)
        {
            long summ = 0;
            foreach (char c in password.ToCharArray())
                summ += c.GetHashCode();
            summ *= password.Length * 9;
            string f = summ.ToString() + 32;

            SQL_Block sqb = new SQL_Block("accounts");
            sqb.param("account", user);
            sqb.param("password", f);
            sqb.param("lastlogin", DateTime.Now.ToLocalTime());
            sqb.param("lastAddress", address);
            sqb.sql_insert();

            L2Account account = new L2Account();
            account.name = user;
            account.password = summ;
            account.address = address;
            account.builder = 0;
            account.serverId = 0;
            account.type = AccountType.normal;

            _accounts.Add(user, account);

            CLogger.extra_info("ACM: created user account " + user + ".");
            return account;
        }

        public L2Account get(string username)
        {
            if (!_accounts.ContainsKey(username.ToLower()))
                return null;

            return _accounts[username.ToLower()];
        }

        public void UpdatePremium(string account, byte status, long points)
        {
            L2Account acc = _accounts[account];
            acc.premium = status == 1;
            acc.points = points;

            SQL_Block sqb = new SQL_Block("accounts");
            sqb.param("premium", status);
            sqb.param("points", points);
            sqb.where("account", account);
            sqb.sql_update();
        }
    }
}
