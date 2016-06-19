using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class Player
    {
        ///<summary>Amount of adenas that a new character is given, default is 100.</summary>
        [JsonProperty(PropertyName = "StartingAdena")]
        public int StartingAdena { get; set; }

        ///<summary>If True, when effects of the same stack group are used, lesser.</summary>
        ///<summary>effects will be canceled if stronger effects are used. New effects.</summary>
        ///<summary>that are added will be canceled if they are of lesser priority to the old one.</summary>
        ///<summary>If False, they will not be canceled, and it will switch to them after the.</summary>
        ///<summary>stronger one runs out, if the lesser one is still in effect.</summary>
        [JsonProperty(PropertyName = "CancelLesserEffect")]
        public bool CancelLesserEffect { get; set; }

        ///<summary>% regeneration of normal regeneration speed - on a base 1 = 100%.</summary>
        [JsonProperty(PropertyName = "HpRegenMultiplier")]
        public double HpRegenMultiplier { get; set; }

        ///<summary>% regeneration of normal regeneration speed - on a base 1 = 100%.</summary>
        [JsonProperty(PropertyName = "MpRegenMultiplier")]
        public double MpRegenMultiplier { get; set; }

        ///<summary>% regeneration of normal regeneration speed - on a base 1 = 100%.</summary>
        [JsonProperty(PropertyName = "CpRegenMultiplier")]
        public double CpRegenMultiplier { get; set; }

        ///<summary>Player Protection after teleporting or login in seconds, 0 for disabled.</summary>
        [JsonProperty(PropertyName = "PlayerSpawnProtection")]
        public int PlayerSpawnProtection { get; set; }

        ///<summary>Player Protection from (agro) mobs after getting up from fake death; in seconds, 0 for disabled.</summary>
        [JsonProperty(PropertyName = "PlayerFakeDeathUpProtection")]
        public int PlayerFakeDeathUpProtection { get; set; }

        ///<summary>Amount of HP restored at revive - on a base 1 = 100%.</summary>
        [JsonProperty(PropertyName = "RespawnRestoreHP")]
        public double RespawnRestoreHP { get; set; }

        ///<summary>Maximum number of allowed slots for Private Stores (sell/buy) for dwarves.</summary>
        ///<summary>Normally, dwarves get 5 slots for pvt stores, while other races get only 4.</summary>
        [JsonProperty(PropertyName = "MaxPvtStoreSlotsDwarf")]
        public int MaxPvtStoreSlotsDwarf { get; set; }

        ///<summary>Maximum number of allowed slots for Private Stores (sell/buy) for all other races (except dwarves).</summary>
        ///<summary>Normally, dwarves get 5 slots for pvt stores, while other races get only 4.</summary>
        [JsonProperty(PropertyName = "MaxPvtStoreSlotsOther")]
        public int MaxPvtStoreSlotsOther { get; set; }

        ///<summary>If True, the following deep blue mobs' drop penalties will be applied:.</summary>
        ///<summary>- When player's level is 9 times greater than mob's level, drops got divided by 3.</summary>
        ///<summary>- After 9 lvl's of difference between player and deep blue mobs, drop chance is.</summary>
        ///<summary>lowered by 9% each lvl that difference increases. (9lvls diff = -9%; 10lvls diff = -18%; .).</summary>
        ///<summary>NOTE1: These rules are applied to both normal and sweep drops.</summary>
        ///<summary>NOTE2: These rules ignores the server's rate when drop is of adena type (Complies with retail server).</summary>
        [JsonProperty(PropertyName = "UseDeepBlueDropRules")]
        public bool UseDeepBlueDropRules { get; set; }

        ///<summary>XP loss (and deleveling) enabled, default is True.</summary>
        [JsonProperty(PropertyName = "Delevel")]
        public bool Delevel { get; set; }

        ///<summary>Death Penalty chance if killed by mob (in %), 20 by default.</summary>
        [JsonProperty(PropertyName = "DeathPenaltyChance")]
        public int DeathPenaltyChance { get; set; }

        ///<summary>Inventory.</summary>
        [JsonProperty(PropertyName = "Inventory")]
        public Inventory Inventory;

        ///<summary>Warehouse.</summary>
        [JsonProperty(PropertyName = "Warehouse")]
        public Warehouse Warehouse;

        ///<summary>Enchant.</summary>
        [JsonProperty(PropertyName = "Enchant")]
        public Enchant Enchant;

        ///<summary>Augmentation.</summary>
        [JsonProperty(PropertyName = "Augmentation")]
        public Augmentation Augmentation;

        ///<summary>Karma & PvP.</summary>
        [JsonProperty(PropertyName = "Combat")]
        public Combat Combat;

        ///<summary>Party.</summary>
        [JsonProperty(PropertyName = "Party")]
        public Party Party;

        ///<summary>GMs / Admin Stuff.</summary>
        [JsonProperty(PropertyName = "GM")]
        public GM GM;

        ///<summary>Petition.</summary>
        [JsonProperty(PropertyName = "Petition")]
        public Petition Petition;

        ///<summary>Crafting.</summary>
        [JsonProperty(PropertyName = "Crafting")]
        public Crafting Crafting;

        ///<summary>Skills / Classes.</summary>
        [JsonProperty(PropertyName = "Skill")]
        public Skill Skill;

        ///<summary>Buffs Config.</summary>
        [JsonProperty(PropertyName = "Buff")]
        public Buff Buff;
    }

    ///<summary>Inventory.</summary>
    public class Inventory
    {
        ///<summary>Inventory space limits.</summary>
        [JsonProperty(PropertyName = "MaximumSlotsForNoDwarf")]
        public int MaximumSlotsForNoDwarf { get; set; }

        ///<summary>Inventory space limits.</summary>
        [JsonProperty(PropertyName = "MaximumSlotsForDwarf")]
        public int MaximumSlotsForDwarf { get; set; }

        ///<summary>Inventory space limits.</summary>
        [JsonProperty(PropertyName = "MaximumSlotsForQuestItems")]
        public int MaximumSlotsForQuestItems { get; set; }

        ///<summary>Inventory space limits.</summary>
        [JsonProperty(PropertyName = "MaximumSlotsForPet")]
        public int MaximumSlotsForPet { get; set; }

        ///<summary>Weight Limit multiplier - default 1.</summary>
        [JsonProperty(PropertyName = "WeightLimit")]
        public int WeightLimit { get; set; }
    }

    ///<summary>Warehouse.</summary>
    public class Warehouse
    {
        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [JsonProperty(PropertyName = "MaximumWarehouseSlotsForDwarf")]
        public int MaximumWarehouseSlotsForDwarf { get; set; }

        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [JsonProperty(PropertyName = "MaximumWarehouseSlotsForNoDwarf")]
        public int MaximumWarehouseSlotsForNoDwarf { get; set; }

        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [JsonProperty(PropertyName = "MaximumWarehouseSlotsForClan")]
        public int MaximumWarehouseSlotsForClan { get; set; }

        ///<summary>Warehouse space limits (Must be lesser than 300 for prevent client crash).</summary>
        ///<summary>Dwarfs will get bonus from lvl 60.</summary>
        [JsonProperty(PropertyName = "MaximumFreightSlots")]
        public int MaximumFreightSlots { get; set; }

        ///<summary>Alternative Freight mode. If true, freights can be withdrawed from any place.</summary>
        ///<summary>Also, possibility to change Freight price (in adena) for each item slot in freight.</summary>
        ///<summary>NOTE: AltGameFreightPrice WILL NOT change the value shown to the player, but the.</summary>
        ///<summary>player will actually get charged for the value set in here.</summary>
        [JsonProperty(PropertyName = "GameFreights")]
        public bool GameFreights { get; set; }

        ///<summary>Alternative Freight mode. If true, freights can be withdrawed from any place.</summary>
        ///<summary>Also, possibility to change Freight price (in adena) for each item slot in freight.</summary>
        ///<summary>NOTE: AltGameFreightPrice WILL NOT change the value shown to the player, but the.</summary>
        ///<summary>player will actually get charged for the value set in here.</summary>
        [JsonProperty(PropertyName = "GameFreightPrice")]
        public int GameFreightPrice { get; set; }
    }

    ///<summary>Enchant.</summary>
    public class Enchant
    {
        ///<summary>% chance of success to enchant a magic weapon.</summary>
        [JsonProperty(PropertyName = "ChanceMagicWeapon")]
        public double ChanceMagicWeapon { get; set; }

        ///<summary>% chance of success to enchant a magic weapon +15.</summary>
        [JsonProperty(PropertyName = "ChanceMagicWeapon15Plus")]
        public double ChanceMagicWeapon15Plus { get; set; }

        ///<summary>% chance of success to enchant a non magic weapon.</summary>
        [JsonProperty(PropertyName = "ChanceNonMagicWeapon")]
        public double ChanceNonMagicWeapon { get; set; }

        ///<summary>% chance of success to enchant a non magic weapon +15.</summary>
        [JsonProperty(PropertyName = "ChanceNonMagicWeapon15Plus")]
        public double ChanceNonMagicWeapon15Plus { get; set; }

        ///<summary>% chance of success to enchant an armor part (both jewelry or armor).</summary>
        [JsonProperty(PropertyName = "ChanceArmor")]
        public double ChanceArmor { get; set; }

        ///<summary>Enchant limit [default = 0].</summary>
        [JsonProperty(PropertyName = "MaxWeapon")]
        public int MaxWeapon { get; set; }

        ///<summary>Enchant limit [default = 0].</summary>
        [JsonProperty(PropertyName = "MaxArmor")]
        public int MaxArmor { get; set; }

        ///<summary>if EnchantSafeMax is set to for ex '8' the item will be safly enchanted to '8' regardless of.</summary>
        ///<summary>enchant chance(default = 3 for EnchantSafeMax and default = 4 for EnchantSafeMaxFull).</summary>
        ///<summary>EnchantSafeMaxFull is for full body armor (upper and lower), value should be > 0.</summary>
        [JsonProperty(PropertyName = "SafeMax")]
        public int SafeMax { get; set; }

        ///<summary>if EnchantSafeMax is set to for ex '8' the item will be safly enchanted to '8' regardless of.</summary>
        ///<summary>enchant chance(default = 3 for EnchantSafeMax and default = 4 for EnchantSafeMaxFull).</summary>
        ///<summary>EnchantSafeMaxFull is for full body armor (upper and lower), value should be > 0.</summary>
        [JsonProperty(PropertyName = "SafeMaxFull")]
        public int SafeMaxFull { get; set; }
    }

    ///<summary>Augmentation.</summary>
    public class Augmentation
    {
        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 15.</summary>
        [JsonProperty(PropertyName = "NGSkillChance")]
        public int NGSkillChance { get; set; }

        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 30.</summary>
        [JsonProperty(PropertyName = "MidSkillChance")]
        public int MidSkillChance { get; set; }

        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 45.</summary>
        [JsonProperty(PropertyName = "HighSkillChance")]
        public int HighSkillChance { get; set; }

        ///<summary>Control the chance to get a skill in the augmentation process.</summary>
        ///<summary>Default: 60.</summary>
        [JsonProperty(PropertyName = "TopSkillChance")]
        public int TopSkillChance { get; set; }

        ///<summary>This controls the chance to get a base stat modifier in the augmentation process.</summary>
        ///<summary>Notes: This has no dependancy on the grade of Life Stone.</summary>
        ///<summary>Default: 1.</summary>
        [JsonProperty(PropertyName = "BaseStatChance")]
        public int BaseStatChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 0.</summary>
        [JsonProperty(PropertyName = "NGGlowChance")]
        public int NGGlowChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 40.</summary>
        [JsonProperty(PropertyName = "MidGlowChance")]
        public int MidGlowChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 70.</summary>
        [JsonProperty(PropertyName = "HighGlowChance")]
        public int HighGlowChance { get; set; }

        ///<summary>These control the chance to get a glow effect in the augmentation process.</summary>
        ///<summary>Notes: No/Mid Grade Life Stone can't have glow effect if you do not get a skill or base stat modifier.</summary>
        ///<summary>Default: 100.</summary>
        [JsonProperty(PropertyName = "TopGlowChance")]
        public int TopGlowChance { get; set; }
    }

    ///<summary>Karma & PvP.</summary>
    public class Combat
    {
        [JsonProperty(PropertyName = "KarmaPlayerCanBeKilledInPeaceZone")]
        public bool KarmaPlayerCanBeKilledInPeaceZone { get; set; }

        [JsonProperty(PropertyName = "KarmaPlayerCanShop")]
        public bool KarmaPlayerCanShop { get; set; }

        [JsonProperty(PropertyName = "KarmaPlayerCanTeleport")]
        public bool KarmaPlayerCanTeleport { get; set; }

        [JsonProperty(PropertyName = "KarmaPlayerCanUseGK")]
        public bool KarmaPlayerCanUseGK { get; set; }

        [JsonProperty(PropertyName = "KarmaPlayerCanTrade")]
        public bool KarmaPlayerCanTrade { get; set; }

        [JsonProperty(PropertyName = "KarmaPlayerCanUseWareHouse")]
        public bool KarmaPlayerCanUseWareHouse { get; set; }

        ///<summary>Equipment loss.</summary>
        [JsonProperty(PropertyName = "CanGMDropEquipment")]
        public bool CanGMDropEquipment { get; set; }

        ///<summary>List of pet items we cannot drop.</summary>
        [JsonProperty(PropertyName = "ListOfPetItems")]
        public int[] ListOfPetItems { get; set; }

        ///<summary>Lists of items which should NEVER be dropped by PKer.</summary>
        [JsonProperty(PropertyName = "ListOfNonDroppableItemsForPK")]
        public int[] ListOfNonDroppableItemsForPK { get; set; }

        ///<summary>Item drop related min/max.</summary>
        [JsonProperty(PropertyName = "MinimumPKRequiredToDrop")]
        public int MinimumPKRequiredToDrop { get; set; }

        ///<summary>Should we award a pvp point for killing a player with karma?.</summary>
        [JsonProperty(PropertyName = "AwardPKKillPVPPoint")]
        public bool AwardPKKillPVPPoint { get; set; }

        ///<summary>Length one stays in PvP mode after hitting an innocent (in ms).</summary>
        [JsonProperty(PropertyName = "PvPVsNormalTime")]
        public int PvPVsNormalTime { get; set; }

        ///<summary>Length one stays in PvP mode after hitting a purple player (in ms).</summary>
        [JsonProperty(PropertyName = "PvPVsPvPTime")]
        public int PvPVsPvPTime { get; set; }
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
        [JsonProperty(PropertyName = "XpCutoffMethod")]
        public string XpCutoffMethod { get; set; }

        ///<summary>This option takes effect when "percentage" method is chosen. Don't use high values for this!.</summary>
        ///<summary>Default: 3.0.</summary>
        [JsonProperty(PropertyName = "XpCutoffPercent")]
        public double XpCutoffPercent { get; set; }

        ///<summary>This option takes effect when "level" method is chosen. Don't use low values for this!.</summary>
        ///<summary>Default: 20.</summary>
        [JsonProperty(PropertyName = "XpCutoffLevel")]
        public int XpCutoffLevel { get; set; }

        ///<summary>Party range for l2attackable (default 1600).</summary>
        [JsonProperty(PropertyName = "Range")]
        public int Range { get; set; }

        ///<summary>Party range for l2party (default 1400).</summary>
        [JsonProperty(PropertyName = "Range2")]
        public int Range2 { get; set; }

        ///<summary>If True, when the party leader leaves the party, the next member in party will be the leader.</summary>
        ///<summary>If False, the party will be dispersed.</summary>
        ///<summary>Default: False.</summary>
        [JsonProperty(PropertyName = "LeavePartyLeader")]
        public bool LeavePartyLeader { get; set; }
    }

    ///<summary>GMs / Admin Stuff.</summary>
    public class GM
    {
        ///<summary>If next switch is set to true every newly created character will have access level 200.</summary>
        [JsonProperty(PropertyName = "EverybodyHasAdminRights")]
        public bool EverybodyHasAdminRights { get; set; }

        ///<summary>This option will set the default AccessLevel for MasterAccess.</summary>
        ///<summary>Characters set to this AccessLevel will have the right to execute every AdminCommand ingame.</summary>
        ///<summary>Default: 127 (Maximum value: 255).</summary>
        [JsonProperty(PropertyName = "MasterAccessLevel")]
        public byte MasterAccessLevel { get; set; }

        ///<summary>Name color for those matching the above MasterAccess AccessLevel.</summary>
        ///<summary>Default: 00CCFF (golden color).</summary>
        [JsonProperty(PropertyName = "MasterNameColor")]
        public string MasterNameColor { get; set; }

        ///<summary>Title color for those matching the above MasterAccess AccessLevel.</summary>
        ///<summary>Default: 00CCFF (golden color).</summary>
        [JsonProperty(PropertyName = "MasterTitleColor")]
        public string MasterTitleColor { get; set; }

        ///<summary>Enable GMs to have the glowing aura of a Hero character.</summary>
        [JsonProperty(PropertyName = "GMHeroAura")]
        public bool GMHeroAura { get; set; }

        ///<summary>Auto set invulnerable status to a GM on login.</summary>
        [JsonProperty(PropertyName = "GMStartupInvulnerable")]
        public bool GMStartupInvulnerable { get; set; }

        ///<summary>Auto set invisible status to a GM on login.</summary>
        [JsonProperty(PropertyName = "GMStartupInvisible")]
        public bool GMStartupInvisible { get; set; }

        ///<summary>Auto block private messages to a GM on login.</summary>
        [JsonProperty(PropertyName = "GMStartupSilence")]
        public bool GMStartupSilence { get; set; }

        ///<summary>Auto list GMs in GM list (/gmlist) on login.</summary>
        [JsonProperty(PropertyName = "GMStartupAutoList")]
        public bool GMStartupAutoList { get; set; }
    }

    ///<summary>Petition.</summary>
    public class Petition
    {
        ///<summary>Enable players to send in-game petitions.</summary>
        [JsonProperty(PropertyName = "PetitioningAllowed")]
        public bool PetitioningAllowed { get; set; }

        ///<summary>Total number of petitions to allow per player, per session.</summary>
        [JsonProperty(PropertyName = "MaxPetitionsPerPlayer")]
        public int MaxPetitionsPerPlayer { get; set; }

        ///<summary>Total number of petitions pending, if more are submitted they will be rejected.</summary>
        [JsonProperty(PropertyName = "MaxPetitionsPending")]
        public int MaxPetitionsPending { get; set; }
    }

    ///<summary>Crafting.</summary>
    public class Crafting
    {
        ///<summary>Crafting enabled/disabled. True by default.</summary>
        [JsonProperty(PropertyName = "CraftingEnabled")]
        public bool CraftingEnabled { get; set; }

        ///<summary>Limits for recipes (default : 50).</summary>
        [JsonProperty(PropertyName = "DwarfRecipeLimit")]
        public int DwarfRecipeLimit { get; set; }

        ///<summary>Limits for recipes (default : 50).</summary>
        [JsonProperty(PropertyName = "CommonRecipeLimit")]
        public int CommonRecipeLimit { get; set; }

        ///<summary>If set to False, blacksmiths don't take recipes from players inventory when crafting. Default = True (retail).</summary>
        [JsonProperty(PropertyName = "BlacksmithUseRecipes")]
        public bool BlacksmithUseRecipes { get; set; }
    }

    ///<summary>Skills / Classes.</summary>
    public class Skill
    {
        ///<summary>AutoLearnSkills. True to enable, False to disable.</summary>
        [JsonProperty(PropertyName = "AutoLearnSkills")]
        public bool AutoLearnSkills { get; set; }

        ///<summary>If disabled, magic dmg has always 100% chance of success, default is 'true'.</summary>
        [JsonProperty(PropertyName = "MagicFailures")]
        public bool MagicFailures { get; set; }

        ///<summary>Alternative rules for shields - if they block, the damage is powerAtk-shieldDef,.</summary>
        ///<summary>otherwice it's powerAttak / (shieldDef + powerDef).</summary>
        [JsonProperty(PropertyName = "ShieldBlocks")]
        public bool ShieldBlocks { get; set; }

        ///<summary>Alternative Rate Value for Perfect Shield Block Rate.</summary>
        [JsonProperty(PropertyName = "PerfectShieldBlockRate")]
        public int PerfectShieldBlockRate { get; set; }

        ///<summary>Life crystal needed to learn clan skills.</summary>
        ///<summary>Default: True.</summary>
        [JsonProperty(PropertyName = "LifeCrystalNeeded")]
        public bool LifeCrystalNeeded { get; set; }

        ///<summary>Spell book needed to learn skills.</summary>
        ///<summary>Default: True.</summary>
        [JsonProperty(PropertyName = "SpBookNeeded")]
        public bool SpBookNeeded { get; set; }

        ///<summary>Spell book needed to enchant skills.</summary>
        ///<summary>Default: True.</summary>
        [JsonProperty(PropertyName = "EnchantSkillSpBookNeeded")]
        public bool EnchantSkillSpBookNeeded { get; set; }

        ///<summary>Spell book needed to learn Divine Inspiration.</summary>
        ///<summary>Default: True.</summary>
        [JsonProperty(PropertyName = "DivineInspirationSpBookNeeded")]
        public bool DivineInspirationSpBookNeeded { get; set; }

        ///<summary>Allow player subclass addition without checking for unique quest items.</summary>
        [JsonProperty(PropertyName = "SubClassWithoutQuests")]
        public bool SubClassWithoutQuests { get; set; }
    }

    ///<summary>Buffs Config.</summary>
    public class Buff
    {
        ///<summary>Maximum number of buffs.</summary>
        ///<summary>Remember that Divine Inspiration will give 4 additional buff slots on top of the number specified.</summary>
        ///<summary>Default: 20.</summary>
        [JsonProperty(PropertyName = "MaxBuffsAmount")]
        public int MaxBuffsAmount { get; set; }

        ///<summary>Store buffs/debuffs on user logout?.</summary>
        [JsonProperty(PropertyName = "StoreSkillCooltime")]
        public bool StoreSkillCooltime { get; set; }
    }
}