using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.skills2.effects
{
    class p_physical_defence : TEffect
    {
        public p_physical_defence()
        {
            SU_ID = StatusUpdate.P_DEF;
            type = TEffectType.p_physical_defense;
        }

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            SetCondition(v[1]);
            SetSup(v[2]);
        }
    }
}
