using System;
using System.Collections.Generic;
using System.Linq;
using L2dotNET.model.npcs.decor;
using L2dotNET.model.playable;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tools;
using L2dotNET.Utility;
using L2dotNET.world;

namespace L2dotNET.model.skills2
{
    public class Skill
    {
        public int SkillId;
        public int Level;
        public SkillOperational OpType;
        public int MagicLevel;
        public int CastRange,
                   EffectiveRange;
        public int SkillCoolTime;
        public double ReuseDelay;

        public short IsMagic = 0;

        public short ActivateRate = -1;
        public short SkillHitTime;

        public SkillScope AffectScope;
        public SkillTarget TargetType;

        public List<SkillCond> Conditions = new List<SkillCond>();

        public bool BonusOverhit = false;
        public bool BonusSsBoost = false;

        public List<Effect> StartEffect;
        public List<Effect> TickEffect;
        public List<Effect> Effects = new List<Effect>();
        public List<Effect> PvpEffect;
        public List<Effect> PveEffect;
        public List<Effect> EndEffect;

        public int AbnormalVisualEffect = -1;
        public string AbnormalType = null;
        public int AbnormalTime;
        public int AbnormalLv;
        public int Debuff;
        public int EffectPoint;
        public int MpConsume1;
        public int MpConsume2;
        public int HpConsume;

        public int ConsumeItemId;
        public int ConsumeItemCount;

        public byte EnchantEnabled = 0;

        /// <summary>
        /// (effect) (cond)* (sup)*
        /// </summary>
        /// <param name="value"></param>
        public void SetEffect_effect(string value)
        {
            if (value.StartsWithIgnoreCase("{"))
                return;

            byte order = 0;
            foreach (string str in value.Split(';'))
            {
                var tempValue = str.Split(' ')[0];
                var testNewVal = StringHelper.ToTitleCase(tempValue, '_');
                EffectType type = (EffectType)Enum.Parse(typeof(EffectType), testNewVal);
                Effect te = EffectRegistrator.GetInstance().BuildProc(type, str);
                if (te == null)
                    continue;

                te.Type = type;
                te.HashId = HashId();
                te.Order = order;
                te.SkillId = SkillId;
                te.SkillLv = Level;
                Effects.Add(te);
                order++;
                // else
                //     CLogger.error($"skill #{skill_id} requested unregistered effect {str}");
                //  order++;
            }
        }

        public void SetOperateCond(string value)
        {
            if (value.StartsWithIgnoreCase("{"))
                return;

            foreach (string str in value.Split(';'))
            {
                var tempValue = str.Split(' ')[0];
                var newTempValue = StringHelper.ToTitleCase(tempValue, '_');
                SkillCondType type = (SkillCondType)Enum.Parse(typeof(SkillCondType), newTempValue);
                SkillCond cond = EffectRegistrator.GetInstance().BuildCond(type, str);

                Conditions.Add(cond);
            }
        }

        public long HashId()
        {
            return (SkillId * 65536) + Level;
        }

        public byte IsPassive()
        {
            return OpType == SkillOperational.P ? (byte)1 : (byte)0;
        }

        public SortedList<int, L2Object> GetAffectedTargets(L2Character actor)
        {
            SortedList<int, L2Object> targets = new SortedList<int, L2Object>();

            switch (AffectScope)
            {
                case SkillScope.Single:
                {
                    switch (TargetType)
                    {
                        case SkillTarget.Self:
                            targets.Add(actor.ObjId, actor);
                            break;

                        case SkillTarget.Friend:
                        case SkillTarget.Enemy:
                        case SkillTarget.Any:
                        case SkillTarget.Target:
                            if (actor.Target != null)
                                targets.Add(actor.Target.ObjId, actor.Target);
                            break;
                        case SkillTarget.Master:
                            if (actor is L2Summon)
                                targets.Add(((L2Summon)actor).Owner.ObjId, ((L2Summon)actor).Owner);
                            break;
                        case SkillTarget.Unlockable:
                        {
                            if (actor.Target is L2Door)
                                targets.Add(actor.Target.ObjId, actor.Target);
                        }
                            break;
                    }
                }

                    break;
                case SkillScope.Party:
                    L2Character[] members = actor.GetPartyCharacters();
                    targets = members.Where(member => Calcs.CalculateDistance(actor, member, true) < CastRange).ToSortedList(member => member.ObjId, member => (L2Object)member);
                    break;
            }

            return targets;
        }

        public L2Character GetTargetCastId(L2Character actor)
        {
            L2Character target = null;
            switch (AffectScope)
            {
                case SkillScope.Single:
                    switch (TargetType)
                    {
                        case SkillTarget.Self:
                            target = actor;
                            break;
                        case SkillTarget.Any:
                        case SkillTarget.Target:
                            if (actor.Target != null)
                                target = actor.Target;
                            break;
                        case SkillTarget.Friend:
                            if (actor.Target != null)
                                target = actor.Target;
                            break;
                        case SkillTarget.Enemy:
                            if (actor.Target != null)
                                target = actor.Target;
                            break;
                        case SkillTarget.Master:
                            if (actor is L2Summon)
                                target = ((L2Summon)actor).Owner;
                            break;
                        case SkillTarget.Unlockable:
                        {
                            if (actor.Target is L2Door)
                                target = actor.Target;
                        }
                            break;
                    }

                    break;
                case SkillScope.Party:
                    switch (TargetType)
                    {
                        case SkillTarget.Self:
                            target = actor;
                            break;
                    }

                    break;
            }

            return target;
        }

        public bool ConditionOk(L2Player target)
        {
            if (Conditions.Count == 0)
                return true;

            sbyte retcode = -2;

            SkillCond cond = Conditions.FirstOrDefault(c => !c.CanUse(target, this));
            if (cond != null)
                retcode = cond.Retcode;

            if (retcode == -1)
                target.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1CannotBeUsed).AddSkillName(SkillId, Level));

            return retcode == -2;
        }
    }
}