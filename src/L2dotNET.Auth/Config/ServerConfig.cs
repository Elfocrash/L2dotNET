using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth
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
