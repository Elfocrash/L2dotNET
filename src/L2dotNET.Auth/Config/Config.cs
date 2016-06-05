using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.LoginService.Config
{
    public sealed class Config
    {
        private static volatile Config instance;
        private static readonly object syncRoot = new object();

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

        public Config() { }

        public void Initialize()
        {
            serverConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
        }
    }
}