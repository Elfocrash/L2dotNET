using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class GameplayConfig
    {
        [JsonProperty(PropertyName = "AutoLoot")]
        public bool AutoLoot { get; set; }

        [JsonProperty(PropertyName = "DeleteCharAfterDays")]
        public int DeleteDays { get; set; }

        [JsonProperty(PropertyName = "FloodProtector")]
        public FloodProtector FloodProtector { get; set; }

        [JsonProperty(PropertyName = "CommunityBoard")]
        public CommunityBoard CommunityBoard { get; set; }

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
    }
}