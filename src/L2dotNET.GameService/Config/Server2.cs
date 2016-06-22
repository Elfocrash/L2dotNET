using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class Server2
    {
        ///<summary>Gameserver setting.</summary>
        [JsonProperty(PropertyName = "GameServer")]
        public GameServer GameServer { get; set; }

        ///<summary>Database informations.</summary>
        [JsonProperty(PropertyName = "GameDatabase")]
        public GameDatabase GameDatabase { get; set; }

        ///<summary>Server List.</summary>
        [JsonProperty(PropertyName = "ServerList")]
        public ServerList ServerList { get; set; }

        ///<summary>Clients related options.</summary>
        [JsonProperty(PropertyName = "Client")]
        public Client Client { get; set; }

        ///<summary>Jail & Punishements.</summary>
        [JsonProperty(PropertyName = "Punishement")]
        public Punishement Punishement { get; set; }

        ///<summary>Automatic options.</summary>
        [JsonProperty(PropertyName = "Automatic")]
        public Automatic Automatic { get; set; }

        ///<summary>Items Management.</summary>
        [JsonProperty(PropertyName = "ItemManagement")]
        public ItemManagement ItemManagement { get; set; }

        ///<summary>Rates.</summary>
        [JsonProperty(PropertyName = "Rates")]
        public Rates Rates { get; set; }

        ///<summary>Allowed features.</summary>
        [JsonProperty(PropertyName = "AllowedFeatures")]
        public AllowedFeatures AllowedFeatures { get; set; }

        ///<summary>Debug, Dev & Test config.</summary>
        [JsonProperty(PropertyName = "GameDeveloperConfig")]
        public GameDeveloperConfig GameDeveloperConfig { get; set; }

        ///<summary>Dead Lock Detector (thread detecting deadlocks).</summary>
        [JsonProperty(PropertyName = "DeadLockDetector")]
        public DeadLockDetector DeadLockDetector { get; set; }

        ///<summary>Logging features.</summary>
        [JsonProperty(PropertyName = "Logging")]
        public Logging Logging { get; set; }

        ///<summary>Community board configuration.</summary>
        [JsonProperty(PropertyName = "CommunityBoard")]
        public CommunityBoard CommunityBoard { get; set; }

        ///<summary>Flood Protectors.</summary>
        [JsonProperty(PropertyName = "FloodProtector")]
        public FloodProtector FloodProtector { get; set; }

        ///<summary>Misc.</summary>
        [JsonProperty(PropertyName = "Misc")]
        public ServerMisc Misc { get; set; }
    }

    ///<summary>Gameserver setting.</summary>
    public class GameServer
    {
        ///<summary>Bind ip of the gameserver, use * to bind on all available IPs.</summary>
        [JsonProperty(PropertyName = "GameserverHostname")]
        public string GameserverHostname { get; set; }

        [JsonProperty(PropertyName = "GameserverPort")]
        public int GameserverPort { get; set; }

        ///<summary>This is transmitted to the clients connecting from an external network, so it has to be a public IP or resolvable hostname.</summary>
        ///<summary>If this ip is resolvable by Login just leave *.</summary>
        [JsonProperty(PropertyName = "ExternalHostname")]
        public string ExternalHostname { get; set; }

        ///<summary>This is transmitted to the client from the same network, so it has to be a local IP or resolvable hostname.</summary>
        ///<summary>If this ip is resolvable by Login just leave *.</summary>
        [JsonProperty(PropertyName = "InternalHostname")]
        public string InternalHostname { get; set; }

        ///<summary>The Loginserver port.</summary>
        [JsonProperty(PropertyName = "LoginPort")]
        public int LoginPort { get; set; }

        ///<summary>The Loginserver host.</summary>
        [JsonProperty(PropertyName = "LoginHost")]
        public string LoginHost { get; set; }

        ///<summary>This is the server id that the gameserver will request.</summary>
        [JsonProperty(PropertyName = "RequestServerID")]
        public int RequestServerID { get; set; }

        ///<summary>If set to true, the login will give an other id to the server (if the requested id is already reserved).</summary>
        [JsonProperty(PropertyName = "AcceptAlternateID")]
        public bool AcceptAlternateID { get; set; }
    }

    ///<summary>Database informations.</summary>
    public class GameDatabase
    {
        [JsonProperty(PropertyName = "URL")]
        public string URL { get; set; }

        [JsonProperty(PropertyName = "Login")]
        public string Login { get; set; }
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        ///<summary>Maximum database connections (minimum 2, basically 10 if number under 10, default 100).</summary>
        [JsonProperty(PropertyName = "MaximumDbConnections")]
        public int MaximumDbConnections { get; set; }

        ///<summary>Idle connections expiration time (0 = never expire, default).</summary>
        [JsonProperty(PropertyName = "MaximumDbIdleTime")]
        public int MaximumDbIdleTime { get; set; }
    }

    ///<summary>Server List.</summary>
    public class ServerList
    {
        ///<summary>Displays [] in front of server name.</summary>
        [JsonProperty(PropertyName = "ServerListBrackets")]
        public bool ServerListBrackets { get; set; }

        ///<summary>Displays a clock next to the server name.</summary>
        [JsonProperty(PropertyName = "ServerListClock")]
        public bool ServerListClock { get; set; }

        ///<summary>If True, the server will be set as GM only.</summary>
        [JsonProperty(PropertyName = "ServerGMOnly")]
        public bool ServerGMOnly { get; set; }

        ///<summary>If True, the server will be a test server (listed by testserver clients only).</summary>
        [JsonProperty(PropertyName = "TestServer")]
        public bool TestServer { get; set; }
    }

    ///<summary>Clients related options.</summary>
    public class Client
    {
        ///<summary>Allow delete chars after D days, 0 = feature disabled.</summary>
        [JsonProperty(PropertyName = "DeleteCharAfterDays")]
        public int DeleteCharAfterDays { get; set; }

        ///<summary>Define how many players are allowed to play simultaneously on your server.</summary>
        [JsonProperty(PropertyName = "MaximumOnlineUsers")]
        public int MaximumOnlineUsers { get; set; }

        ///<summary>Minimum and maximum protocol revision that server allow to connect.</summary>
        ///<summary>You must keep MinProtocolRevision lesser or equals than MaxProtocolRevision.</summary>
        ///<summary>Default: 730.</summary>
        [JsonProperty(PropertyName = "MinProtocolRevision")]
        public int MinProtocolRevision { get; set; }

        ///<summary>Default: 746.</summary>
        [JsonProperty(PropertyName = "MaxProtocolRevision")]
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
        [JsonProperty(PropertyName = "DefaultPunish")]
        public int DefaultPunish { get; set; }
        [JsonProperty(PropertyName = "DefaultPunishParam")]
        public int DefaultPunishParam { get; set; }
    }

    ///<summary>Automatic options.</summary>
    public class Automatic
    {
        ///<summary>AutoLoot, don't lead herbs behavior. False by default.</summary>
        [JsonProperty(PropertyName = "AutoLoot")]
        public bool AutoLoot { get; set; }

        ///<summary>AutoLoot from raid boss. False by default.</summary>
        [JsonProperty(PropertyName = "AutoLootRaid")]
        public bool AutoLootRaid { get; set; }

        ///<summary>If False, herbs will drop on ground even if AutoLoot is enabled. False by default.</summary>
        [JsonProperty(PropertyName = "AutoLootHerbs")]
        public bool AutoLootHerbs { get; set; }
    }

    public class ItemTimer
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "Timer")]
        public int Timer { get; set; }
    }

    ///<summary>Items Management.</summary>
    public class ItemManagement
    {
        ///<summary>Allows players to drop items on the ground, default True.</summary>
        [JsonProperty(PropertyName = "AllowDiscardItem")]
        public bool AllowDiscardItem { get; set; }

        ///<summary>Allows the creation of multiple non-stackable items at one time, default True.</summary>
        [JsonProperty(PropertyName = "MultipleItemDrop")]
        public bool MultipleItemDrop { get; set; }

        ///<summary>Destroys dropped herbs after X seconds, set 0 to disable, default 15.</summary>
        [JsonProperty(PropertyName = "AutoDestroyHerbTime")]
        public int AutoDestroyHerbTime { get; set; }

        ///<summary>Destroys dropped items after X seconds, set 0 to disable, default 600.</summary>
        [JsonProperty(PropertyName = "AutoDestroyItemTime")]
        public int AutoDestroyItemTime { get; set; }

        ///<summary>Destroys dropped equipable items (armor, weapon, jewelry) after X seconds, set 0 to disable, default 0.</summary>
        [JsonProperty(PropertyName = "AutoDestroyEquipableItemTime")]
        public int AutoDestroyEquipableItemTime { get; set; }

        ///<summary>Destroys dropped items after specified time. Ignores rules above, default 57-0,5575-0,6673-0.</summary>
        ///<summary>57-0: Item id 57 will never be destroyed.</summary>
        ///<summary>57-600: Item id 57 will be destroyed after 600 seconds/10 minutes.</summary>
        [JsonProperty(PropertyName = "AutoDestroySpecialItemTime")]
        public ItemTimer[] AutoDestroySpecialItemTime { get; set; }

        ///<summary>Items dropped by players will have destroy time multiplied by X, default 1.</summary>
        ///<summary>0: Items dropped by players will never be destroyed.</summary>
        [JsonProperty(PropertyName = "PlayerDroppedItemMultiplier")]
        public int PlayerDroppedItemMultiplier { get; set; }

        ///<summary>Save dropped items into DB, restore them after reboot/start, default True.</summary>
        [JsonProperty(PropertyName = "SaveDroppedItem")]
        public bool SaveDroppedItem { get; set; }
    }

    ///<summary>Rates.</summary>
    public class Rates
    {
        ///<summary>Rate control, float values.</summary>
        [JsonProperty(PropertyName = "RateXp")]
        public double RateXp { get; set; }
        [JsonProperty(PropertyName = "RateSp")]
        public double RateSp { get; set; }
        [JsonProperty(PropertyName = "RatePartyXp")]
        public double RatePartyXp { get; set; }
        [JsonProperty(PropertyName = "RatePartySp")]
        public double RatePartySp { get; set; }
        [JsonProperty(PropertyName = "RateDropAdena")]
        public double RateDropAdena { get; set; }
        [JsonProperty(PropertyName = "RateConsumableCost")]
        public double RateConsumableCost { get; set; }
        [JsonProperty(PropertyName = "RateDropItems")]
        public double RateDropItems { get; set; }
        [JsonProperty(PropertyName = "RateRaidDropItems")]
        public double RateRaidDropItems { get; set; }
        [JsonProperty(PropertyName = "RateDropSpoil")]
        public double RateDropSpoil { get; set; }
        [JsonProperty(PropertyName = "RateDropManor")]
        public int RateDropManor { get; set; }

        ///<summary>Quest configuration settings.</summary>
        [JsonProperty(PropertyName = "RateQuestDrop")]
        public double RateQuestDrop { get; set; }
        [JsonProperty(PropertyName = "RateQuestReward")]
        public double RateQuestReward { get; set; }
        [JsonProperty(PropertyName = "RateQuestRewardXP")]
        public double RateQuestRewardXP { get; set; }
        [JsonProperty(PropertyName = "RateQuestRewardSP")]
        public double RateQuestRewardSP { get; set; }
        [JsonProperty(PropertyName = "RateQuestRewardAdena")]
        public double RateQuestRewardAdena { get; set; }

        [JsonProperty(PropertyName = "RateKarmaExpLost")]
        public int RateKarmaExpLost { get; set; }
        [JsonProperty(PropertyName = "RateSiegeGuardsPrice")]
        public int RateSiegeGuardsPrice { get; set; }

        ///<summary>Player Drop Rate control, percent (%) values.</summary>
        [JsonProperty(PropertyName = "PlayerDropLimit")]
        public int PlayerDropLimit { get; set; }
        [JsonProperty(PropertyName = "PlayerRateDrop")]
        public int PlayerRateDrop { get; set; }
        [JsonProperty(PropertyName = "PlayerRateDropItem")]
        public int PlayerRateDropItem { get; set; }
        [JsonProperty(PropertyName = "PlayerRateDropEquip")]
        public int PlayerRateDropEquip { get; set; }
        [JsonProperty(PropertyName = "PlayerRateDropEquipWeapon")]
        public int PlayerRateDropEquipWeapon { get; set; }

        ///<summary>Karma Drop Rate control, percent (%) values.</summary>
        [JsonProperty(PropertyName = "KarmaDropLimit")]
        public int KarmaDropLimit { get; set; }
        [JsonProperty(PropertyName = "KarmaRateDrop")]
        public int KarmaRateDrop { get; set; }
        [JsonProperty(PropertyName = "KarmaRateDropItem")]
        public int KarmaRateDropItem { get; set; }
        [JsonProperty(PropertyName = "KarmaRateDropEquip")]
        public int KarmaRateDropEquip { get; set; }
        [JsonProperty(PropertyName = "KarmaRateDropEquipWeapon")]
        public int KarmaRateDropEquipWeapon { get; set; }

        ///<summary>Pet rate control (float values except for "PetFoodRate", default 1./1/1.).</summary>
        [JsonProperty(PropertyName = "PetXpRate")]
        public double PetXpRate { get; set; }
        [JsonProperty(PropertyName = "PetFoodRate")]
        public int PetFoodRate { get; set; }
        [JsonProperty(PropertyName = "SinEaterXpRate")]
        public double SinEaterXpRate { get; set; }

        ///<summary>Common herbs (default).</summary>
        [JsonProperty(PropertyName = "RateCommonHerbs")]
        public double RateCommonHerbs { get; set; }
        ///<summary>Herb of Life (categorie 1).</summary>
        [JsonProperty(PropertyName = "RateHpHerbs")]
        public double RateHpHerbs { get; set; }
        ///<summary>Herb of Mana (categorie 2).</summary>
        [JsonProperty(PropertyName = "RateMpHerbs")]
        public double RateMpHerbs { get; set; }
        ///<summary>Special herbs (categorie 3).</summary>
        [JsonProperty(PropertyName = "RateSpecialHerbs")]
        public double RateSpecialHerbs { get; set; }
    }

    ///<summary>Allowed features.</summary>
    public class AllowedFeatures
    {
        [JsonProperty(PropertyName = "AllowFreight")]
        public bool AllowFreight { get; set; }
        [JsonProperty(PropertyName = "AllowWarehouse")]
        public bool AllowWarehouse { get; set; }

        ///<summary>If True, player can try on weapon and armor in shops.</summary>
        ///<summary>Each item tried cost WearPrice adena.</summary>
        [JsonProperty(PropertyName = "AllowWear")]
        public bool AllowWear { get; set; }
        [JsonProperty(PropertyName = "WearDelay")]
        public int WearDelay { get; set; }
        [JsonProperty(PropertyName = "WearPrice")]
        public int WearPrice { get; set; }

        ///<summary>"Allow" types - Read variable name for info about ;p.</summary>
        [JsonProperty(PropertyName = "AllowLottery")]
        public bool AllowLottery { get; set; }
        [JsonProperty(PropertyName = "AllowWater")]
        public bool AllowWater { get; set; }
        [JsonProperty(PropertyName = "AllowCursedWeapons")]
        public bool AllowCursedWeapons { get; set; }
        [JsonProperty(PropertyName = "AllowManor")]
        public bool AllowManor { get; set; }
        [JsonProperty(PropertyName = "AllowBoat")]
        public bool AllowBoat { get; set; }

        ///<summary>NOTE : Fishing will "bug" without geodata (if you activate w/o geodata, fishing is possible everywhere).</summary>
        [JsonProperty(PropertyName = "AllowFishing")]
        public bool AllowFishing { get; set; }

        ///<summary>Allow characters to receive damage from falling. CoordSynchronize = 2 is recommended.</summary>
        ///<summary>True - enabled.</summary>
        ///<summary>False - disabled.</summary>
        ///<summary>Auto - True if geodata enabled and False if disabled.</summary>
        ///<summary>Default: Auto.</summary>
        [JsonProperty(PropertyName = "EnableFallingDamage")]
        public string EnableFallingDamage { get; set; }
    }

    ///<summary>Debug, Dev & Test config.</summary>
    public class GameDeveloperConfig
    {
        ///<summary>Don't load spawns.</summary>
        [JsonProperty(PropertyName = "NoSpawns")]
        public bool NoSpawns { get; set; }

        ///<summary>Debug messages (by default False, easily "flood" your GS logs).</summary>
        [JsonProperty(PropertyName = "Debug")]
        public bool Debug { get; set; }
        [JsonProperty(PropertyName = "Developer")]
        public bool Developer { get; set; }
        [JsonProperty(PropertyName = "PacketHandlerDebug")]
        public bool PacketHandlerDebug { get; set; }
    }

    ///<summary>Dead Lock Detector (thread detecting deadlocks).</summary>
    public class DeadLockDetector
    {
        ///<summary>Activate the feature (by default: False).</summary>
        [JsonProperty(PropertyName = "DeadLockDetector")]
        public bool EnableDeadLockDetector { get; set; }

        ///<summary>Check interval in seconds (by default: 20).</summary>
        [JsonProperty(PropertyName = "DeadLockCheckInterval")]
        public int DeadLockCheckInterval { get; set; }

        ///<summary>Automatic restart if deadlock case is found (by default: False).</summary>
        [JsonProperty(PropertyName = "RestartOnDeadlock")]
        public bool RestartOnDeadlock { get; set; }
    }

    ///<summary>Logging features.</summary>
    public class Logging
    {
        ///<summary>Logging ChatWindow.</summary>
        [JsonProperty(PropertyName = "LogChat")]
        public bool LogChat { get; set; }

        ///<summary>Logging Item handling NOTE: This can be very space consuming.</summary>
        [JsonProperty(PropertyName = "LogItems")]
        public bool LogItems { get; set; }

        ///<summary>Log GM actions.</summary>
        [JsonProperty(PropertyName = "GMAudit")]
        public bool GMAudit { get; set; }
    }

    ///<summary>Community board configuration.</summary>
    public class CommunityBoard
    {
        ///<summary>Activate or no the community board.</summary>
        [JsonProperty(PropertyName = "EnableCommunityBoard")]
        public bool EnableCommunityBoard { get; set; }

        ///<summary>Show this community board section when you open it.</summary>
        [JsonProperty(PropertyName = "BBSDefault")]
        public string BBSDefault { get; set; }
    }

    ///<summary>Flood Protectors.</summary>
    public class FloodProtector
    {
        ///<summary>The values are shown on ms. They can be setted to 0 to be disabled.</summary>
        [JsonProperty(PropertyName = "RollDiceTime")]
        public int RollDiceTime { get; set; }

        [JsonProperty(PropertyName = "HeroVoiceTime")]
        public int HeroVoiceTime { get; set; }

        [JsonProperty(PropertyName = "SubclassTime")]
        public int SubclassTime { get; set; }

        [JsonProperty(PropertyName = "DropItemTime")]
        public int DropItemTime { get; set; }

        [JsonProperty(PropertyName = "ServerBypassTime")]
        public int ServerBypassTime { get; set; }

        [JsonProperty(PropertyName = "MultisellTime")]
        public int MultisellTime { get; set; }

        [JsonProperty(PropertyName = "ManufactureTime")]
        public int ManufactureTime { get; set; }

        [JsonProperty(PropertyName = "ManorTime")]
        public int ManorTime { get; set; }

        [JsonProperty(PropertyName = "SendMailTime")]
        public int SendMailTime { get; set; }

        [JsonProperty(PropertyName = "CharacterSelectTime")]
        public int CharacterSelectTime { get; set; }

        [JsonProperty(PropertyName = "GlobalChatTime")]
        public int GlobalChatTime { get; set; }

        [JsonProperty(PropertyName = "TradeChatTime")]
        public int TradeChatTime { get; set; }

        [JsonProperty(PropertyName = "SocialTime")]
        public int SocialTime { get; set; }
    }

    ///<summary>Misc.</summary>
    public class ServerMisc
    {
        ///<summary>Basic protection against L2Walker.</summary>
        [JsonProperty(PropertyName = "L2WalkerProtection")]
        public bool L2WalkerProtection { get; set; }

        ///<summary>Delete invalid quest from player.</summary>
        [JsonProperty(PropertyName = "AutoDeleteInvalidQuestData")]
        public bool AutoDeleteInvalidQuestData { get; set; }

        ///<summary>Zone setting.</summary>
        ///<summary>0 = Peace All the Time.</summary>
        ///<summary>1 = PVP During Siege for siege participants.</summary>
        ///<summary>2 = PVP All the Time.</summary>
        [JsonProperty(PropertyName = "ZoneTown")]
        public int ZoneTown { get; set; }

        ///<summary>Show "data/html/servnews.htm" when a character logins.</summary>
        [JsonProperty(PropertyName = "ShowServerNews")]
        public bool ShowServerNews { get; set; }

        ///<summary>Disable tutorial on new player game entrance. Default: False.</summary>
        [JsonProperty(PropertyName = "DisableTutorial")]
        public bool DisableTutorial { get; set; }
    }
}