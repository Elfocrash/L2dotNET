using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    public class CubHeal : Effect
    {
        private int _power;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _power = int.Parse(v[1]);
        }

        public CubHeal()
        {
            Type = EffectType.CubHeal;
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            double current = target.CurHp;
            target.CurHp = _power;
            double next = target.CurHp;

            int diff = (int)(next - current);
            target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1HpRestored).AddNumber(diff));
            return Nothing;
        }
    }
}