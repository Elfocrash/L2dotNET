using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class Npc
    {
        ///<summary>Champion mobs.</summary>
        [JsonProperty(PropertyName = "ChampionMobs")]
        public ChampionMobs ChampionMobs { get; set; }

        ///<summary>Buffer.</summary>
        [JsonProperty(PropertyName = "Buffer")]
        public Buffer Buffer { get; set; }

        ///<summary>Misc.</summary>
        [JsonProperty(PropertyName = "Misc")]
        public Misc Misc { get; set; }

        ///<summary>WyvernManager.</summary>
        [JsonProperty(PropertyName = "WyvernManager")]
        public WyvernManager WyvernManager { get; set; }

        ///<summary>RaidBoss.</summary>
        [JsonProperty(PropertyName = "RaidBoss")]
        public RaidBoss RaidBoss { get; set; }

        ///<summary>GrandBosses.</summary>
        [JsonProperty(PropertyName = "GrandBosses")]
        public GrandBosses GrandBosses { get; set; }

        ///<summary>IA.</summary>
        [JsonProperty(PropertyName = "IA")]
        public IA IA { get; set; }
    }

    ///<summary>Champion mobs.</summary>
    public class ChampionMobs
    {
        ///<summary>Chance for a mob to became champion (in percent) - 0 to disable.</summary>
        [JsonProperty(PropertyName = "Frequency")]
        public int Frequency { get; set; }

        ///<summary>Min lvl allowed for a mob to be champion.</summary>
        [JsonProperty(PropertyName = "MinLevel")]
        public int MinLevel { get; set; }

        ///<summary>Min lvl allowed for a mob to be champion.</summary>
        [JsonProperty(PropertyName = "MaxLevel")]
        public int MaxLevel { get; set; }

        ///<summary>Hp multiplier.</summary>
        [JsonProperty(PropertyName = "Hp")]
        public int Hp { get; set; }

        ///<summary>Hp Regen Multiplier.</summary>
        [JsonProperty(PropertyName = "RegenHp")]
        public double RegenHp { get; set; }

        ///<summary>Rewards multiplier.</summary>
        [JsonProperty(PropertyName = "Rewards")]
        public int Rewards { get; set; }

        ///<summary>Adenas & seal stones rewards multiplier.</summary>
        [JsonProperty(PropertyName = "AdenasRewards")]
        public int AdenasRewards { get; set; }

        ///<summary>Atk bonus for champion (changes apply on patk & matk).</summary>
        [JsonProperty(PropertyName = "Atk")]
        public double Atk { get; set; }

        ///<summary>Spd Atk bonus for champion (changes apply on patkspd & matkspd).</summary>
        [JsonProperty(PropertyName = "SpdAtk")]
        public double SpdAtk { get; set; }

        ///<summary>Chance to obtain a specified reward item from a higher lvl champion (in percents) default is off using glittering medal as reward.</summary>
        [JsonProperty(PropertyName = "RewardItem")]
        public int RewardItem { get; set; }

        ///<summary>Specified reward item ID.</summary>
        [JsonProperty(PropertyName = "RewardItemID")]
        public int RewardItemID { get; set; }

        ///<summary>Specified reward item rnd qty.</summary>
        [JsonProperty(PropertyName = "RewardItemQty")]
        public int RewardItemQty { get; set; }
    }

    ///<summary>Buffer.</summary>
    public class Buffer
    {
        ///<summary>Maximum number of available schemes per player.</summary>
        [JsonProperty(PropertyName = "MaxSchemesPerChar")]
        public int MaxSchemesPerChar { get; set; }

        ///<summary>Maximum number of buffs per scheme.</summary>
        [JsonProperty(PropertyName = "MaxSkillsPerScheme")]
        public int MaxSkillsPerScheme { get; set; }

        ///<summary>Static cost of buffs ; override skills price if different of -1.</summary>
        [JsonProperty(PropertyName = "StaticCostPerBuff")]
        public int StaticCostPerBuff { get; set; }

        ///<summary>The list of buffs, under a skillId,buffPrice,groupType format.</summary>
        [JsonProperty(PropertyName = "Buffs")]
        public int Buffs { get; set; }
    }

    ///<summary>Misc.</summary>
    public class Misc
    {
        ///<summary>Allow the use of class Managers to change occupation.</summary>
        ///<summary>Default = False.</summary>
        [JsonProperty(PropertyName = "AllowClassMasters")]
        public bool AllowClassMasters { get; set; }

        ///<summary>ConfigClassMaster=1;[57(100000)];[];2;[57(1000000)];[];3;[57(10000000)],[5575(1000000)];[6622(1)].</summary>
        ///<summary>1st occupation change for 100.000 Adena (item id 57).</summary>
        ///<summary>2nd occupation change for 1.000.0000 Adena (item id 57).</summary>
        ///<summary>3rd occupation change for 10.000.0000 Adena (item id 57) and 1.000.000 Ancient Adena (item id 5575).</summary>
        ///<summary>on 3rd occupation change player will be rewarded with 1 Book of Giants (item id 6622).</summary>
        ///<summary>ConfigClassMaster=1;[];[];2;[];[];3;[];[].</summary>
        ///<summary>1st, 2nd, 3rd occupation change for free, without rewards.</summary>
        ///<summary>ConfigClassMaster=1;[];[];2;[];[].</summary>
        ///<summary>Allow only first and second change.</summary>
        [JsonProperty(PropertyName = "ConfigClassMaster")]
        public int ConfigClassMaster { get; set; }

        ///<summary>Class Masters will allow changing to any occupation on any level inside class tree.</summary>
        ///<summary>For example, Dwarven Fighter will be able to advance to:.</summary>
        ///<summary>Artisan, Scavenger, Warsmith, Bounty Hunter, Maestro, Fortune Seeker.</summary>
        ///<summary>But Warsmith will be able to change only to Maestro.</summary>
        ///<summary>Default = False.</summary>
        [JsonProperty(PropertyName = "AllowEntireTree")]
        public bool AllowEntireTree { get; set; }

        ///<summary>Allow free teleportation around the world.</summary>
        [JsonProperty(PropertyName = "FreeTeleporting")]
        public bool FreeTeleporting { get; set; }

        ///<summary>Announce to players the location of the Mammon NPCs during Seal Validation.</summary>
        ///<summary>Default is False.</summary>
        [JsonProperty(PropertyName = "AnnounceMammonSpawn")]
        public bool AnnounceMammonSpawn { get; set; }

        ///<summary>Alternative mob behavior in peace zones.</summary>
        ///<summary>Default = True; Set to False to prevent mobs from auto-agro against players in peace zones.</summary>
        [JsonProperty(PropertyName = "MobAgroInPeaceZone")]
        public bool MobAgroInPeaceZone { get; set; }

        ///<summary>Show L2Monster level and aggro.</summary>
        [JsonProperty(PropertyName = "ShowNpcLevel")]
        public bool ShowNpcLevel { get; set; }

        ///<summary>Show clan && alliance crests on NPCs, default: False.</summary>
        [JsonProperty(PropertyName = "ShowNpcCrest")]
        public bool ShowNpcCrest { get; set; }

        ///<summary>Show clan && alliance crests on summons, default: False.</summary>
        [JsonProperty(PropertyName = "ShowSummonCrest")]
        public bool ShowSummonCrest { get; set; }
    }

    ///<summary>Wyvern Manager.</summary>
    public class WyvernManager
    {
        ///<summary>Spawn instances of Wyvern Manager on castles (allow castle lords to mount wyverns).</summary>
        [JsonProperty(PropertyName = "AllowWyvernUpgrader")]
        public bool AllowWyvernUpgrader { get; set; }

        ///<summary>Required minimum level of the Strider to allow NPC to transform it to wyvern.</summary>
        [JsonProperty(PropertyName = "RequiredStriderLevel")]
        public int RequiredStriderLevel { get; set; }

        ///<summary>Number of needed B-crystals.</summary>
        [JsonProperty(PropertyName = "RequiredCrystalsNumber")]
        public int RequiredCrystalsNumber { get; set; }
    }

    ///<summary>RaidBoss.</summary>
    public class RaidBoss
    {
        ///<summary>% hp regeneration for RaidBoss and their minions - on a base 1 = 100%.</summary>
        [JsonProperty(PropertyName = "HpRegenMultiplier")]
        public double HpRegenMultiplier { get; set; }

        ///<summary>% mp regeneration for RaidBoss and their minions - on a base 1 = 100%.</summary>
        [JsonProperty(PropertyName = "MpRegenMultiplier")]
        public double MpRegenMultiplier { get; set; }

        ///<summary>% defence for RaidBoss and their minions - on a base 1 = 100%.</summary>
        [JsonProperty(PropertyName = "DefenceMultiplier")]
        public double DefenceMultiplier { get; set; }

        ///<summary>Minions respawn timer in ms (default: 300000 = 5 mins).</summary>
        [JsonProperty(PropertyName = "MinionRespawnTime")]
        public int MinionRespawnTime { get; set; }

        ///<summary>Disable the penalty level paralize curse, false by default.</summary>
        [JsonProperty(PropertyName = "DisableCurse")]
        public bool DisableCurse { get; set; }

        ///<summary>Configure the interval at which raid bosses and minions wont reconsider their target. This time is in seconds. Default: 30.</summary>
        [JsonProperty(PropertyName = "RaidChaosTime")]
        public int RaidChaosTime { get; set; }

        ///<summary>Configure the interval at which raid bosses and minions wont reconsider their target. This time is in seconds. Default: 30.</summary>
        [JsonProperty(PropertyName = "GrandChaosTime")]
        public int GrandChaosTime { get; set; }

        ///<summary>Configure the interval at which raid bosses and minions wont reconsider their target. This time is in seconds. Default: 30.</summary>
        [JsonProperty(PropertyName = "MinionChaosTime")]
        public int MinionChaosTime { get; set; }
    }

    ///<summary>Grand Boss.</summary>
    public class GrandBoss
    {
        ///<summary>Interval time. Value is hour.</summary>
        [JsonProperty(PropertyName = "SpawnInterval")]
        public int SpawnInterval { get; set; }

        ///<summary>Random interval. Value is hour.</summary>
        [JsonProperty(PropertyName = "RandomSpawn")]
        public int RandomSpawn { get; set; }

        ///<summary>Delay of appearance time. Value is minute.</summary>
        [JsonProperty(PropertyName = "WaitTime")]
        public int WaitTime { get; set; }
    }

    ///<summary>GrandBosses.</summary>
    public class GrandBosses
    {
        ///<summary>Ant Queen.</summary>
        [JsonProperty(PropertyName = "AntQueen")]
        private GrandBoss AntQueen { get; set; }

        ///<summary>Antharas.</summary>
        [JsonProperty(PropertyName = "Antharas")]
        private GrandBoss Antharas { get; set; }

        ///<summary>Baium.</summary>
        [JsonProperty(PropertyName = "Baium")]
        private GrandBoss Baium { get; set; }

        ///<summary>Core.</summary>
        [JsonProperty(PropertyName = "Core")]
        private GrandBoss Core { get; set; }

        ///<summary>Frintezza.</summary>
        [JsonProperty(PropertyName = "Frintezza")]
        private GrandBoss Frintezza { get; set; }

        ///<summary>Orfen.</summary>
        [JsonProperty(PropertyName = "Orfen")]
        private GrandBoss Orfen { get; set; }

        ///<summary>Sailren.</summary>
        [JsonProperty(PropertyName = "Sailren")]
        private GrandBoss Sailren { get; set; }
    }

    ///<summary>IA.</summary>
    public class IA
    {
        ///<summary>If True, guards will attack at sight aggressive monsters.</summary>
        ///<summary>Default: False.</summary>
        [JsonProperty(PropertyName = "GuardAttackAggroMob")]
        public bool GuardAttackAggroMob { get; set; }

        ///<summary>Maximum range mobs can randomly go from spawn point.</summary>
        [JsonProperty(PropertyName = "MaxDriftRange")]
        public int MaxDriftRange { get; set; }

        ///<summary>Interval (in milliseconds) in which the knownlist does full updates.</summary>
        ///<summary>For move based updates, it is used for intermediate updates.</summary>
        ///<summary>WARNING ! Interval must be between 300 - 2000. Too small value may kill your CPU, too high value may not update knownlists properly.</summary>
        ///<summary>Default: 1250.</summary>
        [JsonProperty(PropertyName = "KnownListUpdateInterval")]
        public int KnownListUpdateInterval { get; set; }

        ///<summary>Minimum variable in seconds for npc animation delay.</summary>
        ///<summary>You must keep MinNPCAnimation lesser or equals than MaxNPCAnimation.</summary>
        [JsonProperty(PropertyName = "MinNPCAnimation")]
        public int MinNPCAnimation { get; set; }

        ///<summary>Maximum maximum variable in seconds for npc animation delay.</summary>
        ///<summary>You must keep MinNPCAnimation lesser or equals than MaxNPCAnimation.</summary>
        [JsonProperty(PropertyName = "MaxNPCAnimation")]
        public int MaxNPCAnimation { get; set; }

        ///<summary>Minimum variable in seconds for monster animation delay.</summary>
        ///<summary>You must keep MinMonsterAnimation lesser or equals than MaxMonsterAnimation.</summary>
        [JsonProperty(PropertyName = "MinMonsterAnimation")]
        public int MinMonsterAnimation { get; set; }

        ///<summary>Maximum variable in seconds for monster animation delay.</summary>
        ///<summary>You must keep MinMonsterAnimation lesser or equals than MaxMonsterAnimation.</summary>
        [JsonProperty(PropertyName = "MaxMonsterAnimation")]
        public int MaxMonsterAnimation { get; set; }

        ///<summary>Grid options: Grids can now turn themselves on and off.  This also affects the loading.</summary>
        ///<summary>and processing of all AI tasks and (in the future) geodata within this grid.</summary>
        ///<summary>Turn on for a grid with a person in it is immediate, but it then turns on.</summary>
        ///<summary>the 8 neighboring grids based on the specified number of seconds.</summary>
        ///<summary>Turn off for self and neighbors occures after the specified number of.</summary>
        ///<summary>seconds have passed during which a grid has had no players in or in any of its neighbors.</summary>
        ///<summary>The always on option allows to ignore all this and let all grids be active at all times.</summary>
        [JsonProperty(PropertyName = "GridsAlwaysOn")]
        public bool GridsAlwaysOn { get; set; }

        [JsonProperty(PropertyName = "GridNeighborTurnOnTime")]
        public int GridNeighborTurnOnTime { get; set; }

        [JsonProperty(PropertyName = "GridNeighborTurnOffTime")]
        public int GridNeighborTurnOffTime { get; set; }
    }
}