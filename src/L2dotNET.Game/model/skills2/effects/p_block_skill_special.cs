namespace L2dotNET.GameService.Model.skills2.effects
{
    public class p_block_skill_special : TEffect
    {
        public p_block_skill_special()
        {
            type = TEffectType.p_block_skill_special;
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            target.Mute(2, this.HashID, true);
            return new TEffectResult().AsTotalUI();
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
        {
            target.Mute(2, this.HashID, false);
            if (target.MutedSpecial)
                return new TEffectResult().AsTotalUI();
            else
                return nothing;
        }
    }
}