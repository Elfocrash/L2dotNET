using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using log4net;
using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using MySql.Data.MySqlClient;

namespace L2dotNET.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountRepository));

        internal IDbConnection db;

        public AccountRepository()
        {
            this.db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public AccountModel GetAccountByLogin(string login)
        {
            try
            {
                return this.db.Query<AccountModel>("select Login,Password,LastActive,access_level as AccessLevel,LastServer from accounts where login=@login", new { login = login }).FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: {"GetAccountByLogin"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return null;
            }
        }

        public AccountModel CreateAccount(string login, string password)
        {
            try
            {
                this.db.Execute("insert into accounts (Login,Password,LastActive,access_level,LastServer) Values (@login,@pass,@lastactive,@access,@lastServer)", new { login = login, pass = password, lastactive = DateTime.Now.Ticks, access = 0, lastServer = 1 }); //to be edited

                AccountModel accModel = new AccountModel() { Login = login, Password = password, LastActive = DateTime.Now.Ticks, AccessLevel = 0, LastServer = 1 };

                return accModel;
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: {"CreateAccount"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return null;
            }
        }

        public bool CheckIfAccountIsCorrect(string login, string password)
        {
            try
            {
                return this.db.Query("select distinct 1 from accounts where login=@login AND password=@pass", new { login = login, pass = password }).Any();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: {"CheckIfAccountIsCorrect"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return false;
            }
        }

        public List<int> GetPlayerIdsListByAccountName(string login)
        {
            try
            {
                return this.db.Query<int>("select obj_Id from characters where account_name=@acc", new { acc = login }).ToList();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: {"GetPlayerIdsListByAccountName"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return new List<int>();
            }
        }
    }
}