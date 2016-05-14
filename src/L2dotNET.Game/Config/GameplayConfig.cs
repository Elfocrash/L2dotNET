using Newtonsoft.Json;

namespace L2dotNET.GameService
{
    public class GameplayConfig
    {
        [JsonProperty(PropertyName = "autoloot")]
        public bool AutoLoot { get; set; }

        [JsonProperty(PropertyName = "DeleteCharAfterDays")]
        public int DELETE_DAYS { get; set; }

        [JsonProperty(PropertyName = "max_buffs")]
        public int MaxBuffs { get; set; }
    }
}
