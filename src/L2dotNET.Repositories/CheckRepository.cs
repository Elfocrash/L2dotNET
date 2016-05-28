using Dapper;
using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using log4net;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Net.NetworkInformation;
using L2dotNET.Repositories.Utils;

namespace L2dotNET.Repositories
{
    public class CheckRepository : ICheckRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CheckRepository));

        internal IDbConnection db;

        private const int PING_TIMEOUT = 5000;
        private const int PING_RETRY_ATTEMPTS = 5;
        private const int MYSQL_SERVICE_RETRY_WAIT_TIME_MS = 5000;
        private const int MYSQL_SERVICE_RETRY_ATTEMPTS = 5;

        private readonly string host;
        private readonly string database;

        public CheckRepository()
        {
            this.db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());

            MySqlConnectionStringBuilder connStrBuilder = new MySqlConnectionStringBuilder(this.db.ConnectionString);
            this.host = connStrBuilder.Server;
            this.database = connStrBuilder.Database;
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
            log.Info($"Checking ping to database host...");
            bool isHostPinging = HostCheck.IsPingSuccessful(host, PING_TIMEOUT);

            for (int i = 1; !isHostPinging && i <= PING_RETRY_ATTEMPTS; i++)
            {
                log.Error($"Ping to database host '{ host }' has FAILED!");
                log.Warn($"Retrying to ping...Retry attempt: { i }.");

                isHostPinging = HostCheck.IsPingSuccessful(host, PING_TIMEOUT);

                if (isHostPinging)
                    break;
            }

            if (isHostPinging)
                log.Info($"Ping to database host '{ host }' was SUCCESSFUL!");
            else
                log.Error($"Ping to database host '{ host }' has FAILED!");

            return isHostPinging;
        }

        private bool CheckMySQLService()
        {
            if (HostCheck.IsLocalIPAddress(host))
            {
                log.Info($"Database host running at localhost.");

                log.Info($"Checking if MySQL Service is running at localhost...");

                bool isMySQLServiceRunning = HostCheck.IsServiceRunning("MySQL");

                for (int i = 1; !isMySQLServiceRunning && i <= MYSQL_SERVICE_RETRY_ATTEMPTS; i++)
                {
                    log.Error($"MySQL Service was not found running at localhost!");
                    log.Warn($"Retrying to check MySQL Service...Retry attempt: { i }.");

                    Thread.Sleep(MYSQL_SERVICE_RETRY_WAIT_TIME_MS);
                    isMySQLServiceRunning = HostCheck.IsServiceRunning("MySQL");

                    if (isMySQLServiceRunning)
                        break;
                }

                if (isMySQLServiceRunning)
                    log.Info($"MySQL Service running at localhost.");
                else
                    log.Error($"MySQL Service was not found running at localhost!");

                return isMySQLServiceRunning;
            }
            else
            {
                log.Info($"Database host NOT running at localhost. MySQL Service check skipped.");
                return true;
            }
        }

        private bool CheckDatabaseQuery()
        {
            log.Info($"Checking if query to database works...");

            bool isQuerySuccessful = TryQueryDatabase();

            if (isQuerySuccessful)
                log.Info($"Query to database '{ database }' was SUCCESSFUL!");
            else
                log.Error($"Query to database '{ database }' has FAILED!");

            return isQuerySuccessful;
        }

        private bool TryQueryDatabase()
        {
            try
            {
                return this.db.Query("SELECT 1").Any();
            }
            catch (MySqlException ex)
            {
                log.Error($"Method: { "TryQueryDatabase" }. Message: '{ ex.Message }' (Error Number: '{ ex.Number }')");
            }
            return false;
        }
    }
}
