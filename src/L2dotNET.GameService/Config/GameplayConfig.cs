using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class GameplayConfig
    {
        [JsonProperty(PropertyName = "autoloot")]
        public bool AutoLoot { get; set; }

        [JsonProperty(PropertyName = "DeleteCharAfterDays")]
        public int DeleteDays { get; set; }

        [JsonProperty(PropertyName = "max_buffs")]
        public int MaxBuffs { get; set; }

        [JsonProperty(PropertyName = "FloodProtector")]
        public FloodProtector FloodProtector { get; set; }

        [JsonProperty(PropertyName = "CommunityBoard")]
        public CommunityBoard CommunityBoard { get; set; }

        [JsonProperty(PropertyName = "Siege")]
        public Siege Siege { get; set; }
    }
}