using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowUpdate : GameServerNetworkPacket
    {
        private readonly L2Player member;

        public PartySmallWindowUpdate(L2Player member)
        {
            this.member = member;
        }

        protected internal override void write()
        {
            writeC(0x52);
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
        }
    }
}