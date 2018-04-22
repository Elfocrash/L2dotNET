using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using log4net;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Repositories.Utils;
using MySql.Data.MySqlClient;

namespace L2dotNET.Repositories
{
    public class CheckRepository : ICheckRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CheckRepository));

        internal IDbConnection Db;

        private const int PingTimeoutMs = 3000;
        private const int PingRetryAttempts = 5;
        private const int MysqlServiceStartTimeoutMs = 3000;
        private const int MysqlServiceRetryAttempts = 5;
        private const string MysqlServiceName = "MySQL";

        private readonly string _host;
        private readonly string _database;

        public CheckRepository()
        {
            Db = new MySqlConnection("Server = localhost; Database = l2dotnet; Uid = root; Pwd = root; sslmode = none;");

            MySqlConnectionStringBuilder connStrBuilder = new MySqlConnectionStringBuilder(Db.ConnectionString);
            _host = connStrBuilder.Server;
            _database = connStrBuilder.Database;
        }

        public bool PreCheckRepository()
        {
            if (!CheckDatabaseHostPing())
                return false;

            if (!CheckMySqlService())
                return false;

            if (!CheckDatabaseQuery())
                return false;

            return true;
        }

        private bool CheckDatabaseHostPing()
        {
            Log.Info("Checking ping to database host...");

            bool isHostPinging = HostCheck.IsPingSuccessful(_host, PingTimeoutMs);

            for (int i = 1; !isHostPinging && (i <= PingRetryAttempts); i++)
            {
                Log.Error($"Ping to database host '{_host}' has FAILED!");
                Log.Warn($"Retrying to ping...Retry attempt: {i}.");

                isHostPinging = HostCheck.IsPingSuccessful(_host, PingTimeoutMs);

                if (isHostPinging)
                    break;
            }

            if (isHostPinging)
                Log.Info($"Ping to database host '{_host}' was SUCCESSFUL!");
            else
                Log.Error($"Ping to database host '{_host}' has FAILED!");

            return isHostPinging;
        }

        private bool CheckMySqlService()
        {
            if (HostCheck.IsLocalIpAddress(_host))
            {
                Log.Info("Database host running at localhost.");
                Log.Info("Checking if MySQL Service is running at localhost...");

                if (!HostCheck.ServiceExists(MysqlServiceName))
                {
                    Log.Error("MySQL Service does not exist at localhost Windows Services!");
                    return false;
                }

                bool isMySqlServiceRunning = HostCheck.IsServiceRunning(MysqlServiceName);

                for (int i = 1; !isMySqlServiceRunning && (i <= MysqlServiceRetryAttempts); i++)
                {
                    Log.Error("MySQL Service was not found running at localhost!");
                    Log.Warn($"Trying to start MySQL service...Retry attempt: {i}.");

                    HostCheck.StartService(MysqlServiceName, MysqlServiceStartTimeoutMs);

                    isMySqlServiceRunning = HostCheck.IsServiceRunning(MysqlServiceName);

                    if (!isMySqlServiceRunning)
                        continue;

                    Log.Info("MySQL Service started!");
                    break;
                }

                if (isMySqlServiceRunning)
                    Log.Info("MySQL Service running at localhost.");
                else
                    Log.Error("MySQL Service was not found running at localhost!");

                return isMySqlServiceRunning;
            }

            Log.Info("Database host NOT running at localhost. MySQL Service check skipped.");
            return true;
        }

        private bool CheckDatabaseQuery()
        {
            Log.Info("Checking if query to database works...");

            bool isQuerySuccessful = TryQueryDatabase();

            if (isQuerySuccessful)
                Log.Info($"Query to database '{_database}' was SUCCESSFUL!");
            else
                Log.Error($"Query to database '{_database}' has FAILED!");

            return isQuerySuccessful;
        }

        private bool TryQueryDatabase()
        {
            try
            {
                return Db.Query("SELECT 1").Any();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(TryQueryDatabase)}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
            }

            return false;
        }
    }
}