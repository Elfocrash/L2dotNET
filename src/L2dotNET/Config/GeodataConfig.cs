using System.ComponentModel;
using Newtonsoft.Json;

namespace L2dotNET.Config
{
    ///<summary>Geodata Config.</summary>
    public class GeodataConfig
    {
        ///<summary>GeoData options, default 0.</summary>
        ///<summary>0: GeoData, PathChecking and PathFinding OFF.</summary>
        ///<summary>* Players/monsters can attack and walk through walls, but not doors.</summary>
        ///<summary>1: GeoData and PathChecking ON, PathFinding OFF.</summary>
        ///<summary>* GeoData files are required.</summary>
        ///<summary>* The Line Of Sight (LoS) check is being performed. None interaction/.</summary>
        ///<summary>attack/spellcast through walls or over a large obstacle is allowed.</summary>
        ///<summary>* The Line of Movement (LoM) check is being performed. Can not walk.</summary>
        ///<summary>through wall or an obstacle.</summary>
        ///<summary>? Monsters can pass walls but not aggro through them.</summary>
        ///<summary>2: GeoData, PathChecking and PathFinding ON.</summary>
        ///<summary>* When LoM check fails, the PathFinding is performed to look for.</summary>
        ///<summary>an alternative path (e.g. walk around obstacle).</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "GeoData", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int GeoData { get; set; }

