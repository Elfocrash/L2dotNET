using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Auth
{
    class Cfg
    {
        public static string SERVER_HOST;
        public static int SERVER_PORT;
        public static int SERVER_PORT_GS;

        public static string DB_HOST, DB_NAME, DB_USER, DB_PASS;

        public static bool AUTO_ACCOUNTS;

        public static void Load()
        {
            ConfigFile server = new ConfigFile(@"config\server.ini");

            SERVER_HOST = server.getProperty("host", "192.168.1.2");
            SERVER_PORT = int.Parse(server.getProperty("port", "2106"));
            SERVER_PORT_GS = int.Parse(server.getProperty("gsport", "2107"));

            DB_HOST = server.getProperty("dbhost", "localhost");
            DB_NAME = server.getProperty("dbname", "rabbit_cauth");
            DB_USER = server.getProperty("dbuser", "root");
            DB_PASS = server.getProperty("dbpass", "root");

            AUTO_ACCOUNTS = bool.Parse(server.getProperty("autoaccounts", "false"));
        }
    }
}
