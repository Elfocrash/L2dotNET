using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.GameService
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
        public GameplayConfig gameplayConfig;

        public Config()
        {

        }

        public void Initialize()
        {
            serverConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
            gameplayConfig = JsonConvert.DeserializeObject<GameplayConfig>(File.ReadAllText(@"config\gameplay.json"));
        }
    }
}
