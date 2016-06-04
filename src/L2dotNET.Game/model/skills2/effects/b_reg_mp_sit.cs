using L2dotNET.GameService.model.skills2.speceffects;

namespace L2dotNET.GameService.model.skills2.effects
{
    class b_reg_mp_sit : TEffect
    {
        private TSpecEffect ef;
        public override void build(string str)
        {
            string val = str.Split(' ')[1];
            ef = new b_regen_mp_by_sit(double.Parse(val.Substring(1)));
            ef.mul = val.StartsWith("*");
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            if (!(target is L2Player))
                return nothing;

            ((L2Player)target).specEffects.Add(ef);

            return nothing;
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
        {
            if (!(target is L2Player))
                return nothing;

            lock(((L2Player)target).specEffects)
                ((L2Player)target).specEffects.Remove(ef);

            return nothing;
        }
    }
}
