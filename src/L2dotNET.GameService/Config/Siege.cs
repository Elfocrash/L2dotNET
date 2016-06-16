using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    ///<summary>Configs for Sieges</summary>
    public class Siege
    {
        ///<summary>Length of siege before the countdown(in minutes).</summary>
        [JsonProperty(PropertyName = "SiegeLength")]
        public string SiegeLength { get; set; }

        ///<summary>Max numbers of flags per clan.</summary>
        [JsonProperty(PropertyName = "MaxFlags")]
        public string MaxFlags { get; set; }

        ///<summary>Minimum clan level to register.</summary>
        [JsonProperty(PropertyName = "SiegeClanMinLevel")]
        public string SiegeClanMinLevel { get; set; }

        ///<summary>Max numbers of clans that can register on attacker side.</summary>
        [JsonProperty(PropertyName = "AttackerMaxClans")]
        public string AttackerMaxClans { get; set; }

        ///<summary>Max numbers of clans that can register on defender side.</summary>
        [JsonProperty(PropertyName = "DefenderMaxClans")]
        public string DefenderMaxClans { get; set; }

        ///<summary>Attackers respawn time (in ms).</summary>
        [JsonProperty(PropertyName = "AttackerRespawn")]
        public string AttackerRespawn { get; set; }

        ////////////////////////////////////////////////////////////////////

        ///<summary></summary>
        [JsonProperty(PropertyName = "AdenControlTower1")]
        public ControlTower AdenControlTower1 { get; set; }

        ///<summary></summary>
        [JsonProperty(PropertyName = "AdenControlTower2")]
        public ControlTower AdenControlTower2 { get; set; }

        ///<summary></summary>
        [JsonProperty(PropertyName = "AdenControlTower3")]
        public FlameTower AdenControlTower3 { get; set; }

        ///<summary></summary>
        [JsonProperty(PropertyName = "AdenFlameTower1")]
        public FlameTower AdenFlameTower1 { get; set; }

        ///<summary></summary>
        [JsonProperty(PropertyName = "AdenFlameTower2")]
        public FlameTower AdenFlameTower2 { get; set; }
    }

    public class Coordinate
    {
        [JsonProperty(PropertyName = "x")]
        public string x { get; set; }

        [JsonProperty(PropertyName = "y")]
        public string y { get; set; }

        [JsonProperty(PropertyName = "z")]
        public string z { get; set; }
    }

    public class ControlTower
    {
        [JsonProperty(PropertyName = "Coords")]
        public Coordinate Coords { get; set; }

        [JsonProperty(PropertyName = "npcId")]
        public int npcId { get; set; }
    }

    public class FlameTower : ControlTower
    {
        [JsonProperty(PropertyName = "zoneIds")]
        public int[] zoneIds { get; set; }
    }
}
