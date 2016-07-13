using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class Restoration : Effect
    {
        private int _itemId;
        private int _count;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _itemId = int.Parse(v[1]);
            _count = int.Parse(v[2]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
            {
                return Nothing;
            }

            L2Player player = (L2Player)target;
            player.AddItem(_itemId, _count);

            return Nothing;
        }

        public override bool CanUse(L2Character caster)
        {
            return caster is L2Player;
        }
    }
}