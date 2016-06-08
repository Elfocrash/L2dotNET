using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    public class p_block_skill_physical : TEffect
    {
        public p_block_skill_physical()
        {
            type = TEffectType.p_block_skill_physical;
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            target.Mute(0, HashID, true);
            return new TEffectResult().AsTotalUI();
        }

        public override TEffectResult onEnd(L2Character caster, L2Character target)
        {
            target.Mute(0, HashID, false);
            if (target.MutedPhysically)
                return new TEffectResult().AsTotalUI();

            return nothing;
        }
    }
}