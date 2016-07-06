using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowUpdate : GameServerNetworkPacket
    {
        private readonly L2Player _member;

        public PartySmallWindowUpdate(L2Player member)
        {
            _member = member;
        }

        protected internal override void Write()
        {
            WriteC(0x52);
            WriteD(_member.ObjId);
            WriteS(_member.Name);
            WriteD(_member.CurCp);
            WriteD(_member.CharacterStat.GetStat(EffectType.BMaxCp));
            WriteD(_member.CurHp);
            WriteD(_member.CharacterStat.GetStat(EffectType.BMaxHp));
            WriteD(_member.CurMp);
            WriteD(_member.CharacterStat.GetStat(EffectType.BMaxMp));
            WriteD(_member.Level);
            WriteD((int)_member.ActiveClass.ClassId.Id);
        }
    }
}