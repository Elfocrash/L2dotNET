using L2dotNET.Game.model.npcs.cubic;
using L2dotNET.Game.world;

namespace L2dotNET.Game.model.skills2.effects
{
    public class i_summon_cubic : TEffect
    {
        private int id;
        private int lvl;
        public override void build(string str)
        {
            string[] v = str.Split(' ');
            id = int.Parse(v[1]);
            lvl = int.Parse(v[2]);
        }

        public i_summon_cubic()
        {
            type = TEffectType.i_summon_cubic;
        }

        public override TEffectResult onStart(L2Character caster, world.L2Character target)
        {
            L2Player player = caster as L2Player;
            CubicTemplate template = CubicController.getController().getCubic(id, lvl);

            Cubic cub = new Cubic(player, template);
            player.AddCubic(cub, false);

            return new TEffectResult().AsTotalUI();
        }

        public override bool canUse(L2Character caster)
        {
            return caster is L2Player;
        }
    }
}
