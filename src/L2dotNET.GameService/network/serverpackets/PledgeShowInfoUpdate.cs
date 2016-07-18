using L2dotNET.GameService.Model.Communities;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeShowInfoUpdate : GameserverPacket
    {
        private readonly L2Clan _clan;

        public PledgeShowInfoUpdate(L2Clan clan)
        {
            _clan = clan;
        }

        public override void Write()
        {
            WriteByte(0x8e);
            WriteInt(_clan.ClanId);
            WriteInt(_clan.CrestId);
            WriteInt(_clan.Level);
            WriteInt(_clan.CastleId);
            WriteInt(_clan.HideoutId);
            WriteInt(_clan.FortressId);
            WriteInt(_clan.ClanRank);
            WriteInt(_clan.ClanNameValue);
            WriteInt(_clan.Status);
            WriteInt(_clan.Guilty);
            WriteInt(_clan.AllianceId);
            WriteString(_clan.AllianceName);
            WriteInt(_clan.AllianceCrestId);
            WriteInt(_clan.InWar);
            WriteInt(_clan.LargeCrestId);
            WriteInt(_clan.JoinDominionWarId);
        }
    }
}