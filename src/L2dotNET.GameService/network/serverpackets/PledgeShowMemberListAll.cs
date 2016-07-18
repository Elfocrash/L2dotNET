using System.Collections.Generic;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeShowMemberListAll : GameserverPacket
    {
        private readonly L2Clan _clan;
        private readonly EClanType _type;

        public PledgeShowMemberListAll(L2Clan clan, EClanType type)
        {
            _clan = clan;
            _type = type;
        }

        public override void Write()
        {
            WriteByte(0x5a);

            WriteInt((short)_type == 0 ? 0 : 1);
            WriteInt(_clan.ClanId);
            WriteInt((short)_type);
            WriteString(_clan.Name);
            WriteString(_clan.ClanMasterName);

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
            WriteInt(_clan.JoinDominionWarId);
            List<ClanMember> members = _clan.GetClanMembers(_type, 0);
            WriteInt(members.Count);

            foreach (ClanMember m in members)
            {
                WriteString(m.Name);
                WriteInt(m.Level);
                WriteInt(m.ClassId);
                WriteInt(m.Gender);
                WriteInt(m.Race);
                WriteInt(m.OnlineId()); // 1=online 0=offline
                WriteInt(m.HaveMaster()); //c5 makes the name yellow. member is in academy and has a sponsor
            }
        }
    }
}