using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPartyPetWindowAdd : GameServerNetworkPacket
    {
        private readonly L2Summon _summon;

        public ExPartyPetWindowAdd(L2Summon summon)
        {
            _summon = summon;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x18);
            WriteD(_summon.ObjId);
            WriteD(_summon.Template.NpcId + 1000000);
            WriteD(_summon.ObjectSummonType);
            WriteD(_summon.Owner.ObjId);
            WriteS(_summon.Name);
            WriteD(_summon.CurHp);
            WriteD(_summon.CharacterStat.GetStat(EffectType.BMaxHp));
            WriteD(_summon.CurMp);
            WriteD(_summon.CharacterStat.GetStat(EffectType.BMaxMp));
            WriteD(_summon.Level);
        }
    }
}