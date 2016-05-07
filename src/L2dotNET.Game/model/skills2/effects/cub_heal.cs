using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;

namespace L2dotNET.Game.model.skills2.effects
{
    public class cub_heal : TEffect
    {
        private int power;
        public override void build(string str)
        {
            string[] v = str.Split(' ');
            power = int.Parse(v[1]);
        }

        public cub_heal()
        {
            type = TEffectType.cub_heal;
        }

        public override TEffectResult onStart(L2Character caster, world.L2Character target)
        {
            double current = target.CurHP;
            target.CurHP = power;
            double next = target.CurHP;

            int diff = (int)(next - current);
            //$s1 HP has been restored.
            target.sendPacket(new SystemMessage(1066).AddNumber(diff));
            return nothing;
        }
    }
}
