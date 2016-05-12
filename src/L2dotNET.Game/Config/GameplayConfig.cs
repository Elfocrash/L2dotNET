using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService
{
    public class GameplayConfig
    {
        [JsonProperty(PropertyName = "max_buffs")]
        public int MaxBuffs { get; set; }

        [JsonProperty(PropertyName = "autoloot")]
        public bool AutoLoot { get; set; }
    }
}
