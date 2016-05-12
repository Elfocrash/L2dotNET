using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2.effects
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
