using L2dotNET.model.playable;
using L2dotNET.model.skills2;

namespace L2dotNET.Network.serverpackets
{
    class ExPartyPetWindowAdd : GameserverPacket
    {
        private readonly L2Summon _summon;

        public ExPartyPetWindowAdd(L2Summon summon)
        {
            _summon = summon;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x18);
            WriteInt(_summon.ObjId);
            WriteInt(_summon.Template.NpcId + 1000000);
            WriteInt(_summon.ObjectSummonType);
            WriteInt(_summon.Owner.ObjId);
            WriteString(_summon.Name);
            WriteInt(_summon.CurHp);
            WriteInt(_summon.CharacterStat.GetStat(EffectType.BMaxHp));
            WriteInt(_summon.CurMp);
            WriteInt(_summon.CharacterStat.GetStat(EffectType.BMaxMp));
            WriteInt(_summon.Level);
        }
    }
}