
using System;
namespace L2dotNET.Game
{
    class Cfg
    {
        public static string SERVER_HOST;
        public static short SERVER_PORT;

        public static string DB_HOST, DB_NAME, DB_USER, DB_PASS;
        public static int DB_PORT;

        public static int max_buffs;

        public static string[] param = { "server", "options", "rates" };

        public static int rate_quest_exp, rate_quest_sp, rate_quest_adena;

        public static chatoptions chat_shout, chat_trade;
        public static int world_visble_range_surface;
        public static int world_visble_height_surface;
        public static int world_visble_range_siege;
        public static int world_visble_height_siege;
        public static int world_visble_range_dungeon;
        public static int world_visble_height_dungeon;

        public static bool autoloot;
        public static string AUTH_HOST;
        public static short AUTH_PORT;
        public static string auth_code;
        public static short max_players;
        public static byte gmonly;
        public static byte test;
        public static bool MinigameRankEnabled;

        public static void init(string val)
        {
            if (val.Equals("all"))
            {
                foreach (string type in param)
                {
                    load(type);
                }
            }
            else
            {
                if (!val.Equals(""))
                {
                    load(val);
                }
            }
        }


        public static void load(string param)
        {
            ConfigFile options;
            switch (param)
            {
                case "server":
                    {
                        options = new ConfigFile(@"config\server.ini");

                        SERVER_HOST = options.getProperty("host", "192.168.1.2");
                        SERVER_PORT = short.Parse(options.getProperty("port", "7777"));

                        DB_HOST = options.getProperty("dbhost", "localhost");
                        DB_NAME = options.getProperty("dbname", "rabbit_cgame");
                        DB_USER = options.getProperty("dbuser", "root");
                        DB_PASS = options.getProperty("dbpass", "root");
                        DB_PORT = int.Parse(options.getProperty("dbport", "3306"));

                        AUTH_HOST = options.getProperty("auth_host", "192.168.1.2");
                        AUTH_PORT = short.Parse(options.getProperty("auth_port", "2107"));

                        auth_code = options.getProperty("auth_code", "<none>");
                        max_players = short.Parse(options.getProperty("max_players", "100"));

                        gmonly = byte.Parse(options.getProperty("status_gmonly", "0"));
                        test = byte.Parse(options.getProperty("status_testserver", "0"));
                    }
                    break;

                case "options":
                    {
                        options = new ConfigFile(@"config\options.ini");
                        max_buffs = int.Parse(options.getProperty("max_buffs", "24"));
                        chat_shout = (chatoptions)Enum.Parse(typeof(chatoptions), options.getProperty("chat_shout", "Default"));
                        chat_trade = (chatoptions)Enum.Parse(typeof(chatoptions), options.getProperty("chat_trade", "Default"));

                        world_visble_range_surface = int.Parse(options.getProperty("visible_surface_range", "4000"));
                        world_visble_height_surface = int.Parse(options.getProperty("visible_surface_height", "1600"));
                        world_visble_range_siege = int.Parse(options.getProperty("visible_surface_siege_range", "2700"));
                        world_visble_height_siege = int.Parse(options.getProperty("visible_surface_siege_height", "1000"));
                        world_visble_range_dungeon = int.Parse(options.getProperty("visible_surface_dungeon_range", "3000"));
                        world_visble_height_dungeon = int.Parse(options.getProperty("visible_surface_dungeon_height", "500"));

                        autoloot = bool.Parse(options.getProperty("autoloot", "True"));

                        MinigameRankEnabled = bool.Parse(options.getProperty("MinigameRankEnabled", "True"));
                    }
                    break;

                case "rates":
                    {
                        options = new ConfigFile(@"config\rates.ini");
                        rate_quest_exp = int.Parse(options.getProperty("quest_exp", "1"));
                        rate_quest_sp = int.Parse(options.getProperty("quest_sp", "1"));
                        rate_quest_adena = int.Parse(options.getProperty("quest_adena", "1"));
                    }
                    break;
            }
        }

        public enum chatoptions
        {
            Default, Disabled, GMonly, Global
        }
    }
}
