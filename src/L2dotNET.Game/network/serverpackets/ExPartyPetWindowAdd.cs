using L2dotNET.Game.model.playable;
using L2dotNET.Game.model.skills2;

namespace L2dotNET.Game.network.l2send
{
    class ExPartyPetWindowAdd : GameServerNetworkPacket
    {
        private L2Summon _summon;
        public ExPartyPetWindowAdd(L2Summon summon)
        {
            this._summon = summon;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x18);
            writeD(_summon.ObjID);
            writeD(_summon.Template.NpcId + 1000000);
            writeD(_summon.ObjectSummonType);
            writeD(_summon.Owner.ObjID);
            writeS(_summon.Name);
            writeD(_summon.CurHP);
            writeD(_summon.CharacterStat.getStat(TEffectType.b_max_hp));
            writeD(_summon.CurMP);
            writeD(_summon.CharacterStat.getStat(TEffectType.b_max_mp));
            writeD(_summon.Level);
        }
    }
}
