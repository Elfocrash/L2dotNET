using L2dotNET.GameService.model.communities;

namespace L2dotNET.GameService.network.l2send
{
    class PledgeShowInfoUpdate : GameServerNetworkPacket
    {
        private readonly L2Clan Clan;
        public PledgeShowInfoUpdate(L2Clan clan)
        {
            Clan = clan;
        }

        protected internal override void write()
        {
            writeC(0x8e);
            writeD(Clan.ClanID);
            writeD(Clan.CrestID);
            writeD(Clan.Level); 
            writeD(Clan.CastleID);
            writeD(Clan.HideoutID);
            writeD(Clan.FortressID);
            writeD(Clan.ClanRank);
            writeD(Clan.ClanNameValue); 
            writeD(Clan.Status);
            writeD(Clan.Guilty);
            writeD(Clan.AllianceID);
            writeS(Clan.AllianceName);
            writeD(Clan.AllianceCrestId);
            writeD(Clan.InWar);
            writeD(Clan.LargeCrestID); 
            writeD(Clan.JoinDominionWarID); 
        }
    }
}
