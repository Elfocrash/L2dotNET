using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
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

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            double current = target.CurHP;
            target.CurHP = power;
            double next = target.CurHP;

            int diff = (int)(next - current);
            target.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_HP_RESTORED).AddNumber(diff));
            return nothing;
        }
    }
}