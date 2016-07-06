using System.Collections.Generic;
using L2dotNET.GameService.Model.Communities;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeShowMemberListAll : GameServerNetworkPacket
    {
        private readonly L2Clan _clan;
        private readonly EClanType _type;

        public PledgeShowMemberListAll(L2Clan clan, EClanType type)
        {
            _clan = clan;
            _type = type;
        }

        protected internal override void Write()
        {
            WriteC(0x5a);

            WriteD((short)_type == 0 ? 0 : 1);
            WriteD(_clan.ClanId);
            WriteD((short)_type);
            WriteS(_clan.Name);
            WriteS(_clan.ClanMasterName);

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
            WriteD(_clan.JoinDominionWarId);
            List<ClanMember> members = _clan.GetClanMembers(_type, 0);
            WriteD(members.Count);

            foreach (ClanMember m in members)
            {
                WriteS(m.Name);
                WriteD(m.Level);
                WriteD(m.ClassId);
                WriteD(m.Gender);
                WriteD(m.Race);
                WriteD(m.OnlineId()); // 1=online 0=offline
                WriteD(m.HaveMaster()); //c5 makes the name yellow. member is in academy and has a sponsor
            }
        }
    }
}