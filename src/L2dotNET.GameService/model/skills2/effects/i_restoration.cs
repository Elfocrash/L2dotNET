using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class i_restoration : TEffect
    {
        private int itemId;
        private long count;

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            itemId = int.Parse(v[1]);
            count = long.Parse(v[2]);
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            L2Player player = target as L2Player;
            if (player == null)
                return nothing;

            player.addItem(itemId, count);

            return nothing;
        }

        public override bool canUse(L2Character caster)
        {
            return caster is L2Player;
        }
    }
}