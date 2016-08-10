using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    public class PBlockSkillPhysical : Effect
    {
        public PBlockSkillPhysical()
        {
            Type = EffectType.PBlockSkillPhysical;
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            target.Mute(0, HashId, true);
            return new EffectResult().AsTotalUi();
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            target.Mute(0, HashId, false);
            return target.MutedPhysically ? new EffectResult().AsTotalUi() : Nothing;
        }
    }
}