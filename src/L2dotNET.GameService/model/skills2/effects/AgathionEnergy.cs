using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class AgathionEnergy : Effect
    {
        public AgathionEnergy()
        {
            Type = EffectType.IAgathionEnergy;
        }

        private int _count;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _count = int.Parse(v[1]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
            {
                return Nothing;
            }

            L2Player player = (L2Player)target;
            player.UpdateAgathionEnergy(_count);

            return Nothing;
        }
    }
}