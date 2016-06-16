using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    ///<summary>Configs for Sieges</summary>
    public class Siege
    {
        ///<summary>Length of siege before the countdown(in minutes).</summary>
        [JsonProperty(PropertyName = "SiegeLength")]
        public int SiegeLength { get; set; }

        ///<summary>Max numbers of flags per clan.</summary>
        [JsonProperty(PropertyName = "MaxFlags")]
        public int MaxFlags { get; set; }

        ///<summary>Minimum clan level to register.</summary>
        [JsonProperty(PropertyName = "SiegeClanMinLevel")]
        public int SiegeClanMinLevel { get; set; }

        ///<summary>Max numbers of clans that can register on attacker side.</summary>
        [JsonProperty(PropertyName = "AttackerMaxClans")]
        public int AttackerMaxClans { get; set; }

        ///<summary>Max numbers of clans that can register on defender side.</summary>
        [JsonProperty(PropertyName = "DefenderMaxClans")]
        public int DefenderMaxClans { get; set; }

        ///<summary>Attackers respawn time (in ms).</summary>
        [JsonProperty(PropertyName = "AttackerRespawn")]
        public int AttackerRespawn { get; set; }

        [JsonProperty(PropertyName = "Castles")]
        public Castles Castles { get; set; }
    }

    public class Coordinate
    {
        [JsonProperty(PropertyName = "x")]
        public int x { get; set; }

        [JsonProperty(PropertyName = "y")]
        public int y { get; set; }

        [JsonProperty(PropertyName = "z")]
        public int z { get; set; }
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

    public class CastleTower
    {
        [JsonProperty(PropertyName = "ControlTower1")]
        private ControlTower ControlTower1 { get; set; }

        [JsonProperty(PropertyName = "ControlTower2")]
        private ControlTower ControlTower2 { get; set; }

        [JsonProperty(PropertyName = "ControlTower3")]
        private ControlTower ControlTower3 { get; set; }

        [JsonProperty(PropertyName = "FlameTower1")]
        private FlameTower FlameTower1 { get; set; }

        [JsonProperty(PropertyName = "FlameTower2")]
        private FlameTower FlameTower2 { get; set; }
    }

    public class Castles
    {
        [JsonProperty(PropertyName = "Aden")]
        public CastleTower Aden { get; set; }

        [JsonProperty(PropertyName = "Gludio")]
        public CastleTower Gludio { get; set; }

        [JsonProperty(PropertyName = "Dion")]
        public CastleTower Dion { get; set; }

        [JsonProperty(PropertyName = "Giran")]
        public CastleTower Giran { get; set; }

        [JsonProperty(PropertyName = "Oren")]
        public CastleTower Oren { get; set; }

        [JsonProperty(PropertyName = "Innadril")]
        public CastleTower Innadril { get; set; }

        [JsonProperty(PropertyName = "Goddard")]
        public CastleTower Goddard { get; set; }

        [JsonProperty(PropertyName = "Rune")]
        public CastleTower Rune { get; set; }

        [JsonProperty(PropertyName = "Schuttgart")]
        public CastleTower Schuttgart { get; set; }
    }
}