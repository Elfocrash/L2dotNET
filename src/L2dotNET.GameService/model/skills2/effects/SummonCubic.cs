using L2dotNET.GameService.Model.Npcs.Cubic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    public class SummonCubic : Effect
    {
        private int _id;
        private int _lvl;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _id = int.Parse(v[1]);
            _lvl = int.Parse(v[2]);
        }

        public SummonCubic()
        {
            Type = EffectType.ISummonCubic;
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!(caster is L2Player))
            {
                return new EffectResult().AsTotalUi();
            }

            L2Player player = (L2Player)caster;
            CubicTemplate template = CubicController.GetController().GetCubic(_id, _lvl);
            Cubic cub = new Cubic(player, template);
            player.AddCubic(cub, false);

            return new EffectResult().AsTotalUi();
        }

        public override bool CanUse(L2Character caster)
        {
            return caster is L2Player;
        }
    }
}