using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class Event
    {
        ///<summary>Olympiad Config.</summary>
        [JsonProperty(PropertyName = "Olympiad")]
        public Olympiad Olympiad { get; set; }

        ///<summary>Seven Signs Config.</summary>
        [JsonProperty(PropertyName = "SevenSigns")]
        public SevenSigns SevenSigns { get; set; }

        ///<summary>Four Sepulchers Config.</summary>
        [JsonProperty(PropertyName = "FourSepulchers")]
        public FourSepulchers FourSepulchers { get; set; }

        ///<summary>Dimension Rift Config.</summary>
        [JsonProperty(PropertyName = "DimensionRift")]
        public DimensionRift DimensionRift { get; set; }

        ///<summary>Wedding Config.</summary>
        [JsonProperty(PropertyName = "Wedding")]
        public Wedding Wedding { get; set; }

        ///<summary>Lottery Config.</summary>
        [JsonProperty(PropertyName = "Lottery")]
        public Lottery Lottery { get; set; }

        ///<summary>Fishing Config.</summary>
        [JsonProperty(PropertyName = "Fishing")]
        public Fishing Fishing { get; set; }
    }

    ///<summary>Olympiad Config.</summary>
    public class Olympiad
    {
        ///<summary>Olympiad start time hour, default 18 (6PM).</summary>
        [JsonProperty(PropertyName = "StartTime")]
        public int StartTime { get; set; }

        ///<summary>Olympiad start time minutes, default 00.</summary>
        [JsonProperty(PropertyName = "Min")]
        public int Min { get; set; }

        ///<summary>Olympiad competition period, default 6 hours (should be changed by steps of 10mins).</summary>
        [JsonProperty(PropertyName = "CPeriod")]
        public int CPeriod { get; set; }

        ///<summary>Olympiad battle period, default 6 minutes.</summary>
        [JsonProperty(PropertyName = "Battle")]
        public int Battle { get; set; }

        ///<summary>Olympiad weekly period, default 1 week.</summary>
        [JsonProperty(PropertyName = "WPeriod")]
        public int WPeriod { get; set; }

        ///<summary>Olympiad validation period, default 24 hours.</summary>
        [JsonProperty(PropertyName = "VPeriod")]
        public int VPeriod { get; set; }

        ///<summary>Time to wait before teleported to arena, default: 30 seconds.</summary>
        [JsonProperty(PropertyName = "WaitTime")]
        public int WaitTime { get; set; }

        ///<summary>Time to wait before battle starts (at 20s characters receive buffs), default: 60 seconds.</summary>
        [JsonProperty(PropertyName = "WaitBattle")]
        public int WaitBattle { get; set; }

        ///<summary>Time to wait before teleported back to town, default: 40 seconds.</summary>
        [JsonProperty(PropertyName = "WaitEnd")]
        public int WaitEnd { get; set; }

        ///<summary>Rewarded points for the first mpiad cycle, default: 18.</summary>
        [JsonProperty(PropertyName = "StartPoints")]
        public int StartPoints { get; set; }

        ///<summary>Points allowed every week after first cycle, default: 3.</summary>
        [JsonProperty(PropertyName = "WeeklyPoints")]
        public int WeeklyPoints { get; set; }

        ///<summary>Required number of matches in order to be classed as hero, default: 9.</summary>
        [JsonProperty(PropertyName = "MinMatchesToBeClassed")]
        public int MinMatchesToBeClassed { get; set; }

        ///<summary>Required number of participants for the class based games, default: 5.</summary>
        [JsonProperty(PropertyName = "ClassedParticipants")]
        public int ClassedParticipants { get; set; }

        ///<summary>Required number of participants for the non-class based games, default: 9.</summary>
        [JsonProperty(PropertyName = "NonClassedParticipants")]
        public int NonClassedParticipants { get; set; }

        ///<summary>Reward for the class based games.</summary>
        ///<summary>Format: itemId1,itemNum1;itemId2,itemNum2.</summary>
        ///<summary>Default: 6651,50.</summary>
        [JsonProperty(PropertyName = "ClassedReward")]
        public int[] ClassedReward { get; set; }

        ///<summary>Reward for the non-class based games.</summary>
        ///<summary>Format: itemId1,itemNum1;itemId2,itemNum2.</summary>
        ///<summary>Default: 6651,30.</summary>
        [JsonProperty(PropertyName = "NonClassedReward")]
        public int[] NonClassedReward { get; set; }

        ///<summary>Rate to exchange points to reward item, default: 1000.</summary>
        [JsonProperty(PropertyName = "GPPerPoint")]
        public int GPPerPoint { get; set; }

        ///<summary>CRPs for heroes in clan, default: 300.</summary>
        [JsonProperty(PropertyName = "HeroPoints")]
        public int HeroPoints { get; set; }

        ///<summary>Noblesse points awarded to Rank 1 members, default: 100.</summary>
        [JsonProperty(PropertyName = "Rank1Points")]
        public int Rank1Points { get; set; }

        ///<summary>Noblesse points awarded to Rank 2 members, default: 75.</summary>
        [JsonProperty(PropertyName = "Rank2Points")]
        public int Rank2Points { get; set; }

        ///<summary>Noblesse points awarded to Rank 3 members, default: 55.</summary>
        [JsonProperty(PropertyName = "Rank3Points")]
        public int Rank3Points { get; set; }

        ///<summary>Noblesse points awarded to Rank 4 members, default: 40.</summary>
        [JsonProperty(PropertyName = "Rank4Points")]
        public int Rank4Points { get; set; }

        ///<summary>Noblesse points awarded to Rank 5 members, default: 30.</summary>
        [JsonProperty(PropertyName = "Rank5Points")]
        public int Rank5Points { get; set; }

        ///<summary>Maximum points that player can gain/lose on a match, default: 10.</summary>
        [JsonProperty(PropertyName = "MaxPoints")]
        public int MaxPoints { get; set; }

        ///<summary>Olympiad Managers announce each start of fight, default: True.</summary>
        [JsonProperty(PropertyName = "AnnounceGames")]
        public bool AnnounceGames { get; set; }

        ///<summary>Divider for points in classed and non-classed games, default: 3, 5.</summary>
        [JsonProperty(PropertyName = "DividerClassed")]
        public int DividerClassed { get; set; }

        ///<summary>Divider for points in classed and non-classed games, default: 3, 5.</summary>
        [JsonProperty(PropertyName = "DividerNonClassed")]
        public int DividerNonClassed { get; set; }
    }

    ///<summary>Seven Signs Config.</summary>
    public class SevenSigns
    {
        ///<summary>Dawn:</summary>
        ///<summary>True - Players not owning castle need pay participation fee.</summary>
        ///<summary>False - Anyone can join Dawn.</summary>
        [JsonProperty(PropertyName = "CastleForDawn")]
        public bool CastleForDawn { get; set; }

        ///<summary>Dusk:</summary>
        ///<summary>True - Players owning castle can not join Dusk side.</summary>
        ///<summary>False - Anyone can join Dusk.</summary>
        [JsonProperty(PropertyName = "CastleForDusk")]
        public bool CastleForDusk { get; set; }

        ///<summary>Save SevenSigns status only each 30 mins and after period change.</summary>
        ///<summary>Player info saved only during periodic data store (set by CharacterDataStoreInterval) and logout.</summary>
        ///<summary>If False then save info and status immediately after every changes (heavy but accurate).</summary>
        ///<summary>Default: True.</summary>
        [JsonProperty(PropertyName = "LazyUpdate")]
        public bool LazyUpdate { get; set; }

        ///<summary>Festival.</summary>
        public Festival Festival { get; set; }
    }

    ///<summary>Festival Config.</summary>
    public class Festival
    {
        ///<summary>Minimum Players for participate in SevenSigns Festival.</summary>
        ///<summary>Default : 5.</summary>
        [JsonProperty(PropertyName = "MinPlayer")]
        public int MinPlayer { get; set; }

        ///<summary>Maximum contribution per player during festival.</summary>
        ///<summary>/!\ This value is NOT impacted by server drop rate.</summary>
        [JsonProperty(PropertyName = "MaxPlayerContrib")]
        public int MaxPlayerContrib { get; set; }

        ///<summary>Festival Manager Start time.</summary>
        ///<summary>Default : 2 minutes.</summary>
        [JsonProperty(PropertyName = "ManagerStart")]
        public int ManagerStart { get; set; }

        ///<summary>Festival Length.</summary>
        ///<summary>Default : 18 minutes.</summary>
        [JsonProperty(PropertyName = "Length")]
        public int Length { get; set; }

        ///<summary>Festival Cycle Length.</summary>
        ///<summary>Default : 38 Minutes (20 minutes wait time, + Festival time).</summary>
        [JsonProperty(PropertyName = "CycleLength")]
        public int CycleLength { get; set; }

        ///<summary>At what point the first festival spawn occures.</summary>
        ///<summary>Default : 2 minutes.</summary>
        [JsonProperty(PropertyName = "FirstSpawn")]
        public int FirstSpawn { get; set; }

        ///<summary>At what Point the first festival swarm occures.</summary>
        ///<summary>Default : 5 minutes.</summary>
        [JsonProperty(PropertyName = "FirstSwarm")]
        public int FirstSwarm { get; set; }

        ///<summary>At what Point the Second Festival spawn occures.</summary>
        ///<summary>Default : 9 minutes.</summary>
        [JsonProperty(PropertyName = "SecondSpawn")]
        public int SecondSpawn { get; set; }

        ///<summary>At what Point the Second Festival Swarm occures.</summary>
        ///<summary>Default : 12 minutes.</summary>
        [JsonProperty(PropertyName = "SecondSwarm")]
        public int SecondSwarm { get; set; }

        ///<summary>At what point the Chests Spawn in.</summary>
        ///<summary>Default : 15 minutes.</summary>
        [JsonProperty(PropertyName = "ChestSpawn")]
        public int ChestSpawn { get; set; }
    }

    ///<summary>Four Sepulchers Config.</summary>
    public class FourSepulchers
    {
        ///<summary>Default: 50.</summary>
        [JsonProperty(PropertyName = "TimeOfAttack")]
        public int TimeOfAttack { get; set; }

        ///<summary>Default: 3.</summary>
        [JsonProperty(PropertyName = "TimeOfEntry")]
        public int TimeOfEntry { get; set; }

        ///<summary>Default: 2.</summary>
        [JsonProperty(PropertyName = "TimeOfWarmUp")]
        public int TimeOfWarmUp { get; set; }

        ///<summary>Default: 4.</summary>
        [JsonProperty(PropertyName = "NumberOfNecessaryPartyMembers")]
        public int NumberOfNecessaryPartyMembers { get; set; }
    }

    ///<summary>Dimension Rift Config.</summary>
    public class DimensionRift
    {
        ///<summary>Minimal party size to enter rift. Min = 2, Max = 9 (retail = 2).</summary>
        ///<summary>If in rift party will become smaller all members will be teleported back.</summary>
        [JsonProperty(PropertyName = "RiftMinPartySize")]
        public int RiftMinPartySize { get; set; }

        ///<summary>Number of maximum jumps between rooms allowed, after this time party will be teleported back.</summary>
        [JsonProperty(PropertyName = "MaxRiftJumps")]
        public int MaxRiftJumps { get; set; }

        ///<summary>Time in ms the party has to wait until the mobs spawn when entering a room. C4 retail: 10s.</summary>
        [JsonProperty(PropertyName = "RiftSpawnDelay")]
        public int RiftSpawnDelay { get; set; }

        ///<summary>Time between automatic jumps in seconds (retail : 8min(480sec) - 10min(600sec)).</summary>
        [JsonProperty(PropertyName = "AutoJumpsDelayMin")]
        public int AutoJumpsDelayMin { get; set; }

        ///<summary>Time between automatic jumps in seconds (retail : 8min(480sec) - 10min(600sec)).</summary>
        [JsonProperty(PropertyName = "AutoJumpsDelayMax")]
        public int AutoJumpsDelayMax { get; set; }

        ///<summary>Time Multiplier for stay in the boss room.</summary>aaaaaaaaaa
        [JsonProperty(PropertyName = "BossRoomTimeMultiply")]
        public double BossRoomTimeMultiply { get; set; }

        ///<summary>Cost in dimension fragments to enter the rift, each party member must own this amount.</summary>
        [JsonProperty(PropertyName = "RecruitCost")]
        public int RecruitCost { get; set; }

        ///<summary>Cost in dimension fragments to enter the rift, each party member must own this amount.</summary>
        [JsonProperty(PropertyName = "SoldierCost")]
        public int SoldierCost { get; set; }

        ///<summary>Cost in dimension fragments to enter the rift, each party member must own this amount.</summary>
        [JsonProperty(PropertyName = "OfficerCost")]
        public int OfficerCost { get; set; }

        ///<summary>Cost in dimension fragments to enter the rift, each party member must own this amount.</summary>
        [JsonProperty(PropertyName = "CaptainCost")]
        public int CaptainCost { get; set; }

        ///<summary>Cost in dimension fragments to enter the rift, each party member must own this amount.</summary>
        [JsonProperty(PropertyName = "CommanderCost")]
        public int CommanderCost { get; set; }

        ///<summary>Cost in dimension fragments to enter the rift, each party member must own this amount.</summary>
        [JsonProperty(PropertyName = "HeroCost")]
        public int HeroCost { get; set; }
    }

    ///<summary>Wedding Config.</summary>
    ///<summary>Wedding Manager: //spawn 50007</summary>
    public class Wedding
    {
        ///<summary>True allows Wedding, False disables it.</summary></summary>
        ///<summary>The Wedding Manager is disabled until this setting is put to True.</summary>
        [JsonProperty(PropertyName = "AllowWedding")]
        public bool AllowWedding { get; set; }

        ///<summary>Cost of wedding, price in adenas.</summary>
        [JsonProperty(PropertyName = "WeddingPrice")]
        public int WeddingPrice { get; set; }

        ///<summary>Homosexual marriages allowed, False by default.</summary>
        [JsonProperty(PropertyName = "WeddingAllowSameSex")]
        public bool WeddingAllowSameSex { get; set; }

        ///<summary>Do players require to wear formal wear ? True by default.</summary>
        [JsonProperty(PropertyName = "WeddingFormalWear")]
        public bool WeddingFormalWear { get; set; }
    }

    ///<summary>Lottery Config.</summary>
    public class Lottery
    {
        ///<summary>Initial Lottery prize.</summary>
        [JsonProperty(PropertyName = "Prize")]
        public int Prize { get; set; }

        ///<summary>Lottery Ticket Price.</summary>
        [JsonProperty(PropertyName = "TicketPrice")]
        public int TicketPrice { get; set; }

        ///<summary>What part of jackpot amount should receive characters who pick 5 winning numbers.</summary>
        [JsonProperty(PropertyName = "Winning5NumberRate")]
        public double Winning5NumberRate { get; set; }

        ///<summary>What part of jackpot amount should receive characters who pick 4 winning numbers.</summary>
        [JsonProperty(PropertyName = "Winning4NumberRate")]
        public double Winning4NumberRate { get; set; }

        ///<summary>What part of jackpot amount should receive characters who pick 3 winning numbers.</summary>
        [JsonProperty(PropertyName = "Winning3NumberRate")]
        public double Winning3NumberRate { get; set; }

        ///<summary>How much adena receive characters who pick two or less of the winning number.</summary>
        [JsonProperty(PropertyName = "Winning2And1NumberPrize")]
        public double Winning2And1NumberPrize { get; set; }
    }

    ///<summary>Fishing.</summary>
    public class Fishing
    {
        ///<summary>Enable or disable the Fishing Tournament system.</summary>
        [JsonProperty(PropertyName = "FishChampionshipEnabled")]
        public bool FishChampionshipEnabled { get; set; }

        ///<summary>Item Id used as reward.</summary>
        [JsonProperty(PropertyName = "FishChampionshipRewardItemId")]
        public int FishChampionshipRewardItemId { get; set; }

        ///<summary>Item count used as reward (for the 5 first winners) - 1.</summary>
        [JsonProperty(PropertyName = "FishChampionshipReward1")]
        public int FishChampionshipReward1 { get; set; }

        ///<summary>Item count used as reward (for the 5 first winners) - 2.</summary>
        [JsonProperty(PropertyName = "FishChampionshipReward2")]
        public int FishChampionshipReward2 { get; set; }

        ///<summary>Item count used as reward (for the 5 first winners) - 3.</summary>
        [JsonProperty(PropertyName = "FishChampionshipReward3")]
        public int FishChampionshipReward3 { get; set; }

        ///<summary>Item count used as reward (for the 5 first winners) - 4.</summary>
        [JsonProperty(PropertyName = "FishChampionshipReward4")]
        public int FishChampionshipReward4 { get; set; }

        ///<summary>Item count used as reward (for the 5 first winners) - 5.</summary>
        [JsonProperty(PropertyName = "FishChampionshipReward5")]
        public int FishChampionshipReward5 { get; set; }
    }
}