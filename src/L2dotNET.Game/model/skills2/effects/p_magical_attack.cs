using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.skills2.effects
{
    class p_magical_attack : TEffect
    {
        public p_magical_attack()
        {
            SU_ID = StatusUpdate.M_ATK;
            type = TEffectType.p_magical_attack;
        }
    }
}
