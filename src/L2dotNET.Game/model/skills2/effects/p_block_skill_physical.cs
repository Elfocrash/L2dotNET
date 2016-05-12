
namespace L2dotNET.GameService.model.skills2.effects
{
    public class p_block_skill_physical : TEffect
    {
        public p_block_skill_physical()
        {
            type = TEffectType.p_block_skill_physical;
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            target.Mute(0, this.HashID, true);
            return new TEffectResult().AsTotalUI();
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
        {
            target.Mute(0, this.HashID, false);
            if (target.MutedPhysically)
                return new TEffectResult().AsTotalUI();
            else
                return nothing;
        }
    }
}
