using L2dotNET.GameService.model.skills2.conds;
using L2dotNET.GameService.model.skills2.effects;

namespace L2dotNET.GameService.model.skills2
{
    class TEffectRegistrator
    {
        private static TEffectRegistrator st = new TEffectRegistrator();
        public static TEffectRegistrator getInstance()
        {
            return st;
        }

        public TEffect BuildProc(TEffectType type, string str)
        {
            TEffect effect = null;
            switch (type)
            {
                case TEffectType.p_speed: 
                    effect = new p_speed();
                    break;
                case TEffectType.p_physical_defense: 
                    effect = new p_physical_defence();
                    break;
                case TEffectType.i_restoration: 
                    effect = new i_restoration();
                    break;
                case TEffectType.i_fatal_blow:
                    effect = new i_fatal_blow();
                    break;
                case TEffectType.i_death:
                    effect = new i_death();
                    break;
                case TEffectType.p_block_skill_physical:
                    effect = new p_block_skill_physical();
                    break;
                case TEffectType.p_block_skill_special:
                    effect = new p_block_skill_special();
                    break;
                case TEffectType.p_block_spell:
                    effect = new p_block_spell();
                    break;
                case TEffectType.i_target_cancel:
                    effect = new i_target_cancel();
                    break;
                case TEffectType.p_defence_attribute:
                    effect = new p_defence_attribute();
                    break;
                case TEffectType.i_p_attack:
                    effect = new i_p_attack();
                    break;
                case TEffectType.i_remove_soul:
                    effect = new i_remove_soul();
                    break;
                case TEffectType.i_agathion_energy:
                    effect = new i_agathion_energy();
                    break;
                case TEffectType.i_summon_cubic:
                    effect = new i_summon_cubic();
                    break;
                case TEffectType.cub_heal:
                    effect = new cub_heal();
                    break;
                default:
                    return null;
            }

            effect.build(str);
            return effect;
        }

        public TSkillCond BuildCond(TSkillCondType type, string str)
        {
            TSkillCond cond = null;
            switch (type)
            {
                case TSkillCondType.can_summon_cubic:
                    cond = new can_summon_cubic();
                    break;
            }

            if (cond != null)
                cond.build(str);

            return cond;
        }
    }
}
