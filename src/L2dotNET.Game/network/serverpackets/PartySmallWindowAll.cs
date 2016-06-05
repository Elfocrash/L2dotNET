using L2dotNET.GameService.model.skills2;

namespace L2dotNET.GameService.network.l2send
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
            writeD(party.leader.ObjID);
            writeD(party.itemDistribution);
            writeD(party.Members.Count);

            foreach (L2Player member in party.Members)
            {
                writeD(member.ObjID);
                writeS(member.Name);

                writeD(member.CurCP);
                writeD(member.CharacterStat.getStat(TEffectType.b_max_cp));
                writeD(member.CurHP);
                writeD(member.CharacterStat.getStat(TEffectType.b_max_hp));
                writeD(member.CurMP);
                writeD(member.CharacterStat.getStat(TEffectType.b_max_mp));
                writeD(member.Level);

                writeD((int)member.ActiveClass.ClassId.Id);
                writeD(0x00); // writeD(0x01); ??
                writeD((int)member.BaseClass.ClassId.ClassRace);
            }
        }
    }
}