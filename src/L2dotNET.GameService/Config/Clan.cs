using System.ComponentModel;
using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    ///<summary>Clans Config.</summary>
    ///TODO: Create mapping for cost/level/regeneration
    public class Clan
    {
        ///<summary>Number of days you have to wait before joining another clan.</summary>
        [DefaultValue(5)]
        [JsonProperty(PropertyName = "DaysBeforeJoinAClan", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DaysBeforeJoinAClan { get; set; }

        ///<summary>Number of days you have to wait before creating a new clan.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "DaysBeforeCreateAClan", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DaysBeforeCreateAClan { get; set; }

        ///<summary>Number of days it takes to dissolve a clan.</summary>
        [DefaultValue(7)]
        [JsonProperty(PropertyName = "DaysToPassToDissolveAClan", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DaysToPassToDissolveAClan { get; set; }

        ///<summary>Number of days before joining a new alliance when clan voluntarily leave an alliance.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "DaysBeforeJoinAllyWhenLeaved", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DaysBeforeJoinAllyWhenLeaved { get; set; }

        ///<summary>Number of days before joining a new alliance when clan was dismissed from an alliance.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "DaysBeforeJoinAllyWhenDismissed", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DaysBeforeJoinAllyWhenDismissed { get; set; }

        ///<summary>Number of days before accepting a new clan for alliance when clan was dismissed from an alliance.</summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "DaysBeforeAcceptNewClanWhenDismissed", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DaysBeforeAcceptNewClanWhenDismissed { get; set; }

        ///<summary>Number of days before creating a new alliance when dissolved an alliance.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "DaysBeforeCreateNewAllyWhenDissolved", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int DaysBeforeCreateNewAllyWhenDissolved { get; set; }

        ///<summary>Maximum number of clans in ally.</summary>
        [DefaultValue(3)]
        [JsonProperty(PropertyName = "MaxNumOfClansInAlly", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaxNumOfClansInAlly { get; set; }

        ///<summary>Number of members needed to request a clan war.</summary>
        [DefaultValue(15)]
        [JsonProperty(PropertyName = "ClanMembersForWar", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ClanMembersForWar { get; set; }

        ///<summary>Number of days needed by a clan to war back another clan.</summary>
        [DefaultValue(5)]
        [JsonProperty(PropertyName = "ClanWarPenaltyWhenEnded", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ClanWarPenaltyWhenEnded { get; set; }

        ///<summary>Privilege browse warehouse enables at the same time also withdraw from warehouse!.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "MembersCanWithdrawFromClanWH", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool MembersCanWithdrawFromClanWh { get; set; }

        ///<summary>Remove Castle circlets after a clan lose its castle or a player leaves a clan? - default true.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "RemoveCastleCirclets", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool RemoveCastleCirclets { get; set; }

        ///<summary>Manor Config.</summary>
        public Manor Manor { get; set; }

        ///<summary>Clan Hall functions price.</summary>
        ///<summary>Price = 7 day(one day price = price/7).</summary>
        public ClanHallFunctionsPrice ClanHallFunctionsPrice { get; set; }
    }

    ///<summary>Manor Config.</summary>
    public class Manor
    {
        ///<summary>Manor Refresh Time in Military hours Default 8pm (20).</summary>
        [DefaultValue(20)]
        [JsonProperty(PropertyName = "RefreshTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RefreshTime { get; set; }

        ///<summary>Manor Refresh Time for Min's, Default 00 so at the start of the hour.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "RefreshMin", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RefreshMin { get; set; }

        ///<summary>Manor Next Period Approve Time in Military hours Default 6am.</summary>
        [DefaultValue(6)]
        [JsonProperty(PropertyName = "ApproveTime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ApproveTime { get; set; }

        ///<summary>Manor Next Period Approve Time for Min's, Default 00 so at the start of the hour.</summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "ApproveMin", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ApproveMin { get; set; }

        ///<summary>Manor Maintenance time, Default 6 minutes.</summary>
        [DefaultValue(360000)]
        [JsonProperty(PropertyName = "MaintenancePeriod", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int MaintenancePeriod { get; set; }

        ///<summary>Manor Save Type. 1-Save data into db after every action; Default false.</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "SaveAllActions", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool SaveAllActions { get; set; }

        ///<summary>Manor Save Period (used only if SaveAllActions=false) Default very 2 hours.</summary>
        [DefaultValue(2)]
        [JsonProperty(PropertyName = "SavePeriodRate", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int SavePeriodRate { get; set; }
    }

    ///<summary>Clan Hall functions price.</summary>
    ///<summary>Price = 7 day(one day price = price/7).</summary>
    public class ClanHallFunctionsPrice
    {
        ///<summary>Teleport function price.</summary>
        [JsonProperty(PropertyName = "Teleport", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Teleport Teleport { get; set; }

        ///<summary>Support magic buff function price.</summary>
        [JsonProperty(PropertyName = "Support", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Support Support { get; set; }

        ///<summary>MpRegeneration function price.</summary>
        [JsonProperty(PropertyName = "MpRegeneration", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public MpRegeneration MpRegeneration { get; set; }

        ///<summary>HpRegeneration function price.</summary>
        [JsonProperty(PropertyName = "HpRegeneration", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public HpRegeneration HpRegeneration { get; set; }

        ///<summary>ExpRegeneration function price.</summary>
        [JsonProperty(PropertyName = "ExpRegeneration", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ExpRegeneration ExpRegeneration { get; set; }

        ///<summary>ItemCreation function price.</summary>
        [JsonProperty(PropertyName = "ItemCreation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ItemCreation ItemCreation { get; set; }

        ///<summary>Decor curtain function price.</summary>
        [JsonProperty(PropertyName = "Curtain", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Curtain Curtain { get; set; }

        ///<summary>Decor front platform function price.</summary>
        [JsonProperty(PropertyName = "FrontPlatform", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public FrontPlatform FrontPlatform { get; set; }
    }

    public interface IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        int FeeRatio { get; set; }
    }

    ///<summary>Teleport function price.</summary>
    public class Teleport : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        ///<summary>1st level.</summary>
        [DefaultValue(7000)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [DefaultValue(14000)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }
    }

    ///<summary>Support magic buff function price.</summary>
    public class Support : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        ///<summary>1st level.</summary>
        [DefaultValue(17500)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [DefaultValue(35000)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }

        ///<summary>3rd level.</summary>
        [DefaultValue(49000)]
        [JsonProperty(PropertyName = "Lvl3", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl3 { get; set; }

        ///<summary>4th level.</summary>
        [DefaultValue(77000)]
        [JsonProperty(PropertyName = "Lvl4", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl4 { get; set; }

        ///<summary>5th level.</summary>
        [DefaultValue(147000)]
        [JsonProperty(PropertyName = "Lvl5", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl5 { get; set; }

        ///<summary>6th level.</summary>
        [DefaultValue(252000)]
        [JsonProperty(PropertyName = "Lvl6", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl6 { get; set; }

        ///<summary>7th level.</summary>
        [DefaultValue(259000)]
        [JsonProperty(PropertyName = "Lvl7", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl7 { get; set; }

        ///<summary>8th level.</summary>
        [DefaultValue(364000)]
        [JsonProperty(PropertyName = "Lvl8", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl8 { get; set; }
    }

    ///<summary>MpRegeneration function price.</summary>
    public class MpRegeneration : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        ///<summary>5% MpRegeneration.</summary>
        [DefaultValue(14000)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>10% MpRegeneration.</summary>
        [DefaultValue(26250)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }

        ///<summary>15% MpRegeneration.</summary>
        [DefaultValue(45500)]
        [JsonProperty(PropertyName = "Lvl3", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl3 { get; set; }

        ///<summary>30% MpRegeneration.</summary>
        [DefaultValue(96250)]
        [JsonProperty(PropertyName = "Lvl4", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl4 { get; set; }

        ///<summary>40% MpRegeneration.</summary>
        [DefaultValue(140000)]
        [JsonProperty(PropertyName = "Lvl5", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl5 { get; set; }
    }

    ///<summary>HpRegeneration function price.</summary>
    public class HpRegeneration : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        ///<summary>20% HpRegeneration.</summary>
        [DefaultValue(4900)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>40% HpRegeneration.</summary>
        [DefaultValue(5600)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }

        ///<summary>80% HpRegeneration.</summary>
        [DefaultValue(7000)]
        [JsonProperty(PropertyName = "Lvl3", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl3 { get; set; }

        ///<summary>100% HpRegeneration.</summary>
        [DefaultValue(8166)]
        [JsonProperty(PropertyName = "Lvl4", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl4 { get; set; }

        ///<summary>120% HpRegeneration.</summary>
        [DefaultValue(10500)]
        [JsonProperty(PropertyName = "Lvl5", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl5 { get; set; }

        ///<summary>140% HpRegeneration.</summary>
        [DefaultValue(12250)]
        [JsonProperty(PropertyName = "Lvl6", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl6 { get; set; }

        ///<summary>160% HpRegeneration.</summary>
        [DefaultValue(14000)]
        [JsonProperty(PropertyName = "Lvl7", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl7 { get; set; }

        ///<summary>180% HpRegeneration.</summary>
        [DefaultValue(15750)]
        [JsonProperty(PropertyName = "Lvl8", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl8 { get; set; }

        ///<summary>200% HpRegeneration.</summary>
        [DefaultValue(17500)]
        [JsonProperty(PropertyName = "Lvl9", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl9 { get; set; }

        ///<summary>220% HpRegeneration.</summary>
        [DefaultValue(22750)]
        [JsonProperty(PropertyName = "Lvl10", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl10 { get; set; }

        ///<summary>240% HpRegeneration.</summary>
        [DefaultValue(26250)]
        [JsonProperty(PropertyName = "Lvl11", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl11 { get; set; }

        ///<summary>260% HpRegeneration.</summary>
        [DefaultValue(29750)]
        [JsonProperty(PropertyName = "Lvl12", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl12 { get; set; }

        ///<summary>300% HpRegeneration.</summary>
        [DefaultValue(36166)]
        [JsonProperty(PropertyName = "Lvl13", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl13 { get; set; }
    }

    ///<summary>ExpRegeneration function price.</summary>
    public class ExpRegeneration : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        //<summary>5% ExpRegeneration.</summary>
        [DefaultValue(21000)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>10% ExpRegeneration.</summary>
        [DefaultValue(42000)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }

        ///<summary>15% ExpRegeneration.</summary>
        [DefaultValue(63000)]
        [JsonProperty(PropertyName = "Lvl3", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl3 { get; set; }

        ///<summary>25% ExpRegeneration.</summary>
        [DefaultValue(105000)]
        [JsonProperty(PropertyName = "Lvl4", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl4 { get; set; }

        ///<summary>35% ExpRegeneration.</summary>
        [DefaultValue(147000)]
        [JsonProperty(PropertyName = "Lvl5", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl5 { get; set; }

        ///<summary>40% ExpRegeneration.</summary>
        [DefaultValue(163331)]
        [JsonProperty(PropertyName = "Lvl6", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl6 { get; set; }

        ///<summary>50% ExpRegeneration.</summary>
        [DefaultValue(210000)]
        [JsonProperty(PropertyName = "Lvl7", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl7 { get; set; }
    }

    ///<summary>Creation item function price.</summary>
    public class ItemCreation : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        ///<summary>1st level.</summary>
        [DefaultValue(210000)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [DefaultValue(490000)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }

        ///<summary>3rd level.</summary>
        [DefaultValue(980000)]
        [JsonProperty(PropertyName = "Lvl3", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl3 { get; set; }
    }

    ///<summary>Need core support, need more information on functions in different. Clan Hall in different Towns.</summary>
    ///<summary>Decor curtain function price.</summary>
    public class Curtain : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        ///<summary>1st level.</summary>
        [DefaultValue(2002)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [DefaultValue(2625)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }
    }

    ///<summary>Need core support, need more information on functions in different. Clan Hall in different Towns.</summary>
    ///<summary>Decor front platform function price.</summary>
    public class FrontPlatform : IClanHallFunction
    {
        ///<summary>Fee ratio of the function.</summary>
        [DefaultValue(86400000)]
        [JsonProperty(PropertyName = "FeeRatio", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int FeeRatio { get; set; }

        ///<summary>1st level.</summary>
        [DefaultValue(3031)]
        [JsonProperty(PropertyName = "Lvl1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [DefaultValue(9331)]
        [JsonProperty(PropertyName = "Lvl2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Lvl2 { get; set; }
    }
}