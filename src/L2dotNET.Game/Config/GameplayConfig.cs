using Newtonsoft.Json;

namespace L2dotNET.GameService
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
    }

    public class CommunityBoard
    {
        [JsonProperty(PropertyName = "UseCommunityBoard")]
        public bool EnableCommunityBoard { get; set; }

        [JsonProperty(PropertyName = "BBSDefault")]
        public string BBSDefault { get; set; }
    }

    public class FloodProtector
    {
        [JsonProperty(PropertyName = "CharacterSelectTime")]
        public int CharacterSelectTime { get; set; }

        [JsonProperty(PropertyName = "DropItemTime")]
        public int DropItemTime { get; set; }

        [JsonProperty(PropertyName = "GlobalChatTime")]
        public int GlobalChatTime { get; set; }

        [JsonProperty(PropertyName = "HeroVoiceTime")]
        public int HeroVoiceTime { get; set; }

        [JsonProperty(PropertyName = "ManorTime")]
        public int ManorTime { get; set; }

        [JsonProperty(PropertyName = "ManufactureTime")]
        public int ManufactureTime { get; set; }

        [JsonProperty(PropertyName = "MultisellTime")]
        public int MultisellTime { get; set; }

        [JsonProperty(PropertyName = "RollDiceTime")]
        public int RollDiceTime { get; set; }

        [JsonProperty(PropertyName = "SendMailTime")]
        public int SendMailTime { get; set; }

        [JsonProperty(PropertyName = "ServerBypassTime")]
        public int ServerBypassTime { get; set; }

        [JsonProperty(PropertyName = "SocialTime")]
        public int SocialTime { get; set; }

        [JsonProperty(PropertyName = "SubclassTime")]
        public int SubclassTime { get; set; }

        [JsonProperty(PropertyName = "TradeChatTime")]
        public int TradeChatTime { get; set; }
    }
}