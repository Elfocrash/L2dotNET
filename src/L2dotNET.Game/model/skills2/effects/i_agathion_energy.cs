using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Model.skills2.effects
{
    class i_agathion_energy : TEffect
    {
        public i_agathion_energy()
        {
            type = TEffectType.i_agathion_energy;
        }

        private int count;

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            count = int.Parse(v[1]);
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            L2Player player = target as L2Player;
            if (player == null)
                return nothing;

            player.UpdateAgathionEnergy(count);

            return nothing;
        }
    }
}