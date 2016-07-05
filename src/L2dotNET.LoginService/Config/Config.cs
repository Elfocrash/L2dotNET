using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.LoginService.Config
{
    public sealed class Config
    {
        private static volatile Config _instance;
        private static readonly object SyncRoot = new object();

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Config();
                        }
                    }
                }

                return _instance;
            }
        }

        public ServerConfig ServerConfig;

        public void Initialize()
        {
            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
        }
    }
}