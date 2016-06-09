using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    public class p_block_spell : TEffect
    {
        public p_block_spell()
        {
            type = TEffectType.p_block_spell;
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            target.Mute(1, HashID, true);
            return new TEffectResult().AsTotalUI();
        }

        public override TEffectResult onEnd(L2Character caster, L2Character target)
        {
            target.Mute(1, HashID, false);
            if (target.MutedMagically)
                return new TEffectResult().AsTotalUI();

            return nothing;
        }
    }
}