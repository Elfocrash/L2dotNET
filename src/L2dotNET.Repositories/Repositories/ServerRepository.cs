using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Repositories.Contracts;
using MySql.Data.MySqlClient;

namespace L2dotNET.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        internal IDbConnection Db;

        public ServerRepository()
        {
            Db = new MySqlConnection("Server=127.0.0.1;Database=l2dotnet;Uid=l2dotnet;Pwd=l2dotnet;SslMode=none;");
        }

        public List<int> GetPlayersObjectIdList()
        {
            try
            {
                return Db.Query<int>("select obj_Id from characters").ToList();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(GetPlayersObjectIdList)}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return new List<int>();
            }
        }

        public List<int> GetPlayersItemsObjectIdList()
        {
            try
            {
                return Db.Query<int>("select object_id from items").ToList();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(GetPlayersItemsObjectIdList)}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return new List<int>();
            }
        }

        public List<SpawnlistContract> GetAllSpawns()
        {
            try
            {
                return Db.Query<SpawnlistContract>("select npc_templateid as TemplateId, LocX, LocY, LocZ, Heading, respawn_delay as RespawnDelay, respawn_rand as RespawnRand, PeriodOfDay from spawnlist").ToList();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(GetAllSpawns)}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
                return new List<SpawnlistContract>();
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
                Log.Error($"Method: {nameof(CheckDatabaseQuery)}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
            }

            return false;
        }
    }
}