using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.Model.skills2.speceffects;

namespace L2dotNET.GameService.Model.skills2.effects
{
    class b_accuracy_night : TEffect
    {
        private TSpecEffect ef;

        public override void build(string str)
        {
            ef = new b_accuracy_by_night(double.Parse(str.Split(' ')[1]), SkillId, SkillLv);
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

            lock (((L2Player)target).specEffects)
                ((L2Player)target).specEffects.Remove(ef);

            return nothing;
        }
    }
}