namespace L2dotNET.GameService.model.skills2.effects
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

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            return nothing;
        }
    }
}