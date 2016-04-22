using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace L2dotNET.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        internal IDbConnection db;

        public AccountRepository()
        {
            this.db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public AccountModel GetAccountByLogin(string login)
        {
            return this.db.Query<AccountModel>("select Login,Password,LastActive,access_level as AccessLevel,LastServer from accounts where login=@login",
                new { login = login }).FirstOrDefault();
        }

        public AccountModel CreateAccount(string login, string password)
        {
            this.db.Execute("insert into accounts (Login,Password,LastActive,access_level,LastServer) Values (@login,@pass,@lastactive,@access,@lastServer)",
                new { login = login, pass = password, lastactive = DateTime.Now.Ticks, access = 0, lastServer = 1 });//to be edited

            AccountModel accModel = new AccountModel()
            {
                Login = login,
                Password = password,
                LastActive = DateTime.Now.Ticks,
                AccessLevel = 0,
                LastServer = 1
            };

            return accModel; 
        }

        public bool CheckIfAccountIsCorrect(string login, string password)
        {
            return this.db.Query<bool>("select count(*) from accounts where login=@login AND password=@pass",
                new { login = login, pass = password }).SingleOrDefault();
        }

        public List<int> GetPlayerIdsListByAccountName(string login)
        {
            return this.db.Query<int>("select obj_Id from characters where account_name=@acc", new { acc = login }).ToList();
        }

    }
}
