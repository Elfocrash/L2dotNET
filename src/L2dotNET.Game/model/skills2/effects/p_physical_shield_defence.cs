namespace L2dotNET.GameService.model.skills2.effects
{
    class p_physical_shield_defence : TEffect
    {
        public p_physical_shield_defence()
        {
            SU_ID = -1;
            type = TEffectType.p_physical_shield_defence;
        }

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            SetSup(v[1]);
        }
    }
}