using L2dotNET.GameService.Model.Skills2.Conds;
using L2dotNET.GameService.Model.Skills2.Effects;

namespace L2dotNET.GameService.Model.Skills2
{
    class EffectRegistrator
    {
        private static readonly EffectRegistrator St = new EffectRegistrator();

        public static EffectRegistrator GetInstance()
        {
            return St;
        }

        public Effect BuildProc(EffectType type, string str)
        {
            Effect effect;
            switch (type)
            {
                case EffectType.PSpeed:
                    effect = new PSpeed();
                    break;
                case EffectType.PPhysicalDefense:
                    effect = new PPhysicalDefence();
                    break;
                case EffectType.IRestoration:
                    effect = new Restoration();
                    break;
                case EffectType.IFatalBlow:
                    effect = new FatalBlow();
                    break;
                case EffectType.IDeath:
                    effect = new Death();
                    break;
                case EffectType.PBlockSkillPhysical:
                    effect = new PBlockSkillPhysical();
                    break;
                case EffectType.PBlockSkillSpecial:
                    effect = new PBlockSkillSpecial();
                    break;
                case EffectType.PBlockSpell:
                    effect = new PBlockSpell();
                    break;
                case EffectType.ITargetCancel:
                    effect = new TargetCancel();
                    break;
                case EffectType.PDefenceAttribute:
                    effect = new PDefenceAttribute();
                    break;
                case EffectType.IPAttack:
                    effect = new IpAttack();
                    break;
                case EffectType.IRemoveSoul:
                    effect = new RemoveSoul();
                    break;
                case EffectType.IAgathionEnergy:
                    effect = new AgathionEnergy();
                    break;
                case EffectType.ISummonCubic:
                    effect = new SummonCubic();
                    break;
                case EffectType.CubHeal:
                    effect = new CubHeal();
                    break;
                default:
                    return null;
            }

            effect.Build(str);
            return effect;
        }

        public SkillCond BuildCond(SkillCondType type, string str)
        {
            SkillCond cond = null;
            switch (type)
            {
                case SkillCondType.CanSummonCubic:
                    cond = new CanSummonCubic();
                    break;
            }

            if (cond != null)
            {
                cond.Build(str);
            }

            return cond;
        }
    }
}