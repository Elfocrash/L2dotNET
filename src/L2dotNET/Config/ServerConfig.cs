using Newtonsoft.Json;

namespace L2dotNET.Config
{
    ///<summary>Server Config.</summary>
    public class ServerConfig
    {
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        [JsonProperty(PropertyName = "port")]
        public int Port { get; set; }

        [JsonProperty(PropertyName = "auth_host")]
        public string AuthHost { get; set; }

        [JsonProperty(PropertyName = "auth_port")]
        public int AuthPort { get; set; }

        [JsonProperty(PropertyName = "auth_code")]
        public string AuthCode { get; set; }

        [JsonProperty(PropertyName = "isgmonly")]
        public bool IsGmOnly { get; set; }

        [JsonProperty(PropertyName = "istestserver")]
        public bool IsTestServer { get; set; }

        [JsonProperty(PropertyName = "max_players")]
        public int MaxPlayers { get; set; }

        [JsonProperty(PropertyName = "lazy_html_cache")]
        public bool LazyHtmlCache { get; set; }
    }
}