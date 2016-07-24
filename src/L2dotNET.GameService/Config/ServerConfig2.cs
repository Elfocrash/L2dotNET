using System.ComponentModel;
using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    ///<summary>Server Config.</summary>
    public class ServerConfig2
    {
        ///<summary>Gameserver setting.</summary>
        [JsonProperty(PropertyName = "GameServer", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public GameServer GameServer { get; set; }

        ///<summary>Database informations.</summary>
        [JsonProperty(PropertyName = "GameDatabase", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public GameDatabase GameDatabase { get; set; }

        ///<summary>Server List.</summary>
        [JsonProperty(PropertyName = "ServerList", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ServerList ServerList { get; set; }

        ///<summary>Clients related options.</summary>
        [JsonProperty(PropertyName = "Client", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Client Client { get; set; }

        ///<summary>Jail & Punishements.</summary>
        [JsonProperty(PropertyName = "Punishement", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Punishement Punishement { get; set; }

        ///<summary>Automatic options.</summary>
        [JsonProperty(PropertyName = "Automatic", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Automatic Automatic { get; set; }

        ///<summary>Items Management.</summary>
        [JsonProperty(PropertyName = "ItemManagement", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ItemManagement ItemManagement { get; set; }

        ///<summary>Rates.</summary>
        [JsonProperty(PropertyName = "Rates", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Rates Rates { get; set; }

        ///<summary>Allowed features.</summary>
        [JsonProperty(PropertyName = "AllowedFeatures", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public AllowedFeatures AllowedFeatures { get; set; }

        ///<summary>Debug, Dev & Test config.</summary>
        [JsonProperty(PropertyName = "GameDeveloperConfig", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public GameDeveloperConfig GameDeveloperConfig { get; set; }

        ///<summary>Dead Lock Detector (thread detecting deadlocks).</summary>
        [JsonProperty(PropertyName = "DeadLockDetector", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DeadLockDetector DeadLockDetector { get; set; }

        ///<summary>Logging features.</summary>
        [JsonProperty(PropertyName = "Logging", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Logging Logging { get; set; }

        ///<summary>Community board configuration.</summary>
        [JsonProperty(PropertyName = "CommunityBoard", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public CommunityBoard CommunityBoard { get; set; }

        ///<summary>Flood Protectors.</summary>
        [JsonProperty(PropertyName = "FloodProtector", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public FloodProtector FloodProtector { get; set; }

        ///<summary>Misc.</summary>
        [JsonProperty(PropertyName = "Misc", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ServerMisc Misc { get; set; }
    }

    ///<summary>Gameserver setting.</summary>
    public class GameServer
    {
        ///<summary>Bind ip of the gameserver, use * to bind on all available IPs.</summary>
        [DefaultValue("*")]
        [JsonProperty(PropertyName = "GameserverHostname", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string GameserverHostname { get; set; }

        [DefaultValue(7777)]
        [JsonProperty(PropertyName = "GameserverPort", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int GameserverPort { get; set; }

        ///<summary>This is transmitted to the clients connecting from an external network, so it has to be a public IP or resolvable hostname.</summary>
        ///<summary>If this ip is resolvable by Login just leave *.</summary>
        [DefaultValue("*")]
        [JsonProperty(PropertyName = "ExternalHostname", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ExternalHostname { get; set; }

        ///<summary>This is transmitted to the client from the same network, so it has to be a local IP or resolvable hostname.</summary>
        ///<summary>If this ip is resolvable by Login just leave *.</summary>
        [DefaultValue("*")]
        [JsonProperty(PropertyName = "InternalHostname", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string InternalHostname { get; set; }

        ///<summary>The Loginserver port.</summary>
        [DefaultValue(9014)]
        [JsonProperty(PropertyName = "LoginPort", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int LoginPort { get; set; }

        ///<summary>The Loginserver host.</summary>
        [DefaultValue("localhost")]
        [JsonProperty(PropertyName = "LoginHost", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string LoginHost { get; set; }

        ///<summary>This is the server id that the gameserver will request.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "RequestServerID", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RequestServerId { get; set; }

        ///<summary>If set to true, the login will give an other id to the server (if the requested id is already reserved).</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AcceptAlternateID", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AcceptAlternateId { get; set; }
    }

    ///<summary>Database informations.</summary>
    public class GameDatabase
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "URL", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Url { get; set; }

        [DefaultValue("root")]
        [JsonProperty(PropertyName = "Login", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Login { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "Password", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Password { get; set; }

        ///<summary>Maximum database connections (minimum 2, basically 10 if number under 10, default 100).</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "MaximumDbConnections", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumDbConnections { get; set; }

        ///<summary>Idle connections expiration time (0 = never expire, default).</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "MaximumDbIdleTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumDbIdleTime { get; set; }
    }

    ///<summary>Server List.</summary>
    public class ServerList
    {
        ///<summary>Displays [] in front of server name.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ServerListBrackets", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ServerListBrackets { get; set; }

        ///<summary>Displays a clock next to the server name.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ServerListClock", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ServerListClock { get; set; }

        ///<summary>If True, the server will be set as GM only.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ServerGMOnly", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ServerGmOnly { get; set; }

        ///<summary>If True, the server will be a test server (listed by testserver clients only).</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "TestServer", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool TestServer { get; set; }
    }

    ///<summary>Clients related options.</summary>
    /// TODO: check protocol version
    /// if (MIN_PROTOCOL_REVISION > MAX_PROTOCOL_REVISION)
    ///     throw new Error("MinProtocolRevision is bigger than MaxProtocolRevision in server.properties.");
    public class Client
    {
        ///<summary>Allow delete chars after D days, 0 = feature disabled.</summary>
        [DefaultValue(7)]
        [JsonProperty(PropertyName = "DeleteCharAfterDays", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DeleteCharAfterDays { get; set; }

        ///<summary>Define how many players are allowed to play simultaneously on your server.</summary>
        [DefaultValue(100)]
        [JsonProperty(PropertyName = "MaximumOnlineUsers", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaximumOnlineUsers { get; set; }

        ///<summary>Minimum and maximum protocol revision that server allow to connect.</summary>
        ///<summary>You must keep MinProtocolRevision lesser or equals than MaxProtocolRevision.</summary>
        ///<summary>Default: 730.</summary>
        [DefaultValue(730)]
        [JsonProperty(PropertyName = "MinProtocolRevision", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MinProtocolRevision { get; set; }

        ///<summary>Default: 746.</summary>
        [DefaultValue(746)]
        [JsonProperty(PropertyName = "MaxProtocolRevision", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxProtocolRevision { get; set; }
    }

    ///<summary>Jail & Punishements.</summary>
    public class Punishement
    {
        ///<summary>Player punishment for illegal actions.</summary>
        ///<summary>1 - broadcast warning to gms only.</summary>
        ///<summary>2 - kick player(default).</summary>
        ///<summary>3 - kick & ban player.</summary>
        ///<summary>4 - jail player (define minutes of jail with param: 0 = infinite).</summary>
        [DefaultValue(2)]
        [JsonProperty(PropertyName = "DefaultPunish", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DefaultPunish { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "DefaultPunishParam", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DefaultPunishParam { get; set; }
    }

    ///<summary>Automatic options.</summary>
    public class Automatic
    {
        ///<summary>AutoLoot, don't lead herbs behavior. False by default.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AutoLoot", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AutoLoot { get; set; }

        ///<summary>AutoLoot from raid boss. False by default.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AutoLootRaid", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AutoLootRaid { get; set; }

        ///<summary>If False, herbs will drop on ground even if AutoLoot is enabled. False by default.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AutoLootHerbs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AutoLootHerbs { get; set; }
    }

    public class ItemTimer
    {
        [JsonProperty(PropertyName = "Id", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ItemId { get; set; }
        [JsonProperty(PropertyName = "Timer", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Timer { get; set; }
    }

    ///<summary>Items Management.</summary>
    /// TODO: Check Time * 1000
    public class ItemManagement
    {
        ///<summary>Allows players to drop items on the ground, default True.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowDiscardItem", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowDiscardItem { get; set; }

        ///<summary>Allows the creation of multiple non-stackable items at one time, default True.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "MultipleItemDrop", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool MultipleItemDrop { get; set; }

        ///<summary>Destroys dropped herbs after X seconds, set 0 to disable, default 15.</summary>
        [DefaultValue(15 * 1000)]
        [JsonProperty(PropertyName = "AutoDestroyHerbTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int AutoDestroyHerbTime { get; set; }

        ///<summary>Destroys dropped items after X seconds, set 0 to disable, default 600.</summary>
        [DefaultValue(600 * 1000)]
        [JsonProperty(PropertyName = "AutoDestroyItemTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int AutoDestroyItemTime { get; set; }

        ///<summary>Destroys dropped equipable items (armor, weapon, jewelry) after X seconds, set 0 to disable, default 0.</summary>
        [DefaultValue(0 * 1000)]
        [JsonProperty(PropertyName = "AutoDestroyEquipableItemTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int AutoDestroyEquipableItemTime { get; set; }

        ///<summary>Destroys dropped items after specified time. Ignores rules above, default 57-0,5575-0,6673-0.</summary>
        ///<summary>57-0: Item id 57 will never be destroyed.</summary>
        ///<summary>57-600: Item id 57 will be destroyed after 600 seconds/10 minutes.</summary>
        [JsonProperty(PropertyName = "AutoDestroySpecialItemTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ItemTimer[] AutoDestroySpecialItemTime { get; set; } = { new ItemTimer
        {
            ItemId = 57,
            Timer = 0
        },
            new ItemTimer
            {
                ItemId = 5575,
                Timer = 0
            },
            new ItemTimer
            {
                ItemId = 6673,
                Timer = 0
            } };

        ///<summary>Items dropped by players will have destroy time multiplied by X, default 1.</summary>
        ///<summary>0: Items dropped by players will never be destroyed.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "PlayerDroppedItemMultiplier", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerDroppedItemMultiplier { get; set; }

        ///<summary>Save dropped items into DB, restore them after reboot/start, default True.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "SaveDroppedItem", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SaveDroppedItem { get; set; }
    }

    ///<summary>Rates.</summary>
    public class Rates
    {
        ///<summary>Rate control, float values.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateXp", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateXp { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateSp", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateSp { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RatePartyXp", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RatePartyXp { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RatePartySp", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RatePartySp { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateDropAdena", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateDropAdena { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateConsumableCost", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateConsumableCost { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateDropItems", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateDropItems { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateRaidDropItems", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateRaidDropItems { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateDropSpoil", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateDropSpoil { get; set; }

        [DefaultValue(1)]
        [JsonProperty(PropertyName = "RateDropManor", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RateDropManor { get; set; }

        ///<summary>Quest configuration settings.</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateQuestDrop", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateQuestDrop { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateQuestReward", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateQuestReward { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateQuestRewardXP", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateQuestRewardXp { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateQuestRewardSP", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateQuestRewardSp { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateQuestRewardAdena", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateQuestRewardAdena { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateKarmaExpLost", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RateKarmaExpLost { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateSiegeGuardsPrice", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RateSiegeGuardsPrice { get; set; }

        ///<summary>Player Drop Rate control, percent (%) values.</summary>
        [DefaultValue(3)]
        [JsonProperty(PropertyName = "PlayerDropLimit", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerDropLimit { get; set; }

        [DefaultValue(5)]
        [JsonProperty(PropertyName = "PlayerRateDrop", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerRateDrop { get; set; }

        [DefaultValue(70)]
        [JsonProperty(PropertyName = "PlayerRateDropItem", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerRateDropItem { get; set; }

        [DefaultValue(25)]
        [JsonProperty(PropertyName = "PlayerRateDropEquip", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerRateDropEquip { get; set; }

        [DefaultValue(5)]
        [JsonProperty(PropertyName = "PlayerRateDropEquipWeapon", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PlayerRateDropEquipWeapon { get; set; }

        ///<summary>Karma Drop Rate control, percent (%) values.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "KarmaDropLimit", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int KarmaDropLimit { get; set; }

        [DefaultValue(70)]
        [JsonProperty(PropertyName = "KarmaRateDrop", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int KarmaRateDrop { get; set; }

        [DefaultValue(50)]
        [JsonProperty(PropertyName = "KarmaRateDropItem", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int KarmaRateDropItem { get; set; }

        [DefaultValue(40)]
        [JsonProperty(PropertyName = "KarmaRateDropEquip", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int KarmaRateDropEquip { get; set; }

        [DefaultValue(10d)]
        [JsonProperty(PropertyName = "KarmaRateDropEquipWeapon", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int KarmaRateDropEquipWeapon { get; set; }

        ///<summary>Pet rate control (float values except for "PetFoodRate", default 1./1/1.).</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "PetXpRate", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double PetXpRate { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "PetFoodRate", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int PetFoodRate { get; set; }

        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "SinEaterXpRate", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double SinEaterXpRate { get; set; }

        ///<summary>Common herbs (default).</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateCommonHerbs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateCommonHerbs { get; set; }

        ///<summary>Herb of Life (categorie 1).</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateHpHerbs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateHpHerbs { get; set; }

        ///<summary>Herb of Mana (categorie 2).</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateMpHerbs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateMpHerbs { get; set; }

        ///<summary>Special herbs (categorie 3).</summary>
        [DefaultValue(1.0)]
        [JsonProperty(PropertyName = "RateSpecialHerbs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public double RateSpecialHerbs { get; set; }
    }

    ///<summary>Allowed features.</summary>
    public class AllowedFeatures
    {
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowFreight", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowFreight { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowWarehouse", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowWarehouse { get; set; }

        ///<summary>If True, player can try on weapon and armor in shops.</summary>
        ///<summary>Each item tried cost WearPrice adena.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowWear", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowWear { get; set; }

        [DefaultValue(5)]
        [JsonProperty(PropertyName = "WearDelay", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int WearDelay { get; set; }

        [DefaultValue(10)]
        [JsonProperty(PropertyName = "WearPrice", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int WearPrice { get; set; }

        ///<summary>"Allow" types - Read variable name for info about ;p.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowLottery", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowLottery { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowWater", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowWater { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowCursedWeapons", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowCursedWeapons { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowManor", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowManor { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AllowBoat", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowBoat { get; set; }

        ///<summary>NOTE: Fishing will "bug" without geodata (if you activate w/o geodata, fishing is possible everywhere).</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AllowFishing", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowFishing { get; set; }

        ///<summary>Allow characters to receive damage from falling. CoordSynchronize = 2 is recommended.</summary>
        ///<summary>True - enabled.</summary>
        ///<summary>False - disabled.</summary>
        ///<summary>Auto - True if geodata enabled and False if disabled.</summary>
        ///<summary>Default: Auto.</summary>
        /// TODO: Check ENABLE_FALLING_DAMAGE
        [DefaultValue("auto")]
        [JsonProperty(PropertyName = "EnableFallingDamage", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string EnableFallingDamage { get; set; }
    }

    ///<summary>Debug, Dev & Test config.</summary>
    public class GameDeveloperConfig
    {
        ///<summary>Don't load spawns.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "NoSpawns", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool NoSpawns { get; set; }

        ///<summary>Debug messages (by default False, easily "flood" your GS logs).</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "Debug", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Debug { get; set; }

        [DefaultValue(false)]
        [JsonProperty(PropertyName = "Developer", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Developer { get; set; }

        [DefaultValue(false)]
        [JsonProperty(PropertyName = "PacketHandlerDebug", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool PacketHandlerDebug { get; set; }
    }

    ///<summary>Dead Lock Detector (thread detecting deadlocks).</summary>
    public class DeadLockDetector
    {
        ///<summary>Activate the feature (by default: False).</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "DeadLockDetector", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool EnableDeadLockDetector { get; set; }

        ///<summary>Check interval in seconds (by default: 20).</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "DeadLockCheckInterval", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DeadLockCheckInterval { get; set; }

        ///<summary>Automatic restart if deadlock case is found (by default: False).</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "RestartOnDeadlock", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool RestartOnDeadlock { get; set; }
    }

    ///<summary>Logging features.</summary>
    public class Logging
    {
        ///<summary>Logging ChatWindow.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "LogChat", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool LogChat { get; set; }

        ///<summary>Logging Item handling NOTE: This can be very space consuming.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "LogItems", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool LogItems { get; set; }

        ///<summary>Log GM actions.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "GMAudit", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool GmAudit { get; set; }
    }

    ///<summary>Community board configuration.</summary>
    public class CommunityBoard
    {
        ///<summary>Activate or no the community board.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "EnableCommunityBoard", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool EnableCommunityBoard { get; set; }

        ///<summary>Show this community board section when you open it.</summary>
        [DefaultValue("_bbshome")]
        [JsonProperty(PropertyName = "BBSDefault", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string BBSDefault { get; set; }
    }

    ///<summary>Flood Protectors.</summary>
    public class FloodProtector
    {
        ///<summary>The values are shown on ms. They can be setted to 0 to be disabled.</summary>
        [DefaultValue(4200)]
        [JsonProperty(PropertyName = "RollDiceTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RollDiceTime { get; set; }

        [DefaultValue(10000)]
        [JsonProperty(PropertyName = "HeroVoiceTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int HeroVoiceTime { get; set; }

        [DefaultValue(2000)]
        [JsonProperty(PropertyName = "SubclassTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SubclassTime { get; set; }

        [DefaultValue(1000)]
        [JsonProperty(PropertyName = "DropItemTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DropItemTime { get; set; }

        [DefaultValue(500)]
        [JsonProperty(PropertyName = "ServerBypassTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ServerBypassTime { get; set; }

        [DefaultValue(100)]
        [JsonProperty(PropertyName = "MultisellTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MultisellTime { get; set; }

        [DefaultValue(300)]
        [JsonProperty(PropertyName = "ManufactureTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ManufactureTime { get; set; }

        [DefaultValue(3000)]
        [JsonProperty(PropertyName = "ManorTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ManorTime { get; set; }

        [DefaultValue(10000)]
        [JsonProperty(PropertyName = "SendMailTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SendMailTime { get; set; }

        [DefaultValue(3000)]
        [JsonProperty(PropertyName = "CharacterSelectTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int CharacterSelectTime { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "GlobalChatTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int GlobalChatTime { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "TradeChatTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int TradeChatTime { get; set; }

        [DefaultValue(2000)]
        [JsonProperty(PropertyName = "SocialTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SocialTime { get; set; }
    }

    ///<summary>Misc.</summary>
    public class ServerMisc
    {
        ///<summary>Basic protection against L2Walker.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "L2WalkerProtection", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool L2WalkerProtection { get; set; }

        ///<summary>Delete invalid quest from player.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "AutoDeleteInvalidQuestData", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AutoDeleteInvalidQuestData { get; set; }

        ///<summary>Zone setting.</summary>
        ///<summary>0 = Peace All the Time.</summary>
        ///<summary>1 = PVP During Siege for siege participants.</summary>
        ///<summary>2 = PVP All the Time.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "ZoneTown", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ZoneTown { get; set; }

        ///<summary>Show "data/html/servnews.htm" when a character logins.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "ShowServerNews", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool ShowServerNews { get; set; }

        ///<summary>Disable tutorial on new player game entrance. Default: False.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "DisableTutorial", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool DisableTutorial { get; set; }
    }
}