        ///<summary>Specifies the path to geodata files. For example, when using L2off geodata files located.</summary>
        ///<summary>at Lineage client folder ("C:/Program Files/Lineage II/system/geodata/"), default: ./data/geodata/.</summary>
        [DefaultValue("./data/geodata/")]
        [JsonProperty(PropertyName = "GeoDataPath", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string GeoDataPath { get; set; }

        ///<summary>Sets the geodata file format and filename conventions, default: L2J.</summary>
        ///<summary>L2J:   Geodata are in standard L2J format (using filename e.g. 22_16.l2j).</summary>
        ///<summary>L2OFF: Geodata are in standard L2OFF format (using filename e.g. 22_16_conv.dat).</summary>
        ///<summary>L2D:   Geodata are in diagonal L2D format (using filename e.g. 22_16.l2d).</summary>
        //TODO: Missing DefaultValue, value Enum based.
        [JsonProperty(PropertyName = "GeoDataFormat", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string GeoDataFormat { get; set; }

        ///<summary>Player coordinates synchronization.</summary>
        ///<summary>1 - partial synchronization Client --> Server ; don't use it with geodata.</summary>
        ///<summary>2 - partial synchronization Server --> Client ; use this setting with geodata.</summary>
        ///<summary>-1 - Old system: will synchronize Z only, default: -1.</summary>
        [DefaultValue(-1)]
        [JsonProperty(PropertyName = "CoordSynchronize", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int CoordSynchronize { get; set; }

        ///<summary>=================================================================.</summary>
        ///<summary>Path checking.</summary>
        ///<summary>=================================================================.</summary>
        ///<summary>Line of sight start at X percent of the character height, default: 75.</summary>
        [DefaultValue(75)]
        [JsonProperty(PropertyName = "PartOfCharacterHeight", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PartOfCharacterHeight { get; set; }

        ///<summary>Maximum height of an obstacle, which can exceed the line of sight, default: 32.</summary>
        [DefaultValue(32)]
        [JsonProperty(PropertyName = "MaxObstacleHeight", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxObstacleHeight { get; set; }

        ///<summary>=================================================================.</summary>
        ///<summary>Path finding.</summary>
        ///<summary>=================================================================.</summary>
        ///<summary>Pathfinding array buffers configuration, default: 100x6;128x6;192x6;256x4;320x4;384x4;500x2.</summary>
        //TODO: Make better PathFindBuffers parameters.
        [DefaultValue("100x6;128x6;192x6;256x4;320x4;384x4;500x2")]
        [JsonProperty(PropertyName = "PathFindBuffers", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string PathFindBuffers { get; set; }

        ///<summary>Base path weight, when moving from one node to another on axis direction, default: 10.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "BaseWeight", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int BaseWeight { get; set; }

        ///<summary>Path weight, when moving from one node to another on diagonal direction, default: BaseWeight * sqrt(2) = 14.</summary>
        [DefaultValue(14)]
        [JsonProperty(PropertyName = "DiagonalWeight", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DiagonalWeight { get; set; }

        ///<summary>When movement flags of target node is blocked to any direction, multiply movement weight by this multiplier.</summary>
        ///<summary>This causes pathfinding algorithm to avoid path construction exactly near an obstacle, default: 10.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "ObstacleMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ObstacleMultiplier { get; set; }

        ///<summary>Weight of the heuristic algorithm, which is giving estimated cost from node to target, default: 20.</summary>
        ///<summary>For proper function must be higher than BaseWeight and/or DiagonalWeight.</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "HeuristicWeight", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int HeuristicWeight { get; set; }

        ///<summary>Maximum number of generated nodes per one path-finding process, default 3500.</summary>
        [DefaultValue(3500)]
        [JsonProperty(PropertyName = "MaxIterations", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxIterations { get; set; }

        ///<summary>Path debug function, FOR DEBUG PURPOSES ONLY!.</summary>
        ///<summary>Adena = Nodes known to path-find algorithm (amount show node cost * 10).</summary>
        ///<summary>Antidote = constructed path (amount show node cost * 10).</summary>
        ///<summary>Blue Potion = begining of the path.</summary>
        ///<summary>Green Potion = node removed by LOS post-filter.</summary>
        ///<summary>Red Potion = actual waypoints.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "DebugPath", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool DebugPath { get; set; }

        /*
            =================================================================.
            Geodata files.
            =================================================================.

            The world contains 176 regions (11 x 16), each region has own geodata file.
            Geodata files are loaded according to the list below.
            Some regions are not supported by L2 client.
            16_10: load region (geodata options enabled).
            #16_10: skip region (geodata options disabled, no LoS/LoM checks).
            16_10
            16_11
            16_12
            16_13 - not supported by L2 client.
            16_14 - not supported by L2 client.
            16_15 - not supported by L2 client.
            16_16 - not supported by L2 client.
            16_17 - not supported by L2 client.
            16_18 - not supported by L2 client.
            16_19
            16_20
            16_21
            16_22
            16_23
            16_24
            16_25
            17_10
            17_11
            17_12
            17_13 - not supported by L2 client.
            17_14 - not supported by L2 client.
            17_15 - not supported by L2 client.
            17_16 - not supported by L2 client.
            17_17 - not supported by L2 client.
            17_18
            17_19
            17_20
            17_21
            17_22
            17_23
            17_24
            17_25
            18_10
            18_11
            18_12
            18_13
            18_14
            18_15 - not supported by L2 client.
            18_16 - not supported by L2 client.
            18_17
            18_18
            18_19
            18_20
            18_21
            18_22
            18_23
            18_24
            18_25
            19_10
            19_11
            19_12 - not supported by L2 client.
            19_13
            19_14
            19_15
            19_16
            19_17
            19_18
            19_19
            19_20
            19_21
            19_22
            19_23
            19_24
            19_25
            20_10
            20_11
            20_12 - not supported by L2 client.
            20_13
            20_14
            20_15
            20_16
            20_17
            20_18
            20_19
            20_20
            20_21
            20_22
            20_23
            20_24
            20_25
            21_10 - not supported by L2 client.
            21_11 - not supported by L2 client.
            21_12 - not supported by L2 client.
            21_13
            21_14
            21_15
            21_16
            21_17
            21_18
            21_19
            21_20
            21_21
            21_22
            21_23
            21_24
            21_25
            22_10 - not supported by L2 client.
            22_11 - not supported by L2 client.
            22_12 - not supported by L2 client.
            22_13
            22_14
            22_15
            22_16
            22_17
            22_18
            22_19
            22_20
            22_21
            22_22
            22_23
            22_24
            22_25
            23_10
            23_11
            23_12
            23_13
            23_14
            23_15
            23_16
            23_17
            23_18
            23_19
            23_20
            23_21
            23_22
            23_23
            23_24
            23_25
            24_10
            24_11
            24_12
            24_13
            24_14
            24_15
            24_16
            24_17
            24_18
            24_19
            24_20
            24_21
            24_22
            24_23
            24_24
            24_25
            25_10
            25_11
            25_12
            25_13 - not supported by L2 client.
            25_14
            25_15
            25_16
            25_17
            25_18
            25_19
            25_20
            25_21
            25_22 - not supported by L2 client.
            25_23 - not supported by L2 client.
            25_24 - not supported by L2 client.
            25_25 - not supported by L2 client.
            26_10 - not supported by L2 client.
            26_11
            26_12
            26_13 - not supported by L2 client.
            26_14
            26_15
            26_16
            26_17 - not supported by L2 client.
            26_18 - not supported by L2 client.
            26_19 - not supported by L2 client.
            26_20 - not supported by L2 client.
            26_21 - not supported by L2 client.
            26_22 - not supported by L2 client.
            26_23 - not supported by L2 client.
            26_24 - not supported by L2 client.
            26_25 - not supported by L2 client.
            */
    }
}