using Newtonsoft.Json;

namespace L2dotNET.LoginService.Config
{
    public class ServerConfig
    {
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        [JsonProperty(PropertyName = "port")]
        public int LoginPort { get; set; }

        [JsonProperty(PropertyName = "gsport")]
        public int GSPort { get; set; }

        [JsonProperty(PropertyName = "autocreate")]
        public bool AutoCreate { get; set; }
    }
}