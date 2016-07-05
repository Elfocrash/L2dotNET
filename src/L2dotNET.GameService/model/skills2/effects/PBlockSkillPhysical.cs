using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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
            if (target.MutedPhysically)
            {
                return new EffectResult().AsTotalUi();
            }

            return Nothing;
        }
    }
}