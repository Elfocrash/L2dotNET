using L2dotNET.Game.model.skills2;

namespace L2dotNET.Game.network.l2send
{
    class PartySmallWindowAll : GameServerNetworkPacket
    {
        private L2Party party;
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
                writeD(member.ActiveClass.id);
                writeD(0x00);// writeD(0x01); ??
                writeD(member.BaseClass.race);
                writeD(0); // T2.3
                writeD(0); // T2.3

                if (member.Summon != null)
                {
                    writeD(member.Summon.ObjID);
                    writeD(member.Summon.Template.NpcId + 1000000);
                    writeD(member.Summon.ObjectSummonType);
                    writeS(member.Summon.Name);
                    writeD(member.Summon.CurHP);
                    writeD(member.Summon.CharacterStat.getStat(TEffectType.b_max_hp));
                    writeD(member.Summon.CurMP);
                    writeD(member.Summon.CharacterStat.getStat(TEffectType.b_max_mp));
                    writeD(member.Summon.Level);
                }
                else
                {
                    writeD(0x00);
                }
            }
        }
    }
}
