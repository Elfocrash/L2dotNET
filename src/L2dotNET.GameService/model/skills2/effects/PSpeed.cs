using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    internal class PSpeed : Effect
    {
        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            target.CharacterStat.Apply(this);

            EffectResult ter = new EffectResult { TotalUi = 1 };
            return ter;
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            target.CharacterStat.Stop(this);

            EffectResult ter = new EffectResult { TotalUi = 1 };
            return ter;
        }

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            SetCondition(v[1]);
            SetSup(v[2]);
        }
    }
}