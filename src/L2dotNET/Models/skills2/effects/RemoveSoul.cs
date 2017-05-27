using L2dotNET.model.player;
using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    class RemoveSoul : Effect
    {
        public RemoveSoul()
        {
            Type = EffectType.IRemoveSoul;
        }

        private byte _count;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _count = byte.Parse(v[1]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
                return Nothing;

            L2Player player = (L2Player)target;
            player.ReduceSouls(_count);

            return Nothing;
        }
    }
}