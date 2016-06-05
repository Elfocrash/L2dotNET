using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.Model.skills2.effects
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