using System;
using System.Collections;
using System.Collections.Generic;
using L2dotNET.Game.model.player;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.world;
using L2dotNET.Game.templates;

namespace L2dotNET.Game.model.stats
{
    public class CStats
    {
        public CStats(L2Character owner)
        {
            this.owner = owner;
        }
        private L2Character owner;

        public Hashtable statTemplate = new Hashtable();
        public Hashtable statBuff = new Hashtable();

        private List<TEffect> activeEffects = new List<TEffect>();

        public double SpecBonusRegHP, URegHpMul = 1.0, SpecBonusRegMP, URegMpMul = 1.0, SpecBonusRegCP, SpecBonusEvasion;

        public double getStat(TEffectType type)
        {
            if (!statBuff.ContainsKey(type))
            {
                if (!statTemplate.ContainsKey(type))
                    return 0;
                else
                    return (double)statTemplate[type];
            }

            return (double)statBuff[type];
        }

        private double getTemplate(TEffectType type)
        {
            if (!statTemplate.ContainsKey(type))
                return 0;

            return (double)statTemplate[type];
        }

        public TEffectResult Apply(List<TEffect> effects, L2Character caster)
        {
            TEffectResult result = new TEffectResult();

            foreach (TEffect effect in effects)
            {
                TEffectResult ter = effect.onStart(caster, owner);
                if (result.TotalUI == 0)
                    result.TotalUI = ter.TotalUI;

                if (ter.sus != null)
                    result.addAll(ter.sus);
            }

            return result;
        }

        public double[] Apply(TEffect effect)
        {
            activeEffects.Add(effect);

            double basevalue = getTemplate(effect.type);
            double newvalue = basevalue;
            double buffvalue = getStat(effect.type);

            List<TEffect> arif = null;
            foreach (TEffect cc in activeEffects)
            {
                if (cc.type != effect.type)
                    continue;

                if (cc.supMethod != null)
                {
                    if (cc.supMethod.Method <= SupMethod.SUB)
                    {
                        if (arif == null)
                            arif = new List<TEffect>();

                        arif.Add(cc);
                        continue;
                    }

                    newvalue = calcSupMethod(newvalue, cc.supMethod);
                }
                //log.Info($"newvalue! #1 { newvalue } { cc.supMethod.Value } { cc.type }");
            }

            if (arif != null)
            {
                foreach (TEffect cc in arif)
                {
                    newvalue = calcSupMethod(newvalue, cc.supMethod);
                    //log.Info($"newvalue! arif { newvalue } { cc.supMethod.Value } { cc.type }");                    
                }
            }

            if (statBuff.ContainsKey(effect.type))
                lock (statBuff)
                    statBuff.Remove(effect.type);

            statBuff.Add(effect.type, newvalue);
            return new double[] { buffvalue, newvalue };
        }

        public TEffectResult Stop(List<TEffect> effects, L2Character caster)
        {
            TEffectResult result = new TEffectResult();

            foreach (TEffect effect in effects)
            {
                TEffectResult ter = effect.onEnd(caster, owner);
                if (result.TotalUI == 0)
                    result.TotalUI = ter.TotalUI;

                if (ter.sus != null)
                    result.addAll(ter.sus);
            }

            return result;
        }

        public double[] Stop(TEffect effect)
        {
            activeEffects.Remove(effect);

            double basevalue = getTemplate(effect.type);
            double newvalue = basevalue;
            double buffvalue = getStat(effect.type);

            List<TEffect> arif = null;
            foreach (TEffect cc in activeEffects)
            {
                if (cc.type != effect.type)
                    continue;

                if (cc.supMethod.Method <= SupMethod.SUB)
                {
                    if (arif == null)
                        arif = new List<TEffect>();

                    arif.Add(cc);
                    continue;
                }

                newvalue = calcSupMethod(newvalue, cc.supMethod);
            }

            if (arif != null)
            {
                foreach (TEffect cc in arif)
                {
                    newvalue = calcSupMethod(newvalue, cc.supMethod);
                }
            }

            if (statBuff.ContainsKey(effect.type))
                lock (statBuff)
                    statBuff.Remove(effect.type);

            statBuff.Add(effect.type, newvalue);

            return new double[] { buffvalue, newvalue };
        }

        private double calcSupMethod(double val, SupMethod sup)
        {
            double retval = 0;
            switch (sup.Method)
            {
                case SupMethod.MUL:
                    retval = val * sup.Value;
                    break;
                case SupMethod.DIV:
                    retval = val / sup.Value;
                    break;
                case SupMethod.ADD:
                case SupMethod.SUB:
                    retval = val + sup.Value;
                    break;
            }

            return retval;
        }

        public bool calcDebuffSuccess(TSkill skill, L2Character caster)
        {
            bool success = false;

            int rnd = new Random().Next(0, 100);
            success = rnd <= skill.activate_rate;

            caster.sendMessage(skill.skill_id + " success " + rnd + "% (" + skill.activate_rate + "% base)");

            return success;
        }

        public void addTemplate(TEffectType type, double value)
        {
            statTemplate.Add(type, value);
        }

        public void setTemplate(PcTemplate template)
        {
            addTemplate(TEffectType.p_physical_attack, template.BasePAtk);
            addTemplate(TEffectType.p_physical_defense, template.BasePDef);
            addTemplate(TEffectType.p_magical_attack, template.BaseMAtk);
            addTemplate(TEffectType.p_magical_defense, template.BaseMDef);
            addTemplate(TEffectType.p_speed, 700);//template.runspd);

            addTemplate(TEffectType.b_max_weight, 2500000.0);
            addTemplate(TEffectType.b_accuracy, 50);
            addTemplate(TEffectType.b_critical_rate, 100);
            addTemplate(TEffectType.b_evasion, 50);
            //addTemplate(TEffectType.b_breath, template.B);

            addTemplate(TEffectType.b_attack_spd, template.BasePAtkSpd);
            addTemplate(TEffectType.b_casting_spd, 333);

            addTemplate(TEffectType.b_max_hp, template.HpTable[owner.Level]);
            addTemplate(TEffectType.b_reg_hp, template.BaseHpReg);
            addTemplate(TEffectType.b_max_mp, template.MpTable[owner.Level]);
            addTemplate(TEffectType.b_reg_mp, template.BaseMpReg);
            addTemplate(TEffectType.b_max_cp, template.CpTable[owner.Level]);
            addTemplate(TEffectType.b_reg_cp, 50);
        }

        public void setTemplate(npcs.NpcTemplate template)
        {
            addTemplate(TEffectType.p_physical_attack, template.base_physical_attack);
            addTemplate(TEffectType.p_physical_defense, template.base_defend);
            addTemplate(TEffectType.p_magical_attack, template.base_magic_attack);
            addTemplate(TEffectType.p_magical_defense, template.base_magic_defend);
            addTemplate(TEffectType.p_speed, template.RunSpeed);

            addTemplate(TEffectType.b_accuracy, 50);
            addTemplate(TEffectType.b_critical_rate, template.base_critical);
            addTemplate(TEffectType.b_evasion, 50);

            addTemplate(TEffectType.b_attack_spd, template.base_attack_speed);
            addTemplate(TEffectType.b_casting_spd, template.base_magic_speed);

            addTemplate(TEffectType.b_max_hp, template.org_hp);
            owner.CurHP = template.org_hp;
            addTemplate(TEffectType.b_reg_hp, template.org_hp_regen);
            addTemplate(TEffectType.b_max_mp, template.org_mp);
            owner.CurMP = template.org_mp;
            addTemplate(TEffectType.b_reg_mp, template.org_mp_regen);
        }
    }
}
