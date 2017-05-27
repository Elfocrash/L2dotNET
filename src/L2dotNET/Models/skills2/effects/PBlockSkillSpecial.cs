using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    public class PBlockSkillSpecial : Effect
    {
        public PBlockSkillSpecial()
        {
            Type = EffectType.PBlockSkillSpecial;
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            target.Mute(2, HashId, true);
            return new EffectResult().AsTotalUi();
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            target.Mute(2, HashId, false);
            return target.MutedSpecial ? new EffectResult().AsTotalUi() : Nothing;
        }
    }
}