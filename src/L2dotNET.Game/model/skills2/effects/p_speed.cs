
namespace L2dotNET.GameService.model.skills2.effects
{
    class p_speed : TEffect
    {
        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            double[] val = target.CharacterStat.Apply(this);

            TEffectResult ter = new TEffectResult();
            ter.TotalUI = 1;
            return ter;
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
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
