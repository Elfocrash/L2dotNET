using System;
using System.Linq;
using L2dotNET.model.player;

namespace L2dotNET.model.npcs.cubic.data
{
    /// <summary>
    /// represents smart cubics id 10-14
    /// </summary>
    public class SmartCubic : CubicTemplate
    {
        public override int AiActionTask(L2Player owner)
        {
            int chance = new Random().Next(100);
            int retval;
            byte summ = Skill1Rate;
            if (chance < summ)
                retval = Action1(owner);
            else
            {
                summ += Skill2Rate;
                if (chance < summ)
                    retval = Action2(owner);
                else
                    retval = Action3(owner);
            }

            return retval;
        }

        /// <summary>
        /// cubic main attack
        /// </summary>
        /// <param name="owner"></param>
        private int Action1(L2Player owner)
        {
            //todo target was attacked some time ago with myself
            if (owner.CurrentTarget?.Dead ?? true)
                return 0;

            CallSkill(owner, Skill1, owner.CurrentTarget);
            return 1;
        }

        /// <summary>
        /// cubic self smart attack
        /// </summary>
        /// <param name="owner"></param>
        private int Action2(L2Player owner)
        {
            if (Skill2Target == "heal")
            {
                if (owner.Dead || ((owner.CurHp / owner.MaxHp) > 0.9))
                    return 0;

                CallSkill(owner, Skill2, owner);
                return 1;
            }

            if (owner.CurrentTarget?.Dead ?? true)
                return 0;

            CallSkill(owner, Skill2, owner.CurrentTarget);
            return 1;
        }

        /// <summary>
        /// cubic master support
        /// </summary>
        /// <param name="owner"></param>
        private int Action3(L2Player owner)
        {
            byte n = 0;
            if (owner.Effects.Any(e => e.Skill.Debuff == 1))
                n = 1;

            if (n != 1)
                return 0;

            CallSkill(owner, Skill3, owner);
            return 1;
        }
    }
}