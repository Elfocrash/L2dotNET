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

        private static readonly ILog log = LogManager.GetLogger(typeof(ServerRepository));

        internal IDbConnection db;

        public ServerRepository()
        {
            this.db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public List<ServerModel> GetServerList()
        {
            try
            {
                return this.db.Query<ServerModel>("select * from servers").ToList();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: { "GetServerList" }. Message: '{ ex.Message }' (Error Number: '{ ex.Number }')");
                return new List<ServerModel>();
            }
        }

        public List<int> GetPlayersObjectIdList()
        {
            try
            {
                return this.db.Query<int>("select obj_Id from characters").ToList();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: { "GetPlayersObjectIdList" }. Message: '{ ex.Message }' (Error Number: '{ ex.Number }')");
                return new List<int>();
            }
        }

        public List<AnnouncementModel> GetAnnouncementsList()
        {
            try
            {
                return this.db.Query<AnnouncementModel>("select * from announcements").ToList();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: { "GetAnnouncementsList" }. Message: '{ ex.Message }' (Error Number: '{ ex.Number }')");
                return new List<AnnouncementModel>();
            }
        }

        public bool CheckDatabaseQuery()
        {
            try
            {
                return this.db.Query("SELECT 1").Any();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: { "CheckDatabaseQuery" }. Message: '{ ex.Message }' (Error Number: '{ ex.Number }')");
            }
            return false;
        }

    }
}
