using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
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

        public Config() { }

        public void Initialize()
        {
            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
            GameplayConfig = new GameplayConfig();
            GameplayConfig.Server = JsonConvert.DeserializeObject<Server2>(File.ReadAllText(@"config\gameplay.json"));
            GameplayConfig.Clan = JsonConvert.DeserializeObject<Clan>(File.ReadAllText(@"config\clan.json"));
            GameplayConfig.Event = JsonConvert.DeserializeObject<Event>(File.ReadAllText(@"config\events.json"));
            GameplayConfig.Geodata = JsonConvert.DeserializeObject<Geodata>(File.ReadAllText(@"config\geodata.json"));
            GameplayConfig.Login = JsonConvert.DeserializeObject<Login>(File.ReadAllText(@"config\loginserver.json"));
            GameplayConfig.Npc = JsonConvert.DeserializeObject<Npc>(File.ReadAllText(@"config\npcs.json"));
            GameplayConfig.Player = JsonConvert.DeserializeObject<Player>(File.ReadAllText(@"config\player.json"));
            GameplayConfig.Siege = JsonConvert.DeserializeObject<Siege>(File.ReadAllText(@"config\siege.json"));
        }
    }
}