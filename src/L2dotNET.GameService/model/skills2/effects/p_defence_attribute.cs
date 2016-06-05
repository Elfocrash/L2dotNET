using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    public class p_defence_attribute : TEffect
    {
        public p_defence_attribute()
        {
            type = TEffectType.p_defence_attribute;
        }

        private int value;
        private string element;

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            element = v[1];
            value = int.Parse(v[2]);
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            return nothing;
        }
    }
}