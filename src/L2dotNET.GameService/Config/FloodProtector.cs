using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
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