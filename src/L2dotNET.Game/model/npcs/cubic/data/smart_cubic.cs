using System;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills;

namespace L2dotNET.GameService.Model.Npcs.Cubic.Data
{
    /// <summary>
    /// represents smart cubics id 10-14
    /// </summary>
    public class smart_cubic : CubicTemplate
    {
        public override int AiActionTask(L2Player owner)
        {
            int chance = new Random().Next(100);
            int retval = 0;
            byte summ = skill1rate;
            if (chance < summ)
                retval = action1(owner);
            else
            {
                summ += skill2rate;
                if (chance < summ)
                    retval = action2(owner);
                else
                    retval = action3(owner);
            }

            return retval;
        }

        /// <summary>
        /// cubic main attack
        /// </summary>
        /// <param name="owner"></param>
        private int action1(L2Player owner)
        {
            if (owner.CurrentTarget != null)
            {
                //todo target was attacked some time ago with myself
                if (owner.CurrentTarget.Dead)
                    return 0;

                CallSkill(owner, skill1, owner.CurrentTarget);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// cubic self smart attack
        /// </summary>
        /// <param name="owner"></param>
        private int action2(L2Player owner)
        {
            if (skill2target == "heal")
            {
                if (owner.Dead || owner.CurHP / owner.MaxHP > 0.9)
                    return 0;
                CallSkill(owner, skill2, owner);
                return 1;
            }
            else
            {
                if (owner.CurrentTarget != null)
                {
                    if (owner.CurrentTarget.Dead)
                        return 0;

                    CallSkill(owner, skill2, owner.CurrentTarget);
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// cubic master support
        /// </summary>
        /// <param name="owner"></param>
        private int action3(L2Player owner)
        {
            byte n = 0;
            foreach (AbnormalEffect e in owner._effects)
                if (e.skill.debuff == 1)
                {
                    n = 1;
                    break;
                }

            if (n == 1)
            {
                CallSkill(owner, skill3, owner);
                return 1;
            }
            else
                return 0;
        }
    }
}