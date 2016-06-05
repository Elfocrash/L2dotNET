using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Model.skills2.effects
{
    class i_remove_soul : TEffect
    {
        public i_remove_soul()
        {
            type = TEffectType.i_remove_soul;
        }

        private byte count;

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            count = byte.Parse(v[1]);
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            L2Player player = target as L2Player;
            if (player == null)
                return nothing;

            player.ReduceSouls(count);

            return nothing;
        }
    }
}