using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class CommunityBoard
    {
        [JsonProperty(PropertyName = "UseCommunityBoard")]
        public bool EnableCommunityBoard { get; set; }

        [JsonProperty(PropertyName = "BBSDefault")]
        public string BBSDefault { get; set; }
    }
}