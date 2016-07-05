using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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
            if (target.MutedMagically)
            {
                return new EffectResult().AsTotalUi();
            }

            return Nothing;
        }
    }
}