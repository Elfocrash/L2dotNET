using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowAll : GameServerNetworkPacket
    {
        private readonly L2Party party;

        public PartySmallWindowAll(L2Party party)
        {
            this.party = party;
        }

        protected internal override void write()
        {
            writeC(0x4e);
            writeD(party.leader.ObjId);
            writeD(party.itemDistribution);
            writeD(party.Members.Count);

            foreach (L2Player member in party.Members)
            {
                writeD(member.ObjId);
                writeS(member.Name);

                writeD(member.CurCp);
                writeD(member.CharacterStat.getStat(TEffectType.b_max_cp));
                writeD(member.CurHp);
                writeD(member.CharacterStat.getStat(TEffectType.b_max_hp));
                writeD(member.CurMp);
                writeD(member.CharacterStat.getStat(TEffectType.b_max_mp));
                writeD(member.Level);

                writeD((int)member.ActiveClass.ClassId.Id);
                writeD(0x00); // writeD(0x01); ??
                writeD((int)member.BaseClass.ClassId.ClassRace);
            }
        }
    }
}