using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
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