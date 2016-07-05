using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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
            if (target.MutedSpecial)
            {
                return new EffectResult().AsTotalUi();
            }

            return Nothing;
        }
    }
}