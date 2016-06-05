using System.Collections.Generic;
using L2dotNET.GameService.Model.communities;

namespace L2dotNET.GameService.network.serverpackets
{
    class PledgeShowMemberListAll : GameServerNetworkPacket
    {
        private readonly L2Clan clan;
        private readonly e_ClanType type;

        public PledgeShowMemberListAll(L2Clan clan, e_ClanType type)
        {
            this.clan = clan;
            this.type = type;
        }

        protected internal override void write()
        {
            writeC(0x5a);

            writeD((short)type == 0 ? 0 : 1);
            writeD(clan.ClanID);
            writeD((short)type);
            writeS(clan.Name);
            writeS(clan.ClanMasterName);

            writeD(clan.CrestID);
            writeD(clan.Level);
            writeD(clan.CastleID);
            writeD(clan.HideoutID);
            writeD(clan.FortressID);
            writeD(clan.ClanRank);
            writeD(clan.ClanNameValue);
            writeD(clan.Status);
            writeD(clan.Guilty);
            writeD(clan.AllianceID);
            writeS(clan.AllianceName);
            writeD(clan.AllianceCrestId);
            writeD(clan.InWar);
            writeD(clan.JoinDominionWarID);
            List<ClanMember> members = clan.getClanMembers(type, 0);
            writeD(members.Count);

            foreach (ClanMember m in members)
            {
                writeS(m.Name);
                writeD(m.Level);
                writeD(m.classId);
                writeD(m.Gender);
                writeD(m.Race);
                writeD(m.OnlineID()); // 1=online 0=offline
                writeD(m.haveMaster()); //c5 makes the name yellow. member is in academy and has a sponsor 
            }
        }
    }
}