using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    public class PBlockSpell : Effect
    {
        public PBlockSpell()
        {
            Type = EffectType.PBlockSpell;
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            target.Mute(1, HashId, true);
            return new EffectResult().AsTotalUi();
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            target.Mute(1, HashId, false);
            return target.MutedMagically ? new EffectResult().AsTotalUi() : Nothing;
        }
    }
}