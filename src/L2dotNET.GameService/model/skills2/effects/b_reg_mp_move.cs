using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2.SpecEffects;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class b_reg_mp_move : TEffect
    {
        private TSpecEffect ef;

        public override void build(string str)
        {
            ef = new b_regen_mp_by_move(double.Parse(str.Split(' ')[1]));
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
                return nothing;

            ((L2Player)target).specEffects.Add(ef);

            return nothing;
        }

        public override TEffectResult onEnd(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
                return nothing;

            lock (((L2Player)target).specEffects)
            {
                ((L2Player)target).specEffects.Remove(ef);
            }

            return nothing;
        }
    }
}