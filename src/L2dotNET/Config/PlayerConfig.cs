using System.ComponentModel;
using Newtonsoft.Json;

namespace L2dotNET.Config
{
    ///<summary>Player Config.</summary>
    public class PlayerConfig
    {
        ///<summary>Amount of adenas that a new character is given, default is 100.</summary>
        [DefaultValue(100)]
        [JsonProperty(PropertyName = "StartingAdena", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int StartingAdena { get; set; }

        ///<summary>If True, when effects of the same stack group are used, lesser.</summary>
        ///<summary>effects will be canceled if stronger effects are used. New effects.</summary>
        ///<summary>that are added will be canceled if they are of lesser priority to the old one.</summary>
        ///<summary>If False, they will not be canceled, and it will switch to them after the.</summary>
        ///<summary>stronger one runs out, if the lesser one is still in effect.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "CancelLesserEffect", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool CancelLesserEffect { get; set; }

        ///<summary>% regeneration of normal regeneration speed - on a base 1 = 100%.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "HpRegenMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double HpRegenMultiplier { get; set; }

        ///<summary>% regeneration of normal regeneration speed - on a base 1 = 100%.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "MpRegenMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double MpRegenMultiplier { get; set; }

        ///<summary>% regeneration of normal regeneration speed - on a base 1 = 100%.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "CpRegenMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double CpRegenMultiplier { get; set; }

        ///<summary>Player Protection after teleporting or login in seconds, 0 for disabled.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "PlayerSpawnProtection", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerSpawnProtection { get; set; }

        ///<summary>Player Protection from (agro) mobs after getting up from fake death; in seconds, 0 for disabled.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "PlayerFakeDeathUpProtection", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerFakeDeathUpProtection { get; set; }

        ///<summary>Amount of HP restored at revive - on a base 1 = 100%.</summary>
        [DefaultValue(0.7)]
        [JsonProperty(PropertyName = "RespawnRestoreHP", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RespawnRestoreHp { get; set; }

        ///<summary>Maximum number of allowed slots for Private Stores (sell/buy) for dwarves.</summary>
        ///<summary>Normally, dwarves get 5 slots for pvt stores, while other races get only 4.</summary>
        [DefaultValue(5)]
        [JsonProperty(PropertyName = "MaxPvtStoreSlotsDwarf", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxPvtStoreSlotsDwarf { get; set; }

        ///<summary>Maximum number of allowed slots for Private Stores (sell/buy) for all other races (except dwarves).</summary>
        ///<summary>Normally, dwarves get 5 slots for pvt stores, while other races get only 4.</summary>
        [DefaultValue(4)]
        [JsonProperty(PropertyName = "MaxPvtStoreSlotsOther", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxPvtStoreSlotsOther { get; set; }

        ///<summary>If True, the following deep blue mobs' drop penalties will be applied:.</summary>
        ///<summary>- When player's level is 9 times greater than mob's level, drops got divided by 3.</summary>
        ///<summary>- After 9 lvl's of difference between player and deep blue mobs, drop chance is.</summary>
        ///<summary>lowered by 9% each lvl that difference increases. (9lvls diff = -9%; 10lvls diff = -18%; .).</summary>
        ///<summary>NOTE1: These rules are applied to both normal and sweep drops.</summary>
        ///<summary>NOTE2: These rules ignores the server's rate when drop is of adena type (Complies with retail server).</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "UseDeepBlueDropRules", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool UseDeepBlueDropRules { get; set; }

        ///<summary>XP loss (and deleveling) enabled, default is True.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "Delevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Delevel { get; set; }

        ///<summary>Death Penalty chance if killed by mob (in %), 20 by default.</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "DeathPenaltyChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DeathPenaltyChance { get; set; }

        ///<summary>Inventory.</summary>
        [JsonProperty(PropertyName = "Inventory", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Inventory Inventory;

        ///<summary>Warehouse.</summary>
        [JsonProperty(PropertyName = "Warehouse", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Warehouse Warehouse;

        ///<summary>Enchant.</summary>
        [JsonProperty(PropertyName = "Enchant", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Enchant Enchant;

        ///<summary>Augmentation.</summary>
        [JsonProperty(PropertyName = "Augmentation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Augmentation Augmentation;

        ///<summary>Karma & PvP.</summary>
        [JsonProperty(PropertyName = "Combat", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Combat Combat;

        ///<summary>Party.</summary>
        [JsonProperty(PropertyName = "Party", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Party Party;

        ///<summary>GMs / Admin Stuff.</summary>
        [JsonProperty(PropertyName = "GM", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Gm Gm;

        ///<summary>Petition.</summary>
        [JsonProperty(PropertyName = "Petition", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Petition Petition;

        ///<summary>Crafting.</summary>
        [JsonProperty(PropertyName = "Crafting", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Crafting Crafting;

        ///<summary>Skills / Classes.</summary>
        [JsonProperty(PropertyName = "Skill", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Skill Skill;

        ///<summary>Buffs Config.</summary>
        [JsonProperty(PropertyName = "Buff", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Buff Buff;
    }

    ///<summary>Inventory.</summary>
    public class Inventory
    {
        ///<summary>Inventory space limits.</summary>
        [JsonProperty(PropertyName = "MaximumSlotsForNoDwarf", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(80)]
        public int MaximumSlotsForNoDwarf { get; set; }

        ///<summary>Inventory space limits.</summary>
        [JsonProperty(PropertyName = "MaximumSlotsForDwarf", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(100)]
        public int MaximumSlotsForDwarf { get; set; }

        ///<summary>Inventory space limits.</summary>
        [DefaultValue(100)]
        [JsonProperty(PropertyName = "MaximumSlotsForQuestItems", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumSlotsForQuestItems { get; set; }

        ///<summary>Inventory space limits.</summary>
        [DefaultValue(12)]
        [JsonProperty(PropertyName = "MaximumSlotsForPet", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumSlotsForPet { get; set; }

        ///TODO: Missing MAX_ITEM_IN_PACKET Math.max(INVENTORY_MAXIMUM_NO_DWARF, INVENTORY_MAXIMUM_DWARF);
        ///<summary>Weight Limit multiplier - default 1.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "WeightLimit", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int WeightLimit { get; set; }
    }

    ///<summary>Warehouse.</summary>
    public class Warehouse
    {
        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [DefaultValue(100)]
        [JsonProperty(PropertyName = "MaximumWarehouseSlotsForNoDwarf", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumWarehouseSlotsForNoDwarf { get; set; }

        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [DefaultValue(120)]
        [JsonProperty(PropertyName = "MaximumWarehouseSlotsForDwarf", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumWarehouseSlotsForDwarf { get; set; }

        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [DefaultValue(150)]
        [JsonProperty(PropertyName = "MaximumWarehouseSlotsForClan", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumWarehouseSlotsForClan { get; set; }

        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "MaximumFreightSlots", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumFreightSlots { get; set; }

        ///<summary>Alternative Freight mode. If true, freights can be withdrawed from any place.</summary>
        ///<summary>Also, possibility to change Freight price (in adena) for each item slot in freight.</summary>
        ///<summary>NOTE: AltGameFreightPrice WILL NOT change the value shown to the player, but the.</summary>
        ///<summary>player will actually get charged for the value set in here.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "GameFreights", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GameFreights { get; set; }

        ///<summary>Alternative Freight mode. If true, freights can be withdrawed from any place.</summary>
        ///<summary>Also, possibility to change Freight price (in adena) for each item slot in freight.</summary>
        ///<summary>NOTE: AltGameFreightPrice WILL NOT change the value shown to the player, but the.</summary>
        ///<summary>player will actually get charged for the value set in here.</summary>
        [DefaultValue(1000)]
        [JsonProperty(PropertyName = "GameFreightPrice", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int GameFreightPrice { get; set; }
    }

    ///<summary>Enchant.</summary>
    public class Enchant
    {
        ///<summary>% chance of success to enchant a magic weapon.</summary>
        [DefaultValue(0.4)]
        [JsonProperty(PropertyName = "ChanceMagicWeapon", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double ChanceMagicWeapon { get; set; }

        ///<summary>% chance of success to enchant a magic weapon +15.</summary>
        [DefaultValue(0.2)]
        [JsonProperty(PropertyName = "ChanceMagicWeapon15Plus", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double ChanceMagicWeapon15Plus { get; set; }

        ///<summary>% chance of success to enchant a non magic weapon.</summary>
        [DefaultValue(0.7)]
        [JsonProperty(PropertyName = "ChanceNonMagicWeapon", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double ChanceNonMagicWeapon { get; set; }

        ///<summary>% chance of success to enchant a non magic weapon +15.</summary>
        [DefaultValue(0.35)]
        [JsonProperty(PropertyName = "ChanceNonMagicWeapon15Plus", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double ChanceNonMagicWeapon15Plus { get; set; }

        ///<summary>% chance of success to enchant an armor part (both jewelry or armor).</summary>
        [DefaultValue(0.66)]
        [JsonProperty(PropertyName = "ChanceArmor", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double ChanceArmor { get; set; }

        ///<summary>Enchant limit [default = 0].</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "MaxWeapon", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxWeapon { get; set; }

        ///<summary>Enchant limit [default = 0].</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "MaxArmor", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxArmor { get; set; }

        ///<summary>if EnchantSafeMax is set to for ex '8' the item will be safly enchanted to '8' regardless of.</summary>
        ///<summary>enchant chance(default = 3 for EnchantSafeMax and default = 4 for EnchantSafeMaxFull).</summary>
        ///<summary>EnchantSafeMaxFull is for full body armor (upper and lower), value should be > 0.</summary>
        [DefaultValue(3)]
        [JsonProperty(PropertyName = "SafeMax", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SafeMax { get; set; }

        ///<summary>if EnchantSafeMax is set to for ex '8' the item will be safly enchanted to '8' regardless of.</summary>
        ///<summary>enchant chance(default = 3 for EnchantSafeMax and default = 4 for EnchantSafeMaxFull).</summary>
        ///<summary>EnchantSafeMaxFull is for full body armor (upper and lower), value should be > 0.</summary>
        [DefaultValue(4)]
        [JsonProperty(PropertyName = "SafeMaxFull", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SafeMaxFull { get; set; }
    }

    ///<summary>Augmentation.</summary>
    /// TODO: Create attributes to Chance/Glow rate
    public class Augmentation
    {
        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 15.</summary>
        [DefaultValue(15)]
        [JsonProperty(PropertyName = "NGSkillChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int NgSkillChance { get; set; }

        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 30.</summary>
        [DefaultValue(30)]
        [JsonProperty(PropertyName = "MidSkillChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MidSkillChance { get; set; }

        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 45.</summary>
        [DefaultValue(45)]
        [JsonProperty(PropertyName = "HighSkillChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int HighSkillChance { get; set; }

        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 60.</summary>
        [DefaultValue(60)]
        [JsonProperty(PropertyName = "TopSkillChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int TopSkillChance { get; set; }

        ///<summary>This controls the chance to get a base stat modifier in the augmentation process.</summary>
        ///<summary>Notes: This has no dependancy on the grade of Life Stone.</summary>
        ///<summary>Default: 1.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "BaseStatChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int BaseStatChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 0.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "NGGlowChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int NgGlowChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 40.</summary>
        [DefaultValue(40)]
        [JsonProperty(PropertyName = "MidGlowChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MidGlowChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 70.</summary>
        [DefaultValue(70)]
        [JsonProperty(PropertyName = "HighGlowChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int HighGlowChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 100.</summary>
        [DefaultValue(100)]
        [JsonProperty(PropertyName = "TopGlowChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int TopGlowChance { get; set; }
    }

    ///<summary>Karma & PvP.</summary>
    public class Combat
    {
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "KarmaPlayerCanBeKilledInPeaceZone", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool KarmaPlayerCanBeKilledInPeaceZone { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "KarmaPlayerCanShop", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool KarmaPlayerCanShop { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "KarmaPlayerCanTeleport", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool KarmaPlayerCanTeleport { get; set; }

        [DefaultValue(false)]
        [JsonProperty(PropertyName = "KarmaPlayerCanUseGK", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool KarmaPlayerCanUseGk { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "KarmaPlayerCanTrade", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool KarmaPlayerCanTrade { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "KarmaPlayerCanUseWareHouse", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool KarmaPlayerCanUseWareHouse { get; set; }

        ///<summary>GM Equipment loss.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "CanGMDropEquipment", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool CanGmDropEquipment { get; set; }

        ///<summary>List of pet items we cannot drop.</summary>
        [DefaultValue(new[] { 2375, 3500, 3501, 3502, 4422, 4423, 4424, 4425, 6648, 6649, 6650 })]
        [JsonProperty(PropertyName = "ListOfPetItems", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int[] ListOfPetItems { get; set; }

        ///<summary>Lists of items which should NEVER be dropped by PKer.</summary>
        [DefaultValue(new[] { 1147, 425, 1146, 461, 10, 2368, 7, 6, 2370, 2369 })]
        [JsonProperty(PropertyName = "ListOfNonDroppableItemsForPK", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int[] ListOfNonDroppableItemsForPk { get; set; }

        ///<summary>Item drop related min/max.</summary>
        [DefaultValue(5)]
        [JsonProperty(PropertyName = "MinimumPKRequiredToDrop", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MinimumPkRequiredToDrop { get; set; }

        ///<summary>Should we award a pvp point for killing a player with karma?.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AwardPKKillPVPPoint", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AwardPkKillPvpPoint { get; set; }

        ///<summary>Length one stays in PvP mode after hitting an innocent (in ms).</summary>
        [DefaultValue(15000)]
        [JsonProperty(PropertyName = "PvPVsNormalTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PvPVsNormalTime { get; set; }

        ///<summary>Length one stays in PvP mode after hitting a purple player (in ms).</summary>
        [DefaultValue(30000)]
        [JsonProperty(PropertyName = "PvPVsPvPTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PvPvsPvPTime { get; set; }
    }

    ///<summary>Party.</summary>
    public class Party
    {
        ///<summary>PARTY XP DISTRIBUTION.</summary>
        ///<summary>With "auto method" member is cut from Exp/SP distribution when his share is lower than party bonus acquired for him (30% for 2 member party).</summary>
        ///<summary>In that case he will not receive any Exp/SP from party and is not counted for party bonus.</summary>
        ///<summary>If you don't want to have a cutoff point for party members' XP distribution, set the first option to "none".</summary>
        ///<summary>Available Options: auto, level, percentage, none.</summary>
        ///<summary>Default: level.</summary>
        [DefaultValue("level")]
        [JsonProperty(PropertyName = "XpCutoffMethod", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string XpCutoffMethod { get; set; }

        ///<summary>This option takes effect when "percentage" method is chosen. Don't use high values for this!.</summary>
        ///<summary>Default: 3.0.</summary>
        [DefaultValue(3.0)]
        [JsonProperty(PropertyName = "XpCutoffPercent", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double XpCutoffPercent { get; set; }

        ///<summary>This option takes effect when "level" method is chosen. Don't use low values for this!.</summary>
        ///<summary>Default: 20.</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "XpCutoffLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int XpCutoffLevel { get; set; }

        ///<summary>Party range for l2attackable (default 1600).</summary>
        [DefaultValue(1600)]
        [JsonProperty(PropertyName = "Range", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Range { get; set; }

        ///<summary>Party range for l2party (default 1400).</summary>
        [DefaultValue(1400)]
        [JsonProperty(PropertyName = "Range2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Range2 { get; set; }

        ///<summary>If True, when the party leader leaves the party, the next member in party will be the leader.</summary>
        ///<summary>If False, the party will be dispersed.</summary>
        ///<summary>Default: False.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "LeavePartyLeader", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool LeavePartyLeader { get; set; }
    }

    ///<summary>GMs / Admin Stuff.</summary>
    public class Gm
    {
        ///<summary>If next switch is set to true every newly created character will have access level 200.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "EverybodyHasAdminRights", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool EverybodyHasAdminRights { get; set; }

        ///<summary>This option will set the default AccessLevel for MasterAccess.</summary>
        ///<summary>Characters set to this AccessLevel will have the right to execute every AdminCommand ingame.</summary>
        ///<summary>Default: 127 (Maximum value: 255).</summary>
        [DefaultValue(127)]
        [JsonProperty(PropertyName = "MasterAccessLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public byte MasterAccessLevel { get; set; }

        ///<summary>Name color for those matching the above MasterAccess AccessLevel.</summary>
        ///<summary>Default: 00CCFF (golden color).</summary>
        [DefaultValue("00FF00")]
        [JsonProperty(PropertyName = "MasterNameColor", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string MasterNameColor { get; set; }

        ///<summary>Title color for those matching the above MasterAccess AccessLevel.</summary>
        ///<summary>Default: 00CCFF (golden color).</summary>
        [DefaultValue("00FF00")]
        [JsonProperty(PropertyName = "MasterTitleColor", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string MasterTitleColor { get; set; }

        ///<summary>Enable GMs to have the glowing aura of a Hero character.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "GMHeroAura", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GmHeroAura { get; set; }

        ///<summary>Auto set invulnerable status to a GM on login.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "GMStartupInvulnerable", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GmStartupInvulnerable { get; set; }

        ///<summary>Auto set invisible status to a GM on login.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "GMStartupInvisible", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GmStartupInvisible { get; set; }

        ///<summary>Auto block private messages to a GM on login.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "GMStartupSilence", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GmStartupSilence { get; set; }

        ///<summary>Auto list GMs in GM list (/gmlist) on login.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "GMStartupAutoList", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GmStartupAutoList { get; set; }
    }

    ///<summary>Petition.</summary>
    public class Petition
    {
        ///<summary>Enable players to send in-game petitions.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "PetitioningAllowed", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool PetitioningAllowed { get; set; }

        ///<summary>Total number of petitions to allow per player, per session.</summary>
        [DefaultValue(5)]
        [JsonProperty(PropertyName = "MaxPetitionsPerPlayer", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxPetitionsPerPlayer { get; set; }

        ///<summary>Total number of petitions pending, if more are submitted they will be rejected.</summary>
        [DefaultValue(25)]
        [JsonProperty(PropertyName = "MaxPetitionsPending", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxPetitionsPending { get; set; }
    }

    ///<summary>Crafting.</summary>
    public class Crafting
    {
        ///<summary>Crafting enabled/disabled. True by default.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "CraftingEnabled", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool CraftingEnabled { get; set; }

        ///<summary>Limits for recipes (Default: 50).</summary>
        [DefaultValue(50)]
        [JsonProperty(PropertyName = "DwarfRecipeLimit", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DwarfRecipeLimit { get; set; }

        ///<summary>Limits for recipes (Default: 50).</summary>
        [DefaultValue(50)]
        [JsonProperty(PropertyName = "CommonRecipeLimit", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int CommonRecipeLimit { get; set; }

        ///<summary>If set to False, blacksmiths don't take recipes from players inventory when crafting. Default = True (retail).</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "BlacksmithUseRecipes", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool BlacksmithUseRecipes { get; set; }
    }

    ///<summary>Skills / Classes.</summary>
    public class Skill
    {
        ///<summary>AutoLearnSkills. True to enable, False to disable.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AutoLearnSkills", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AutoLearnSkills { get; set; }

        ///<summary>If disabled, magic dmg has always 100% chance of success, default is 'true'.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "MagicFailures", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool MagicFailures { get; set; }

        ///<summary>Alternative rules for shields - if they block, the damage is powerAtk-shieldDef,.</summary>
        ///<summary>otherwice it's powerAttak / (shieldDef + powerDef).</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ShieldBlocks", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ShieldBlocks { get; set; }

        ///<summary>Alternative Rate Value for Perfect Shield Block Rate.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "PerfectShieldBlockRate", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PerfectShieldBlockRate { get; set; }

        ///<summary>Life crystal needed to learn clan skills.</summary>
        ///<summary>Default: True.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "LifeCrystalNeeded", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool LifeCrystalNeeded { get; set; }

        ///<summary>Spell book needed to learn skills.</summary>
        ///<summary>Default: True.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "SpBookNeeded", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SpBookNeeded { get; set; }

        ///<summary>Spell book needed to enchant skills.</summary>
        ///<summary>Default: True.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "EnchantSkillSpBookNeeded", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool EnchantSkillSpBookNeeded { get; set; }

        ///<summary>Spell book needed to learn Divine Inspiration.</summary>
        ///<summary>Default: True.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "DivineInspirationSpBookNeeded", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool DivineInspirationSpBookNeeded { get; set; }

        ///<summary>Allow player subclass addition without checking for unique quest items.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "SubClassWithoutQuests", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SubClassWithoutQuests { get; set; }
    }

    ///<summary>Buffs Config.</summary>
    public class Buff
    {
        ///<summary>Maximum number of buffs.</summary>
        ///<summary>Remember that Divine Inspiration will give 4 additional buff slots on top of the number specified.</summary>
        ///<summary>Default: 20.</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "MaxBuffsAmount", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxBuffsAmount { get; set; }

        ///<summary>Store buffs/debuffs on user logout?.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "StoreSkillCooltime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool StoreSkillCooltime { get; set; }
    }
}