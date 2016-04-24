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
    public class ServerRepository : IServerRepository
    {
        internal IDbConnection db;

        public ServerRepository()
        {
            this.db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public List<ServerModel> GetServerList()
        {
            return this.db.Query<ServerModel>("select * from servers").ToList();
        }

        public List<int> GetPlayersObjectIdList()
        {
            return this.db.Query<int>("select obj_Id from characters").ToList();
        }

        public List<AnnouncementModel> GetAnnouncementsList()
        {
            return this.db.Query<AnnouncementModel>("select * from announcements").ToList();
        }
    }
}
