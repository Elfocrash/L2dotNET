using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    ///<summary>Clans Config.</summary>
    ///TODO: Create mapping for cost/level/regeneration
    public class Clan
    {
        ///<summary>Number of days you have to wait before joining another clan.</summary>
        [JsonProperty(PropertyName = "DaysBeforeJoinAClan")]
        public int DaysBeforeJoinAClan { get; set; }

        ///<summary>Number of days you have to wait before creating a new clan.</summary>
        [JsonProperty(PropertyName = "DaysBeforeCreateAClan")]
        public int DaysBeforeCreateAClan { get; set; }

        ///<summary>Number of days it takes to dissolve a clan.</summary>
        [JsonProperty(PropertyName = "DaysToPassToDissolveAClan")]
        public int DaysToPassToDissolveAClan { get; set; }

        ///<summary>Number of days before joining a new alliance when clan voluntarily leave an alliance.</summary>
        [JsonProperty(PropertyName = "DaysBeforeJoinAllyWhenLeaved")]
        public int DaysBeforeJoinAllyWhenLeaved { get; set; }

        ///<summary>Number of days before joining a new alliance when clan was dismissed from an alliance.</summary>
        [JsonProperty(PropertyName = "DaysBeforeJoinAllyWhenDismissed")]
        public int DaysBeforeJoinAllyWhenDismissed { get; set; }

        ///<summary>Number of days before accepting a new clan for alliance when clan was dismissed from an alliance.</summary>
        [JsonProperty(PropertyName = "DaysBeforeAcceptNewClanWhenDismissed")]
        public int DaysBeforeAcceptNewClanWhenDismissed { get; set; }

        ///<summary>Number of days before creating a new alliance when dissolved an alliance.</summary>
        [JsonProperty(PropertyName = "DaysBeforeCreateNewAllyWhenDissolved")]
        public int DaysBeforeCreateNewAllyWhenDissolved { get; set; }

        ///<summary>Maximum number of clans in ally.</summary>
        [JsonProperty(PropertyName = "MaxNumOfClansInAlly")]
        public int MaxNumOfClansInAlly { get; set; }

        ///<summary>Number of members needed to request a clan war.</summary>
        [JsonProperty(PropertyName = "ClanMembersForWar")]
        public int ClanMembersForWar { get; set; }

        ///<summary>Number of days needed by a clan to war back another clan.</summary>
        [JsonProperty(PropertyName = "ClanWarPenaltyWhenEnded")]
        public int ClanWarPenaltyWhenEnded { get; set; }

        ///<summary>Privilege browse warehouse enables at the same time also withdraw from warehouse!.</summary>
        [JsonProperty(PropertyName = "MembersCanWithdrawFromClanWH")]
        public bool MembersCanWithdrawFromClanWH { get; set; }

        ///<summary>Remove Castle circlets after a clan lose its castle or a player leaves a clan? - default true.</summary>
        [JsonProperty(PropertyName = "RemoveCastleCirclets")]
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
        [JsonProperty(PropertyName = "RefreshTime")]
        public int RefreshTime { get; set; }

        ///<summary>Manor Refresh Time for Min's, Default 00 so at the start of the hour.</summary>
        [JsonProperty(PropertyName = "RefreshMin")]
        public int RefreshMin { get; set; }

        ///<summary>Manor Next Period Approve Time in Military hours Default 6am.</summary>
        [JsonProperty(PropertyName = "ApproveTime")]
        public int ApproveTime { get; set; }

        ///<summary>Manor Next Period Approve Time for Min's, Default 00 so at the start of the hour.</summary>
        [JsonProperty(PropertyName = "ApproveMin")]
        public int ApproveMin { get; set; }

        ///<summary>Manor Maintenance time, Default 6 minutes.</summary>
        [JsonProperty(PropertyName = "MaintenancePeriod")]
        public int MaintenancePeriod { get; set; }

        ///<summary>Manor Save Type. 1-Save data into db after every action; Default false.</summary>
        [JsonProperty(PropertyName = "SaveAllActions")]
        public bool SaveAllActions { get; set; }

        ///<summary>Manor Save Period (used only if SaveAllActions=false) Default very 2 hours.</summary>
        [JsonProperty(PropertyName = "SavePeriodRate")]
        public int SavePeriodRate { get; set; }
    }

    ///<summary>Clan Hall functions price.</summary>
    ///<summary>Price = 7 day(one day price = price/7).</summary>
    public class ClanHallFunctionsPrice
    {
        ///<summary>Teleport function price.</summary>
        [JsonProperty(PropertyName = "Teleport")]
        public Teleport Teleport { get; set; }

        ///<summary>Support magic buff function price.</summary>
        [JsonProperty(PropertyName = "Support")]
        public Support Support { get; set; }

        ///<summary>MpRegeneration function price.</summary>
        [JsonProperty(PropertyName = "MpRegeneration")]
        public MpRegeneration MpRegeneration { get; set; }

        ///<summary>HpRegeneration function price.</summary>
        [JsonProperty(PropertyName = "HpRegeneration")]
        public HpRegeneration HpRegeneration { get; set; }

        ///<summary>ExpRegeneration function price.</summary>
        [JsonProperty(PropertyName = "ExpRegeneration")]
        public ExpRegeneration ExpRegeneration { get; set; }

        ///<summary>ItemCreation function price.</summary>
        [JsonProperty(PropertyName = "ItemCreation")]
        public ItemCreation ItemCreation { get; set; }

        ///<summary>Decor curtain function price.</summary>
        [JsonProperty(PropertyName = "Curtain")]
        public Curtain Curtain { get; set; }

        ///<summary>Decor front platform function price.</summary>
        [JsonProperty(PropertyName = "FrontPlatform")]
        public FrontPlatform FrontPlatform { get; set; }
    }

    ///<summary>Teleport function price.</summary>
    public class Teleport
    {
        ///<summary>1st level.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }
    }

    ///<summary>Support magic buff function price.</summary>
    public class Support
    {
        ///<summary>1st level.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }

        ///<summary>3rd level.</summary>
        [JsonProperty(PropertyName = "Lvl3")]
        public int Lvl3 { get; set; }

        ///<summary>4th level.</summary>
        [JsonProperty(PropertyName = "Lvl4")]
        public int Lvl4 { get; set; }

        ///<summary>5th level.</summary>
        [JsonProperty(PropertyName = "Lvl5")]
        public int Lvl5 { get; set; }

        ///<summary>6th level.</summary>
        [JsonProperty(PropertyName = "Lvl6")]
        public int Lvl6 { get; set; }

        ///<summary>7th level.</summary>
        [JsonProperty(PropertyName = "Lvl7")]
        public int Lvl7 { get; set; }

        ///<summary>8th level.</summary>
        [JsonProperty(PropertyName = "Lvl8")]
        public int Lvl8 { get; set; }
    }

    ///<summary>MpRegeneration function price.</summary>
    public class MpRegeneration
    {
        ///<summary>5% MpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>10% MpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }

        ///<summary>15% MpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl3")]
        public int Lvl3 { get; set; }

        ///<summary>30% MpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl4")]
        public int Lvl4 { get; set; }

        ///<summary>40% MpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl5")]
        public int Lvl5 { get; set; }
    }

    ///<summary>HpRegeneration function price.</summary>
    public class HpRegeneration
    {
        ///<summary>20% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>40% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }

        ///<summary>80% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl3")]
        public int Lvl3 { get; set; }

        ///<summary>100% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl4")]
        public int Lvl4 { get; set; }

        ///<summary>120% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl5")]
        public int Lvl5 { get; set; }

        ///<summary>140% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl6")]
        public int Lvl6 { get; set; }

        ///<summary>160% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl7")]
        public int Lvl7 { get; set; }

        ///<summary>180% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl8")]
        public int Lvl8 { get; set; }

        ///<summary>200% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl9")]
        public int Lvl9 { get; set; }

        ///<summary>220% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl10")]
        public int Lvl10 { get; set; }

        ///<summary>240% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl11")]
        public int Lvl11 { get; set; }

        ///<summary>260% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl12")]
        public int Lvl12 { get; set; }

        ///<summary>300% HpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl13")]
        public int Lvl13 { get; set; }
    }

    ///<summary>ExpRegeneration function price.</summary>
    public class ExpRegeneration
    {
        //<summary>5% ExpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>10% ExpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }

        ///<summary>15% ExpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl3")]
        public int Lvl3 { get; set; }

        ///<summary>25% ExpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl4")]
        public int Lvl4 { get; set; }

        ///<summary>35% ExpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl5")]
        public int Lvl5 { get; set; }

        ///<summary>40% ExpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl6")]
        public int Lvl6 { get; set; }

        ///<summary>50% ExpRegeneration.</summary>
        [JsonProperty(PropertyName = "Lvl7")]
        public int Lvl7 { get; set; }
    }

    ///<summary>Creation item function price.</summary>
    public class ItemCreation
    {
        ///<summary>1st level.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }

        ///<summary>3rd level.</summary>
        [JsonProperty(PropertyName = "Lvl3")]
        public int Lvl3 { get; set; }
    }

    ///<summary>Need core support, need more information on functions in different. Clan Hall in different Towns.</summary>
    ///<summary>Decor curtain function price.</summary>
    public class Curtain
    {
        ///<summary>1st level.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }
    }

    ///<summary>Need core support, need more information on functions in different. Clan Hall in different Towns.</summary>
    ///<summary>Decor front platform function price.</summary>
    public class FrontPlatform
    {
        ///<summary>1st level.</summary>
        [JsonProperty(PropertyName = "Lvl1")]
        public int Lvl1 { get; set; }

        ///<summary>2nd level.</summary>
        [JsonProperty(PropertyName = "Lvl2")]
        public int Lvl2 { get; set; }
    }
}