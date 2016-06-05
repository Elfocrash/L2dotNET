using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            L2Player player = target as L2Player;
            if (player == null)
                return nothing;

            player.ReduceSouls(count);

            return nothing;
        }
    }
}