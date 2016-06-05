using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.Model.skills2.effects
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