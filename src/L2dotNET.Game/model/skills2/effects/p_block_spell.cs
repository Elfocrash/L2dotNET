namespace L2dotNET.GameService.Model.skills2.effects
{
    public class p_block_spell : TEffect
    {
        public p_block_spell()
        {
            type = TEffectType.p_block_spell;
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            target.Mute(1, this.HashID, true);
            return new TEffectResult().AsTotalUI();
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
        {
            target.Mute(1, this.HashID, false);
            if (target.MutedMagically)
                return new TEffectResult().AsTotalUI();
            else
                return nothing;
        }
    }
}