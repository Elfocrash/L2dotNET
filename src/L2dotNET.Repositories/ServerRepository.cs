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
    public class ServerRepository : IServerRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerRepository));

        internal IDbConnection Db;

        public ServerRepository()
        {
            Db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public List<ServerModel> GetServerList()
        {
            try
            {
                return Db.Query<ServerModel>("select * from servers").ToList();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {"GetServerList"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return new List<ServerModel>();
            }
        }

        public List<int> GetPlayersObjectIdList()
        {
            try
            {
                return Db.Query<int>("select obj_Id from characters").ToList();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {"GetPlayersObjectIdList"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return new List<int>();
            }
        }

        public List<AnnouncementModel> GetAnnouncementsList()
        {
            try
            {
                return Db.Query<AnnouncementModel>("select * from announcements").ToList();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {"GetAnnouncementsList"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return new List<AnnouncementModel>();
            }
        }

        public bool CheckDatabaseQuery()
        {
            try
            {
                return Db.Query("SELECT 1").Any();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {"CheckDatabaseQuery"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
            }

            return false;
        }
    }
}