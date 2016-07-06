using System.ComponentModel;
using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class Npc
    {
        ///<summary>Champion mobs.</summary>
        [JsonProperty(PropertyName = "ChampionMobs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ChampionMobs ChampionMobs { get; set; }

        ///<summary>Buffer.</summary>
        [JsonProperty(PropertyName = "Buffer", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Buffer Buffer { get; set; }

        ///<summary>Misc.</summary>
        [JsonProperty(PropertyName = "Misc", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Misc Misc { get; set; }

        ///<summary>WyvernManager.</summary>
        [JsonProperty(PropertyName = "WyvernManager", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public WyvernManager WyvernManager { get; set; }

        ///<summary>RaidBoss.</summary>
        [JsonProperty(PropertyName = "RaidBoss", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public RaidBoss RaidBoss { get; set; }

        ///<summary>GrandBosses.</summary>
        [JsonProperty(PropertyName = "GrandBosses", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public GrandBosses GrandBosses { get; set; }

        ///<summary>IA.</summary>
        [JsonProperty(PropertyName = "IA", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Ia Ia { get; set; }
    }

    ///<summary>Champion mobs.</summary>
    public class ChampionMobs
    {
        ///<summary>Chance for a mob to became champion (in percent) - 0 to disable.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "Frequency", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Frequency { get; set; }

        ///<summary>Min lvl allowed for a mob to be champion.</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "MinLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MinLevel { get; set; }

        ///<summary>Max lvl allowed for a mob to be champion.</summary>
        [DefaultValue(70)]
        [JsonProperty(PropertyName = "MaxLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxLevel { get; set; }

        ///<summary>Hp multiplier.</summary>
        [DefaultValue(8)]
        [JsonProperty(PropertyName = "Hp", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Hp { get; set; }

        ///<summary>Hp Regen Multiplier.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RegenHp", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RegenHp { get; set; }

        ///<summary>Rewards multiplier.</summary>
        [DefaultValue(8)]
        [JsonProperty(PropertyName = "Rewards", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Rewards { get; set; }

        ///<summary>Adenas & seal stones rewards multiplier.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "AdenasRewards", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int AdenasRewards { get; set; }

        ///<summary>Atk bonus for champion (changes apply on patk & matk).</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "Atk", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double Atk { get; set; }

        ///<summary>Spd Atk bonus for champion (changes apply on patkspd & matkspd).</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "SpdAtk", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double SpdAtk { get; set; }

        ///<summary>Chance to obtain a specified reward item from a higher lvl champion (in percents) default is off using glittering medal as reward.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "RewardItemChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RewardItemChance { get; set; }

        ///<summary>Specified reward item ID and qty.</summary>
        [JsonProperty(PropertyName = "RewardItemID", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ItemAmount[] RewardItem { get; set; } = /*DefaultValue*/ { new ItemAmount
                                                                          {
                                                                              ItemId = 6393,
                                                                              Amount = 1
                                                                          } };
    }

    ///<summary>Buffer.</summary>
    public class Buffer
    {
        ///<summary>Maximum number of available schemes per player.</summary>
        [DefaultValue(4)]
        [JsonProperty(PropertyName = "MaxSchemesPerChar", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxSchemesPerChar { get; set; }

        ///<summary>Maximum number of buffs per scheme.</summary>
        [DefaultValue(24)]
        [JsonProperty(PropertyName = "MaxSkillsPerScheme", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxSkillsPerScheme { get; set; }

        ///<summary>Static cost of buffs ; override skills price if different of -1.</summary>
        [DefaultValue(-1)]
        [JsonProperty(PropertyName = "StaticCostPerBuff", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int StaticCostPerBuff { get; set; }

        ///<summary>The list of buffs, under a skillId,buffPrice,groupType format.</summary>
        [JsonProperty(PropertyName = "Buffs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public BufferBuffs[] Buffs { get; set; } = /*DefaultValue*/ { };
    }

    ///<summary>BufferBuffs.</summary>
    public class BufferBuffs
    {
        ///<summary>Skill Id.</summary>
        [JsonProperty(PropertyName = "SkillId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SkillId { get; set; }

        ///<summary>Cost to buff.</summary>
        [JsonProperty(PropertyName = "Price", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Price { get; set; }

        ///<summary>Name of the group.</summary>
        [JsonProperty(PropertyName = "GroupType", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string GroupType { get; set; }
    }

    ///<summary>Misc.</summary>
    public class Misc
    {
        ///<summary>Allow the use of class Managers to change occupation.</summary>
        ///<summary>Default = False.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AllowClassMasters", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowClassMasters { get; set; }

        ///<summary>Class Masters will allow changing to any occupation on any level inside class tree.</summary>
        ///<summary>For example, Dwarven Fighter will be able to advance to:.</summary>
        ///<summary>Artisan, Scavenger, Warsmith, Bounty Hunter, Maestro, Fortune Seeker.</summary>
        ///<summary>But Warsmith will be able to change only to Maestro.</summary>
        ///<summary>Default = False.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AllowEntireTree", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowEntireTree { get; set; }

        ///<summary>ConfigClassMaster=1;[57(100000)];[];2;[57(1000000)];[];3;[57(10000000)],[5575(1000000)];[6622(1)].</summary>
        ///<summary>1st occupation change for 100.000 Adena (item id 57).</summary>
        ///<summary>2nd occupation change for 1.000.0000 Adena (item id 57).</summary>
        ///<summary>3rd occupation change for 10.000.0000 Adena (item id 57) and 1.000.000 Ancient Adena (item id 5575).</summary>
        ///<summary>on 3rd occupation change player will be rewarded with 1 Book of Giants (item id 6622).</summary>
        ///<summary>ConfigClassMaster=1;[];[];2;[];[];3;[];[].</summary>
        ///<summary>1st, 2nd, 3rd occupation change for free, without rewards.</summary>
        ///<summary>ConfigClassMaster=1;[];[];2;[];[].</summary>
        ///<summary>Allow only first and second change.</summary>
        [JsonProperty(PropertyName = "ConfigClassMaster", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ConfigClassMaster ConfigClassMaster { get; set; } = /*DefaultValue*/ new ConfigClassMaster();

        ///<summary>Allow free teleportation around the world.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "FreeTeleporting", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool FreeTeleporting { get; set; }

        ///<summary>Announce to players the location of the Mammon NPCs during Seal Validation.</summary>
        ///<summary>Default is False.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AnnounceMammonSpawn", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AnnounceMammonSpawn { get; set; }

        ///<summary>Alternative mob behavior in peace zones.</summary>
        ///<summary>Default = True; Set to False to prevent mobs from auto-agro against players in peace zones.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "MobAgroInPeaceZone", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool MobAgroInPeaceZone { get; set; }

        ///<summary>Show L2Monster level and aggro.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ShowNpcLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ShowNpcLevel { get; set; }

        ///<summary>Show clan && alliance crests on NPCs, default: False.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ShowNpcCrest", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ShowNpcCrest { get; set; }

        ///<summary>Show clan && alliance crests on summons, default: False.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ShowSummonCrest", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ShowSummonCrest { get; set; }
    }

    //TODO: Implement get;set; for chance?
    public class ItemAmount
    {
        [JsonProperty(PropertyName = "ItemId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ItemId { get; set; }
        [JsonProperty(PropertyName = "Amount", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Amount { get; set; }
    }

    ///<summary>ConfigClassMaster=1;[57(100000)];[];2;[57(1000000)];[];3;[57(10000000)],[5575(1000000)];[6622(1)].</summary>
    ///<summary>1st occupation change for 100.000 Adena (item id 57).</summary>
    ///<summary>2nd occupation change for 1.000.0000 Adena (item id 57).</summary>
    ///<summary>3rd occupation change for 10.000.0000 Adena (item id 57) and 1.000.000 Ancient Adena (item id 5575).</summary>
    ///<summary>on 3rd occupation change player will be rewarded with 1 Book of Giants (item id 6622).</summary>
    ///<summary>ConfigClassMaster=1;[];[];2;[];[];3;[];[].</summary>
    ///<summary>1st, 2nd, 3rd occupation change for free, without rewards.</summary>
    ///<summary>ConfigClassMaster=1;[];[];2;[];[].</summary>
    ///<summary>Allow only first and second change.</summary>
    public class ConfigClassMaster
    {
        [JsonProperty(PropertyName = "FirstOccupation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Occupation FirstOccupation;
        [JsonProperty(PropertyName = "SecondOccupation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Occupation SecondOccupation;
        [JsonProperty(PropertyName = "ThirdOccupation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Occupation ThirdOccupation;
    }

    public class Occupation
    {
        [JsonProperty(PropertyName = "ItemsCost", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ItemAmount[] ItemsCost;
        [JsonProperty(PropertyName = "ItemsReward", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ItemAmount[] ItemsReward;
    }

    ///<summary>Wyvern Manager.</summary>
    public class WyvernManager
    {
        ///<summary>Spawn instances of Wyvern Manager on castles (allow castle lords to mount wyverns).</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowWyvernUpgrader", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowWyvernUpgrader { get; set; }

        ///<summary>Required minimum level of the Strider to allow NPC to transform it to wyvern.</summary>
        [DefaultValue(55)]
        [JsonProperty(PropertyName = "RequiredStriderLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RequiredStriderLevel { get; set; }

        ///<summary>Number of needed B-crystals.</summary>
        /// TODO: Update to use item class
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "RequiredCrystalsNumber", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RequiredCrystalsNumber { get; set; }
    }

    ///<summary>RaidBoss.</summary>
    public class RaidBoss
    {
        ///<summary>% hp regeneration for RaidBoss and their minions - on a base 1 = 100%.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "HpRegenMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double HpRegenMultiplier { get; set; }

        ///<summary>% mp regeneration for RaidBoss and their minions - on a base 1 = 100%.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "MpRegenMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double MpRegenMultiplier { get; set; }

        ///<summary>% defence for RaidBoss and their minions - on a base 1 = 100%.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "DefenceMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double DefenceMultiplier { get; set; }

        ///<summary>Minions respawn timer in ms (default: 300000 = 5 mins).</summary>
        [DefaultValue(300000)]
        [JsonProperty(PropertyName = "MinionRespawnTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MinionRespawnTime { get; set; }

        ///<summary>Disable the penalty level paralize curse, false by default.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "DisableCurse", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool DisableCurse { get; set; }

        ///<summary>Configure the interval at which raid bosses and minions wont reconsider their target. This time is in seconds. Default: 30.</summary>
        [DefaultValue(30)]
        [JsonProperty(PropertyName = "RaidChaosTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RaidChaosTime { get; set; }

        ///<summary>Configure the interval at which raid bosses and minions wont reconsider their target. This time is in seconds. Default: 30.</summary>
        [DefaultValue(30)]
        [JsonProperty(PropertyName = "GrandChaosTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int GrandChaosTime { get; set; }

        ///<summary>Configure the interval at which raid bosses and minions wont reconsider their target. This time is in seconds. Default: 30.</summary>
        [DefaultValue(30)]
        [JsonProperty(PropertyName = "MinionChaosTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MinionChaosTime { get; set; }
    }

    ///<summary>Grand Boss.</summary>
    public class GrandBoss
    {
        ///<summary>Interval time. Value is hour.</summary>
        [JsonProperty(PropertyName = "SpawnInterval", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SpawnInterval { get; set; }

        ///<summary>Random interval. Value is hour.</summary>
        [JsonProperty(PropertyName = "RandomSpawn", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RandomSpawn { get; set; }

        ///<summary>Delay of appearance time. Value is minute.</summary>
        [JsonProperty(PropertyName = "WaitTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int WaitTime { get; set; }
    }

    ///<summary>GrandBosses.</summary>
    /// TODO: Review WaitTime * 60000 when read by json
    public class GrandBosses
    {
        ///<summary>Ant Queen.</summary>
        [JsonProperty(PropertyName = "AntQueen", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss AntQueen { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                    {
                                                                        SpawnInterval = 36,
                                                                        RandomSpawn = 17
                                                                    };

        ///<summary>Antharas.</summary>
        [JsonProperty(PropertyName = "Antharas", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Antharas { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                    {
                                                                        SpawnInterval = 264,
                                                                        RandomSpawn = 72,
                                                                        WaitTime = 30 * 60000
                                                                    };

        ///<summary>Baium.</summary>
        [JsonProperty(PropertyName = "Baium", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Baium { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                 {
                                                                     SpawnInterval = 168,
                                                                     RandomSpawn = 48
                                                                 };

        ///<summary>Core.</summary>
        [JsonProperty(PropertyName = "Core", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Core { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                {
                                                                    SpawnInterval = 60,
                                                                    RandomSpawn = 23
                                                                };

        ///<summary>Frintezza.</summary>
        [JsonProperty(PropertyName = "Frintezza", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Frintezza { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                     {
                                                                         SpawnInterval = 48,
                                                                         RandomSpawn = 8,
                                                                         WaitTime = 1 * 60000
                                                                     };

        ///<summary>Orfen.</summary>
        [JsonProperty(PropertyName = "Orfen", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Orfen { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                 {
                                                                     SpawnInterval = 48,
                                                                     RandomSpawn = 20
                                                                 };

        ///<summary>Sailren.</summary>
        [JsonProperty(PropertyName = "Sailren", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Sailren { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                   {
                                                                       SpawnInterval = 36,
                                                                       RandomSpawn = 24,
                                                                       WaitTime = 5 * 60000
                                                                   };

        ///<summary>Valakas.</summary>
        [JsonProperty(PropertyName = "Valakas", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Valakas { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                   {
                                                                       SpawnInterval = 264,
                                                                       RandomSpawn = 72,
                                                                       WaitTime = 30 * 60000
                                                                   };

        ///<summary>Zaken.</summary>
        [JsonProperty(PropertyName = "Zaken", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private GrandBoss Zaken { get; set; } = /*DefaultValue*/ new GrandBoss
                                                                 {
                                                                     SpawnInterval = 60,
                                                                     RandomSpawn = 20
                                                                 };
    }

    ///<summary>IA.</summary>
    public class Ia
    {
        ///<summary>If True, guards will attack at sight aggressive monsters.</summary>
        ///<summary>Default: False.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "GuardAttackAggroMob", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GuardAttackAggroMob { get; set; }

        ///<summary>Maximum range mobs can randomly go from spawn point.</summary>
        [DefaultValue(300)]
        [JsonProperty(PropertyName = "MaxDriftRange", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxDriftRange { get; set; }

        ///<summary>Interval (in milliseconds) in which the knownlist does full updates.</summary>
        ///<summary>For move based updates, it is used for intermediate updates.</summary>
        ///<summary>WARNING ! Interval must be between 300 - 2000. Too small value may kill your CPU, too high value may not update knownlists properly.</summary>
        ///<summary>Default: 1250.</summary>
        [DefaultValue(1250)]
        [JsonProperty(PropertyName = "KnownListUpdateInterval", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int KnownListUpdateInterval { get; set; }

        ///<summary>Minimum variable in seconds for npc animation delay.</summary>
        ///<summary>You must keep MinNPCAnimation lesser or equals than MaxNPCAnimation.</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "MinNPCAnimation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MinNpcAnimation { get; set; }

        ///<summary>Maximum maximum variable in seconds for npc animation delay.</summary>
        ///<summary>You must keep MinNPCAnimation lesser or equals than MaxNPCAnimation.</summary>
        [DefaultValue(40)]
        [JsonProperty(PropertyName = "MaxNPCAnimation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxNpcAnimation { get; set; }

        ///<summary>Minimum variable in seconds for monster animation delay.</summary>
        ///<summary>You must keep MinMonsterAnimation lesser or equals than MaxMonsterAnimation.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "MinMonsterAnimation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MinMonsterAnimation { get; set; }

        ///<summary>Maximum variable in seconds for monster animation delay.</summary>
        ///<summary>You must keep MinMonsterAnimation lesser or equals than MaxMonsterAnimation.</summary>
        [DefaultValue(40)]
        [JsonProperty(PropertyName = "MaxMonsterAnimation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxMonsterAnimation { get; set; }

        ///<summary>Grid options: Grids can now turn themselves on and off.  This also affects the loading.</summary>
        ///<summary>and processing of all AI tasks and (in the future) geodata within this grid.</summary>
        ///<summary>Turn on for a grid with a person in it is immediate, but it then turns on.</summary>
        ///<summary>the 8 neighboring grids based on the specified number of seconds.</summary>
        ///<summary>Turn off for self and neighbors occures after the specified number of.</summary>
        ///<summary>seconds have passed during which a grid has had no players in or in any of its neighbors.</summary>
        ///<summary>The always on option allows to ignore all this and let all grids be active at all times.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "GridsAlwaysOn", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GridsAlwaysOn { get; set; }

        [DefaultValue(1)]
        [JsonProperty(PropertyName = "GridNeighborTurnOnTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int GridNeighborTurnOnTime { get; set; }

        [DefaultValue(90)]
        [JsonProperty(PropertyName = "GridNeighborTurnOffTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int GridNeighborTurnOffTime { get; set; }
    }
}