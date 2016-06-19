using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
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
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Config();
                    }

                return instance;
            }
        }

        public ServerConfig serverConfig;
        public GameplayConfig gameplayConfig;

        public Config() { }

        public void Initialize()
        {
            serverConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
            gameplayConfig = new GameplayConfig();
            gameplayConfig.Server = JsonConvert.DeserializeObject<Server2>(File.ReadAllText(@"config\gameplay.json"));
            gameplayConfig.Clan = JsonConvert.DeserializeObject<Clan>(File.ReadAllText(@"config\clan.json"));
            gameplayConfig.Event = JsonConvert.DeserializeObject<Event>(File.ReadAllText(@"config\events.json"));
            gameplayConfig.Login = JsonConvert.DeserializeObject<Login>(File.ReadAllText(@"config\loginserver.json"));
            gameplayConfig.Npc = JsonConvert.DeserializeObject<Npc>(File.ReadAllText(@"config\npcs.json"));
            gameplayConfig.Player = JsonConvert.DeserializeObject<Player>(File.ReadAllText(@"config\player.json"));
            gameplayConfig.Siege = JsonConvert.DeserializeObject<Siege>(File.ReadAllText(@"config\siege.json"));
        }
    }
}