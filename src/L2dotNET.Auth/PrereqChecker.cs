using L2dotNET.Services.Contracts;
using log4net;
using MySql.Data.MySqlClient;
using Ninject;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading;

namespace L2dotNET.LoginService
{
    class PrereqChecker
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PrereqChecker));

        [Inject]
        public IServerService serverService { get { return LoginServer.Kernel.Get<IServerService>(); } }

        private IDbConnection db;

        private const int PING_TIMEOUT = 5000;
        private const int PING_RETRY_ATTEMPTS = 5;
        private const int MYSQL_SERVICE_RETRY_WAIT_TIME_MS = 5000;
        private const int MYSQL_SERVICE_RETRY_ATTEMPTS = 5;

        public PrereqChecker()
        {
            this.db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public bool RunCheckers()
        {
            try
            {
                MySqlConnectionStringBuilder connStrBuilder = new MySqlConnectionStringBuilder(this.db.ConnectionString);

                if (CheckDatabaseHostPing(connStrBuilder.Server))
                    if (IsLocalIPAddress(connStrBuilder.Server))
                    {
                        if (CheckMySQLService())
                            if (CheckDatabaseQuery(connStrBuilder.Database))
                                return true;
                    }
                    else
                    {
                        if (CheckDatabaseQuery(connStrBuilder.Database))
                            return true;
                    }
            }
            catch (Exception ex) { log.Error(ex.Message); }
            return false;
        }

        private bool CheckDatabaseHostPing(string host)
        {
            log.Info($"Checking ping to database host...");
            bool isHostPinging = IsPingSuccessful(host);

            for (int i = 1; !isHostPinging && i <= PING_RETRY_ATTEMPTS; i++)
            {
                log.Error($"Ping to database host '{ host }' has FAILED!");
                log.Warn($"Retrying to ping...Retry attempt: { i }.");

                isHostPinging = IsPingSuccessful(host);

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
            log.Info($"Database host running at localhost.");

            log.Info($"Checking if MySQL Service is running at localhost...");

            bool isMySQLServiceRunning = IsServiceRunning("MySQL");

            for (int i = 1; !isMySQLServiceRunning && i <= MYSQL_SERVICE_RETRY_ATTEMPTS; i++)
            {
                log.Error($"MySQL Service was not found running at localhost!");
                log.Warn($"Retrying to check MySQL Service...Retry attempt: { i }.");

                Thread.Sleep(MYSQL_SERVICE_RETRY_WAIT_TIME_MS);
                isMySQLServiceRunning = IsServiceRunning("MySQL");

                if (isMySQLServiceRunning)
                    break;
            }

            if (isMySQLServiceRunning)
                log.Info($"MySQL Service running at localhost.");
            else
                log.Error($"MySQL Service was not found running at localhost!");

            return isMySQLServiceRunning;
        }

        private bool CheckDatabaseQuery(string databaseName)
        {
            log.Info($"Checking if query to database works...");

            bool isQuerySuccessful = IsDatabaseQuerying();

            if (isQuerySuccessful)
                log.Info($"Query to database '{ databaseName }' was SUCCESSFUL!");
            else
                log.Error($"Query to database '{ databaseName }' has FAILED!");

            return isQuerySuccessful;
        }

        private static bool IsPingSuccessful(string host)
        {
            try { return new Ping().Send(host, PING_TIMEOUT, new byte[1]).Status == IPStatus.Success; }
            catch { }
            return false;
        }
        private static bool IsLocalIPAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private bool IsServiceRunning(string serviceName)
        {
            try { return ServiceController.GetServices().Any(s => s.ServiceName.StartsWith(serviceName) && s.Status == ServiceControllerStatus.Running); }
            catch { }
            return false;
        }

        public bool IsDatabaseQuerying()
        {
            return serverService.CheckDatabaseQuery();
        }

    }
}
