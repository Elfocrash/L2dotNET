using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class b_attack_spd : TEffect
    {
        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            target.CharacterStat.Apply(this);

            TEffectResult ter = new TEffectResult();
            ter.TotalUI = 1;
            return ter;
        }

        public override TEffectResult onEnd(L2Character caster, L2Character target)
        {
            target.CharacterStat.Stop(this);

            TEffectResult ter = new TEffectResult();
            ter.TotalUI = 1;
            return ter;
        }
    }
}