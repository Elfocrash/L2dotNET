using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            L2Player player = target as L2Player;
            if (player == null)
                return nothing;

            player.UpdateAgathionEnergy(count);

            return nothing;
        }
    }
}