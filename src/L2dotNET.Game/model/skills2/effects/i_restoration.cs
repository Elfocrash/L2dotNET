
namespace L2dotNET.GameService.model.skills2.effects
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

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            L2Player player = target as L2Player;
            if (player == null)
                return nothing;

            player.addItem(itemId, count);

            return nothing;
        }

        public override bool canUse(world.L2Character caster)
        {
            return caster is L2Player;
        }
    }
}
