using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class p_speed : TEffect
    {
        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            double[] val = target.CharacterStat.Apply(this);

            TEffectResult ter = new TEffectResult();
            ter.TotalUI = 1;
            return ter;
        }

        public override TEffectResult onEnd(L2Character caster, L2Character target)
        {
            double[] val = target.CharacterStat.Stop(this);

            TEffectResult ter = new TEffectResult();
            ter.TotalUI = 1;
            return ter;
        }

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            SetCondition(v[1]);
            SetSup(v[2]);
        }
    }
}