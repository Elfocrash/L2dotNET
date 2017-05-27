namespace L2dotNET.model.skills2.effects
{
    class PPhysicalShieldDefence : Effect
    {
        public PPhysicalShieldDefence()
        {
            SuId = -1;
            Type = EffectType.PPhysicalShieldDefence;
        }

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            SetSup(v[1]);
        }
    }
}