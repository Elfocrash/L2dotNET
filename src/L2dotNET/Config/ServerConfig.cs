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

        [JsonProperty(PropertyName = "authHost")]
        public string AuthHost { get; set; }

        [JsonProperty(PropertyName = "authPort")]
        public int AuthPort { get; set; }

        [JsonProperty(PropertyName = "authKey")]
        public string AuthKey { get; set; }

        [JsonProperty(PropertyName = "isgmonly")]
        public bool IsGmOnly { get; set; }

        [JsonProperty(PropertyName = "istestserver")]
        public bool IsTestServer { get; set; }

        [JsonProperty(PropertyName = "maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonProperty(PropertyName = "lazyHtmlCache")]
        public bool LazyHtmlCache { get; set; }
    }
}