using System;
using L2dotNET.GameService.Model.npcs.decor;
using L2dotNET.GameService.network.serverpackets;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.Model.skills2.effects
{
    class a_opendoor : TEffect
    {
        private int level,
                    rate;

        public override void build(string str)
        {
            this.level = Convert.ToInt32(str.Split(' ')[1]);
            this.rate = Convert.ToInt32(str.Split(' ')[2]);
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            if (target is L2Door)
            {
                L2Door door = (L2Door)target;
                if (door.Level <= level)
                {
                    if (new Random().Next(100) < rate)
                    {
                        door.OpenForTime();
                    }
                    else
                        caster.sendSystemMessage(SystemMessage.SystemMessageId.FAILED_TO_UNLOCK_DOOR);
                }
                else
                    caster.sendSystemMessage(SystemMessage.SystemMessageId.FAILED_TO_UNLOCK_DOOR);
            }
            else
            {
                caster.sendSystemMessage(SystemMessage.SystemMessageId.TARGET_IS_INCORRECT);
            }

            return nothing;
        }

        public override bool canUse(L2Character caster)
        {
            L2Object target = caster.CurrentTarget;
            if (target is L2Door)
            {
                L2Door door = target as L2Door;
                if (door.Closed == 0)
                {
                    caster.sendSystemMessage(SystemMessage.SystemMessageId.TARGET_IS_INCORRECT);
                    return false;
                }

                if (!door.UnlockSkill)
                {
                    caster.sendSystemMessage(SystemMessage.SystemMessageId.UNABLE_TO_UNLOCK_DOOR);
                    return false;
                }
            }
            else
            {
                caster.sendSystemMessage(SystemMessage.SystemMessageId.TARGET_IS_INCORRECT);
                return false;
            }

            return true;
        }
    }
}