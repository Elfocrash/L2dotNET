using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth
{
    public sealed class Config
    {
        private static volatile Config instance;
        private static object syncRoot = new object();

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Config();
                        }
                    }
                }

                return instance;
            }
        }

        public ServerConfig serverConfig;

        public Config()
        {
            
        }

        public void Initialize()
        {
            serverConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
        }
    }
}
