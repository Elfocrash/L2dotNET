using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.skills2.effects
{
    class p_physical_attack : TEffect
    {
        public p_physical_attack()
        {
            SU_ID = StatusUpdate.P_ATK;
            type = TEffectType.p_physical_attack;
        }

        public override void build(string str)
        {
             string[] v = str.Split(' ');
             SetCondition(v[1]);
             SetSup(v[2]);
        }
    }
}
