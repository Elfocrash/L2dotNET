using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.Config
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
        public GameplayConfig GameplayConfig;

        public void Initialize()
        {
            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
            GameplayConfig = new GameplayConfig
            {
                ClanConfig = JsonConvert.DeserializeObject<ClanConfig>(File.ReadAllText(@"config\clan.json")),
                EventConfig = JsonConvert.DeserializeObject<EventConfig>(File.ReadAllText(@"config\events.json")),
                GeodataConfig = JsonConvert.DeserializeObject<GeodataConfig>(File.ReadAllText(@"config\geodata.json")),
                LoginConfig = JsonConvert.DeserializeObject<LoginConfig>(File.ReadAllText(@"config\loginserver.json")),
                NpcConfig = JsonConvert.DeserializeObject<NpcConfig>(File.ReadAllText(@"config\npcs.json")),
                PlayerConfig = JsonConvert.DeserializeObject<PlayerConfig>(File.ReadAllText(@"config\player.json")),
                Server = JsonConvert.DeserializeObject<ServerConfig2>(File.ReadAllText(@"config\gameplay.json")),
                SiegeConfig = JsonConvert.DeserializeObject<SiegeConfig>(File.ReadAllText(@"config\siege.json")),
                OtherConfig = JsonConvert.DeserializeObject<OtherConfig>(File.ReadAllText(@"config\other.json"))
            };
        }
    }
}