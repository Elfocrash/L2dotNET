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
        private static readonly ILog log = LogManager.GetLogger(typeof(CheckRepository));

        internal IDbConnection db;

        private const int PING_TIMEOUT_MS = 3000;
        private const int PING_RETRY_ATTEMPTS = 5;
        private const int MYSQL_SERVICE_START_TIMEOUT_MS = 3000;
        private const int MYSQL_SERVICE_RETRY_ATTEMPTS = 5;
        private const string MYSQL_SERVICE_NAME = "MySQL";

        private readonly string host;
        private readonly string database;

        public CheckRepository()
        {
            db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());

            MySqlConnectionStringBuilder connStrBuilder = new MySqlConnectionStringBuilder(db.ConnectionString);
            host = connStrBuilder.Server;
            database = connStrBuilder.Database;
        }

        public bool PreCheckRepository()
        {
            if (CheckDatabaseHostPing())
                if (CheckMySQLService())
                    if (CheckDatabaseQuery())
                        return true;

            return false;
        }

        private bool CheckDatabaseHostPing()
        {
            log.Info("Checking ping to database host...");

            bool isHostPinging = HostCheck.IsPingSuccessful(host, PING_TIMEOUT_MS);

            for (int i = 1; !isHostPinging && (i <= PING_RETRY_ATTEMPTS); i++)
            {
                log.Error($"Ping to database host '{host}' has FAILED!");
                log.Warn($"Retrying to ping...Retry attempt: {i}.");

                isHostPinging = HostCheck.IsPingSuccessful(host, PING_TIMEOUT_MS);

                if (isHostPinging)
                    break;
            }

            if (isHostPinging)
                log.Info($"Ping to database host '{host}' was SUCCESSFUL!");
            else
                log.Error($"Ping to database host '{host}' has FAILED!");

            return isHostPinging;
        }

        private bool CheckMySQLService()
        {
            if (HostCheck.IsLocalIPAddress(host))
            {
                log.Info("Database host running at localhost.");
                log.Info("Checking if MySQL Service is running at localhost...");

                if (!HostCheck.ServiceExists(MYSQL_SERVICE_NAME))
                {
                    log.Error("MySQL Service does not exist at localhost Windows Services!");
                    return false;
                }

                bool isMySQLServiceRunning = HostCheck.IsServiceRunning(MYSQL_SERVICE_NAME);

                for (int i = 1; !isMySQLServiceRunning && (i <= MYSQL_SERVICE_RETRY_ATTEMPTS); i++)
                {
                    log.Error("MySQL Service was not found running at localhost!");
                    log.Warn($"Trying to start MySQL service...Retry attempt: {i}.");

                    HostCheck.StartService(MYSQL_SERVICE_NAME, MYSQL_SERVICE_START_TIMEOUT_MS);

                    isMySQLServiceRunning = HostCheck.IsServiceRunning(MYSQL_SERVICE_NAME);

                    if (isMySQLServiceRunning)
                    {
                        log.Info("MySQL Service started!");
                        break;
                    }
                }

                if (isMySQLServiceRunning)
                    log.Info("MySQL Service running at localhost.");
                else
                    log.Error("MySQL Service was not found running at localhost!");

                return isMySQLServiceRunning;
            }

            log.Info("Database host NOT running at localhost. MySQL Service check skipped.");
            return true;
        }

        private bool CheckDatabaseQuery()
        {
            log.Info("Checking if query to database works...");

            bool isQuerySuccessful = TryQueryDatabase();

            if (isQuerySuccessful)
                log.Info($"Query to database '{database}' was SUCCESSFUL!");
            else
                log.Error($"Query to database '{database}' has FAILED!");

            return isQuerySuccessful;
        }

        private bool TryQueryDatabase()
        {
            try
            {
                return db.Query("SELECT 1").Any();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: {"TryQueryDatabase"}. Message: '{ex.Message}' (Error Number: '{ex.Number}')");
            }

            return false;
        }
    }
}