using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    class BAttackSpd : Effect
    {
        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            target.CharacterStat.Apply(this);

            EffectResult ter = new EffectResult
            {
                TotalUi = 1
            };
            return ter;
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            target.CharacterStat.Stop(this);

            EffectResult ter = new EffectResult
            {
                TotalUi = 1
            };
            return ter;
        }
    }
}