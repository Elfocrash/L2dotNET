using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Templates;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Stats
{
    public class CStats
    {
        public CStats(L2Character owner)
        {
            _owner = owner;
        }

        private readonly L2Character _owner;

        public Hashtable StatTemplate = new Hashtable();
        public Hashtable StatBuff = new Hashtable();

        private readonly List<Effect> _activeEffects = new List<Effect>();

        public double SpecBonusRegHp,
                      URegHpMul = 1.0,
                      SpecBonusRegMp,
                      URegMpMul = 1.0,
                      SpecBonusRegCp,
                      SpecBonusEvasion;

        public double GetStat(EffectType type)
        {
            if (!StatBuff.ContainsKey(type))
            {
                if (!StatTemplate.ContainsKey(type))
                {
                    return 0;
                }

                return (double)StatTemplate[type];
            }

            return (double)StatBuff[type];
        }

        private double GetTemplate(EffectType type)
        {
            if (!StatTemplate.ContainsKey(type))
            {
                return 0;
            }

            return (double)StatTemplate[type];
        }

        public EffectResult Apply(List<Effect> effects, L2Character caster)
        {
            EffectResult result = new EffectResult();

            foreach (EffectResult ter in effects.Select(effect => effect.OnStart(caster, _owner)))
            {
                if (result.TotalUi == 0)
                {
                    result.TotalUi = ter.TotalUi;
                }

                if (ter.Sus != null)
                {
                    result.AddAll(ter.Sus);
                }
            }

            return result;
        }

        public double[] Apply(Effect effect)
        {
            _activeEffects.Add(effect);

            double basevalue = GetTemplate(effect.Type);
            double newvalue = basevalue;
            double buffvalue = GetStat(effect.Type);

            List<Effect> arif = null;
            foreach (Effect cc in _activeEffects.Where(cc => (cc.Type == effect.Type) && (cc.SupMethod != null)))
            {
                if (cc.SupMethod.Method <= SupMethod.Sub)
                {
                    if (arif == null)
                    {
                        arif = new List<Effect>();
                    }

                    arif.Add(cc);
                    continue;
                }

                newvalue = CalcSupMethod(newvalue, cc.SupMethod);
            }

            if (arif != null)
            {
                foreach (Effect cc in arif)
                {
                    newvalue = CalcSupMethod(newvalue, cc.SupMethod);
                }
            }

            if (StatBuff.ContainsKey(effect.Type))
            {
                lock (StatBuff)
                {
                    StatBuff.Remove(effect.Type);
                }
            }

            StatBuff.Add(effect.Type, newvalue);
            return new[] { buffvalue, newvalue };
        }

        public EffectResult Stop(List<Effect> effects, L2Character caster)
        {
            EffectResult result = new EffectResult();

            foreach (EffectResult ter in effects.Select(effect => effect.OnEnd(caster, _owner)))
            {
                if (result.TotalUi == 0)
                {
                    result.TotalUi = ter.TotalUi;
                }

                if (ter.Sus != null)
                {
                    result.AddAll(ter.Sus);
                }
            }

            return result;
        }

        public double[] Stop(Effect effect)
        {
            _activeEffects.Remove(effect);

            double basevalue = GetTemplate(effect.Type);
            double newvalue = basevalue;
            double buffvalue = GetStat(effect.Type);

            List<Effect> arif = null;
            foreach (Effect cc in _activeEffects.Where(cc => cc.Type == effect.Type))
            {
                if (cc.SupMethod.Method <= SupMethod.Sub)
                {
                    if (arif == null)
                    {
                        arif = new List<Effect>();
                    }

                    arif.Add(cc);
                    continue;
                }

                newvalue = CalcSupMethod(newvalue, cc.SupMethod);
            }

            if (arif != null)
            {
                foreach (Effect cc in arif)
                {
                    newvalue = CalcSupMethod(newvalue, cc.SupMethod);
                }
            }

            if (StatBuff.ContainsKey(effect.Type))
            {
                lock (StatBuff)
                {
                    StatBuff.Remove(effect.Type);
                }
            }

            StatBuff.Add(effect.Type, newvalue);

            return new[] { buffvalue, newvalue };
        }

        private double CalcSupMethod(double val, SupMethod sup)
        {
            double retval = 0;
            switch (sup.Method)
            {
                case SupMethod.Mul:
                    retval = val * sup.Value;
                    break;
                case SupMethod.Div:
                    retval = val / sup.Value;
                    break;
                case SupMethod.Add:
                case SupMethod.Sub:
                    retval = val + sup.Value;
                    break;
            }

            return retval;
        }

        public bool CalcDebuffSuccess(Skill skill, L2Character caster)
        {
            int rnd = new Random().Next(0, 100);
            bool success = rnd <= skill.ActivateRate;

            caster.SendMessage(skill.SkillId + " success " + rnd + "% (" + skill.ActivateRate + "% base)");

            return success;
        }

        public void AddTemplate(EffectType type, double value)
        {
            if (!StatTemplate.Contains(type))
            {
                StatTemplate.Add(type, value);
            }
        }

        public void SetTemplate(PcTemplate template)
        {
            AddTemplate(EffectType.PPhysicalAttack, template.BasePAtk);
            AddTemplate(EffectType.PPhysicalDefense, template.BasePDef);
            AddTemplate(EffectType.PMagicalAttack, template.BaseMAtk);
            AddTemplate(EffectType.PMagicalDefense, template.BaseMDef);
            AddTemplate(EffectType.PSpeed, 700); //template.runspd);

            AddTemplate(EffectType.BMaxWeight, 2500000.0);
            AddTemplate(EffectType.BAccuracy, 50);
            AddTemplate(EffectType.BCriticalRate, 100);
            AddTemplate(EffectType.BEvasion, 50);
            //addTemplate(TEffectType.b_breath, template.B);

            AddTemplate(EffectType.BAttackSpd, template.BasePAtkSpd);
            AddTemplate(EffectType.BCastingSpd, 333);

            AddTemplate(EffectType.BMaxHp, template.HpTable[_owner.Level]);
            AddTemplate(EffectType.BRegHp, template.BaseHpReg);
            AddTemplate(EffectType.BMaxMp, template.MpTable[_owner.Level]);
            AddTemplate(EffectType.BRegMp, template.BaseMpReg);
            AddTemplate(EffectType.BMaxCp, template.CpTable[_owner.Level]);
            AddTemplate(EffectType.BRegCp, 50);
        }

        //public void setTemplate(npcs.NpcTemplate template)
        //{
        //    addTemplate(TEffectType.p_physical_attack, template.base_physical_attack);
        //    addTemplate(TEffectType.p_physical_defense, template.base_defend);
        //    addTemplate(TEffectType.p_magical_attack, template.base_magic_attack);
        //    addTemplate(TEffectType.p_magical_defense, template.base_magic_defend);
        //    addTemplate(TEffectType.p_speed, template.RunSpeed);

        //    addTemplate(TEffectType.b_accuracy, 50);
        //    addTemplate(TEffectType.b_critical_rate, template.base_critical);
        //    addTemplate(TEffectType.b_evasion, 50);

        //    addTemplate(TEffectType.b_attack_spd, template.base_attack_speed);
        //    addTemplate(TEffectType.b_casting_spd, template.base_magic_speed);

        //    addTemplate(TEffectType.b_max_hp, template.org_hp);
        //    owner.CurHP = template.org_hp;
        //    addTemplate(TEffectType.b_reg_hp, template.org_hp_regen);
        //    addTemplate(TEffectType.b_max_mp, template.org_mp);
        //    owner.CurMP = template.org_mp;
        //    addTemplate(TEffectType.b_reg_mp, template.org_mp_regen);
        //}
    }
}