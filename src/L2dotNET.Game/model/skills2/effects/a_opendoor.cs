using System;
using L2dotNET.Game.model.npcs.decor;
using L2dotNET.Game.world;

namespace L2dotNET.Game.model.skills2.effects
{
    class a_opendoor : TEffect
    {
        private int level, rate;
        public override void build(string str)
        {
            this.level = Convert.ToInt32(str.Split(' ')[1]);
            this.rate = Convert.ToInt32(str.Split(' ')[2]);
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            if(target is L2Door)
            {
                L2Door door = (L2Door)target;
                if (door.Level <= level)
                {
                    if (new Random().Next(100) < rate)
                    {
                        door.OpenForTime();
                    }
                    else
                        caster.sendSystemMessage(320);//You have failed to unlock the door.
                }
                else
                    caster.sendSystemMessage(320);//You have failed to unlock the door.
            }
            else
            {
                caster.sendSystemMessage(144);//That is an incorrect target.
            }

            return nothing;
        }

        public override bool canUse(world.L2Character caster)
        {
            L2Object target = caster.CurrentTarget;
            if (target is L2Door)
            {
                L2Door door = target as L2Door;
                if (door.Closed == 0)
                {
                    caster.sendSystemMessage(144);//That is an incorrect target.
                    return false;
                }

                if (!door.UnlockSkill)
                {
                    caster.sendSystemMessage(319);//This door cannot be unlocked
                    return false;
                }
            }
            else
            {
                caster.sendSystemMessage(144);//That is an incorrect target.
                return false;
            }

            return true;
        }
    }
}
