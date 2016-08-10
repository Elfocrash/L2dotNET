using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2.effects
{
    class PMagicalAttack : Effect
    {
        public PMagicalAttack()
        {
            SuId = StatusUpdate.MAtk;
            Type = EffectType.PMagicalAttack;
        }
    }
}