using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    public class PDefenceAttribute : Effect
    {
        public PDefenceAttribute()
        {
            Type = EffectType.PDefenceAttribute;
        }

        private int _value;
        private string _element;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _element = v[1];
            _value = int.Parse(v[2]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            return Nothing;
        }
    }
}