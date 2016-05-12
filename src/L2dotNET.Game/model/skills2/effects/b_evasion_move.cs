using L2dotNET.GameService.model.skills2.speceffects;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2.effects
{
    class b_evasion_move : TEffect
    {
        TSpecEffect ef;
        public override void build(string str)
        {
            ef = new b_evasion_by_move(double.Parse(str.Split(' ')[1]));
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            if (!(target is L2Player))
                return nothing;

            ((L2Player)target).specEffects.Add(ef);

            TEffectResult ter = new TEffectResult();
            ter.addSU(StatusUpdate.EVASION, ((world.L2Character)target).CharacterStat.getStat(TEffectType.b_evasion));
            return nothing;
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
        {
            if (!(target is L2Player))
                return nothing;

            lock(((L2Player)target).specEffects)
                ((L2Player)target).specEffects.Remove(ef);

            TEffectResult ter = new TEffectResult();
            ter.addSU(StatusUpdate.EVASION, ((world.L2Character)target).CharacterStat.getStat(TEffectType.b_evasion));
            return nothing;
        }
    }
}
