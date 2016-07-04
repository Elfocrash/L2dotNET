using L2dotNET.GameService.Model.Communities;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeShowInfoUpdate : GameServerNetworkPacket
    {
        private readonly L2Clan _clan;

        public PledgeShowInfoUpdate(L2Clan clan)
        {
            _clan = clan;
        }

        protected internal override void Write()
        {
            WriteC(0x8e);
            WriteD(_clan.ClanId);
            WriteD(_clan.CrestId);
            WriteD(_clan.Level);
            WriteD(_clan.CastleId);
            WriteD(_clan.HideoutId);
            WriteD(_clan.FortressId);
            WriteD(_clan.ClanRank);
            WriteD(_clan.ClanNameValue);
            WriteD(_clan.Status);
            WriteD(_clan.Guilty);
            WriteD(_clan.AllianceId);
            WriteS(_clan.AllianceName);
            WriteD(_clan.AllianceCrestId);
            WriteD(_clan.InWar);
            WriteD(_clan.LargeCrestId);
            WriteD(_clan.JoinDominionWarId);
        }
    }
}