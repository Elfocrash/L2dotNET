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
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new Config();
                }

                return _instance;
            }
        }

        public ServerConfig ServerConfig;

        //TODO: Rename server.json to prevent name mismatch from GameServer/server.json
        public void Initialize()
        {
            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
        }
    }
}