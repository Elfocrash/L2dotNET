using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class GameplayConfig
    {
        [JsonProperty(PropertyName = "Clan")]
        public ClanConfig ClanConfig { get; set; }

        [JsonProperty(PropertyName = "Siege")]
        public SiegeConfig SiegeConfig { get; set; }

        [JsonProperty(PropertyName = "Event")]
        public EventConfig EventConfig { get; set; }

        [JsonProperty(PropertyName = "Npc")]
        public NpcConfig NpcConfig { get; set; }

        [JsonProperty(PropertyName = "Player")]
        public PlayerConfig PlayerConfig { get; set; }

        [JsonProperty(PropertyName = "Server")]
        public ServerConfig2 Server { get; set; }

        [JsonProperty(PropertyName = "Login")]
        public LoginConfig LoginConfig { get; set; }

        [JsonProperty(PropertyName = "Geodata")]
        public GeodataConfig GeodataConfig { get; set; }
    }
}