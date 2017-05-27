using L2dotNET.model.player;
using L2dotNET.model.skills2.speceffects;
using L2dotNET.Utility;
using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    class BRegHpSit : Effect
    {
        private SpecEffect _ef;

        public override void Build(string str)
        {
            string val = str.Split(' ')[1];
            _ef = new BRegenHpBySit(double.Parse(val.Substring(1)))
            {
                Mul = val.StartsWithIgnoreCase("*")
            };
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
                return Nothing;

            ((L2Player)target).SpecEffects.Add(_ef);

            return Nothing;
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
                return Nothing;

            lock (((L2Player)target).SpecEffects)
                ((L2Player)target).SpecEffects.Remove(_ef);

            return Nothing;
        }
    }
}