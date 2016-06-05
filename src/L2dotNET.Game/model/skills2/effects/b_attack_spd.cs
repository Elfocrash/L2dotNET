namespace L2dotNET.GameService.Model.skills2.effects
{
    class b_attack_spd : TEffect
    {
        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            double[] val = ((world.L2Character)target).CharacterStat.Apply(this);

            TEffectResult ter = new TEffectResult();
            ter.TotalUI = 1;
            return ter;
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
        {
            double[] val = ((world.L2Character)target).CharacterStat.Stop(this);

            TEffectResult ter = new TEffectResult();
            ter.TotalUI = 1;
            return ter;
        }
    }
}