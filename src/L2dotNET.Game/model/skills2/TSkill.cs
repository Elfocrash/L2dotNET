using System;
using System.Collections.Generic;
using L2dotNET.Game.model.npcs.decor;
using L2dotNET.Game.model.playable;
using L2dotNET.Game.tools;
using L2dotNET.Game.world;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.skills2
{
    public class TSkill
    {
        public int skill_id;
        public int level;
        public TSkillOperational OpType;
        public int magic_level;
        public int cast_range, effective_range;
        public int skill_cool_time;
        public double reuse_delay;
        

        public short is_magic = 0;

        public short activate_rate = -1;
        public short skill_hit_time;

        public TSkillScope affect_scope;
        public TSkillTarget target_type;

        public List<TSkillCond> Conditions;

        public bool Bonus_Overhit = false;
        public bool Bonus_SSBoost = false;

        public List<TEffect> start_effect;
        public List<TEffect> tick_effect;
        public List<TEffect> effects = new List<TEffect>();
        public List<TEffect> pvp_effect;
        public List<TEffect> pve_effect;
        public List<TEffect> end_effect;

        public int abnormal_visual_effect = -1;
        public string abnormal_type = null;
        public int abnormal_time;
        public int abnormal_lv;
        public int debuff;
        public int effect_point;
        public int mp_consume1;
        public int mp_consume2;
        public int hp_consume;

        public int ConsumeItemId;
        public long ConsumeItemCount;

        public byte EnchantEnabled = 0;

        

        /// <summary>
        /// (effect) (cond)* (sup)*
        /// </summary>
        /// <param name="value"></param>
        public void SetEffect_effect(string value)
        {
            if (value.StartsWith("{"))
                return;

            byte order = 0;
            foreach (string str in value.Split(';'))
            {
                TEffectType type = (TEffectType)Enum.Parse(typeof(TEffectType), str.Split(' ')[0]);
                TEffect te = TEffectRegistrator.getInstance().BuildProc(type, str);
                if (te != null)
                {
                    te.type = type;
                    te.HashID = this.HashID();
                    te.Order = order;
                    te.SkillId = skill_id;
                    te.SkillLv = level;
                    effects.Add(te);
                    order++;
                }
               // else
               //     CLogger.error("skill #" + skill_id + " requested unregistered effect " + str);
              ///  order++;
            }
        }

        public void SetOperateCond(string value)
        {
            if (value.StartsWith("{"))
                return;

            foreach (string str in value.Split(';'))
            {
                TSkillCondType type = (TSkillCondType)Enum.Parse(typeof(TSkillCondType), str.Split(' ')[0]);
                TSkillCond cond = TEffectRegistrator.getInstance().BuildCond(type, str);
                if (Conditions == null)
                    Conditions = new List<TSkillCond>();

                Conditions.Add(cond);
            }
        }

        public long HashID()
        {
            return skill_id * 65536 + level;
        }

        public byte isPassive()
        {
            return this.OpType == TSkillOperational.P ? (byte)1 : (byte)0;
        }

        public SortedList<int, L2Object> getAffectedTargets(L2Character actor)
        {
            SortedList<int, L2Object> targets = new SortedList<int, L2Object>();

            switch (affect_scope)
            {
                case TSkillScope.single:
                    {
                        switch (target_type)
                        {
                            case TSkillTarget.self:
                                targets.Add(actor.ObjID, actor);
                                break;

                            case TSkillTarget.friend:
                            case TSkillTarget.enemy:
                            case TSkillTarget.any:
                            case TSkillTarget.target:
                                if (actor.CurrentTarget != null)
                                    targets.Add(actor.CurrentTarget.ObjID, actor.CurrentTarget);
                                break;
                            case TSkillTarget.master:
                                if (actor is L2Summon)
                                    targets.Add(((L2Summon)actor).Owner.ObjID, ((L2Summon)actor).Owner);
                                break;
                            case TSkillTarget.unlockable:
                                {
                                    if (actor.CurrentTarget != null && actor.CurrentTarget is L2Door)
                                        targets.Add(actor.CurrentTarget.ObjID, actor.CurrentTarget);
                                }
                                break;
                        }
                    }
                    break;
                case TSkillScope.party:
                    L2Character[] members = actor.getPartyCharacters();
                    foreach (L2Character member in members)
                    {
                        double dis = Calcs.calculateDistance(actor, member, true);
                        if (dis < cast_range)
                            targets.Add(member.ObjID, member);
                    }
                    break;
            }

            return targets;
        }

        public L2Character getTargetCastId(L2Character actor)
        {
            L2Character target = null;
            switch (affect_scope)
            {
                case TSkillScope.single:
                    switch (target_type)
                    {
                        case TSkillTarget.self:
                            target = actor;
                            break;
                        case TSkillTarget.any:
                        case TSkillTarget.target:
                            if (actor.CurrentTarget != null)
                            {
                                target = actor.CurrentTarget;
                            }
                            break;
                        case TSkillTarget.friend:
                            if (actor.CurrentTarget != null)
                            {
                                target = actor.CurrentTarget;
                            }
                            break;
                        case TSkillTarget.enemy:
                            if (actor.CurrentTarget != null)
                            {
                                target = actor.CurrentTarget;
                            }
                            break;
                        case TSkillTarget.master:
                            if (actor is L2Summon)
                                target = ((L2Summon)actor).Owner;
                            break;
                        case TSkillTarget.unlockable:
                            {
                                if (actor.CurrentTarget != null && actor.CurrentTarget is L2Door)
                                    target = actor.CurrentTarget;
                            }
                            break;
                    }
                    break;
                case TSkillScope.party:
                    switch (target_type)
                    {
                        case TSkillTarget.self:
                            target = actor;
                            break;
                    }
                    break;
            }

            return target;
        }


        public bool ConditionOk(L2Player target)
        {
            if (Conditions == null)
                return true;

            sbyte retcode = -2;
            foreach (TSkillCond cond in Conditions)
                if (!cond.CanUse(target, this))
                {
                    retcode = cond.retcode;
                    break;
                }

            if (retcode == -1)
            {
                //$s1 cannot be used due to unsuitable terms.
                target.sendPacket(new SystemMessage(113).AddSkillName(skill_id, level));
            }

            return retcode == -2;
        }
    }
}
