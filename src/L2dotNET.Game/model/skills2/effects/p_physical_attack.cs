using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.Model.skills2.effects
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