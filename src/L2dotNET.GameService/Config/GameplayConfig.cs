using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class GameplayConfig
    {
        [JsonProperty(PropertyName = "Clan")]
        public Clan Clan { get; set; }

        [JsonProperty(PropertyName = "Siege")]
        public Siege Siege { get; set; }

        [JsonProperty(PropertyName = "Event")]
        public Event Event { get; set; }

        [JsonProperty(PropertyName = "Npc")]
        public Npc Npc { get; set; }

        [JsonProperty(PropertyName = "Player")]
        public Player Player { get; set; }

        [JsonProperty(PropertyName = "Server")]
        public Server2 Server { get; set; }

        [JsonProperty(PropertyName = "Login")]
        public Login Login { get; set; }

        [JsonProperty(PropertyName = "Geodata")]
        public Geodata Geodata { get; set; }
    }
}