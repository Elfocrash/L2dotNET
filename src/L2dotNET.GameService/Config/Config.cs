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
        public GameplayConfig GameplayConfig;

        public void Initialize()
        {
            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
            GameplayConfig = new GameplayConfig
                             {
                                 Server = JsonConvert.DeserializeObject<Server2>(File.ReadAllText(@"config\gameplay.json")),
                                 Clan = JsonConvert.DeserializeObject<Clan>(File.ReadAllText(@"config\clan.json")),
                                 Event = JsonConvert.DeserializeObject<Event>(File.ReadAllText(@"config\events.json")),
                                 Geodata = JsonConvert.DeserializeObject<Geodata>(File.ReadAllText(@"config\geodata.json")),
                                 Login = JsonConvert.DeserializeObject<Login>(File.ReadAllText(@"config\loginserver.json")),
                                 Npc = JsonConvert.DeserializeObject<Npc>(File.ReadAllText(@"config\npcs.json")),
                                 Player = JsonConvert.DeserializeObject<Player>(File.ReadAllText(@"config\player.json")),
                                 Siege = JsonConvert.DeserializeObject<Siege>(File.ReadAllText(@"config\siege.json"))
                             };
        }
    }
}