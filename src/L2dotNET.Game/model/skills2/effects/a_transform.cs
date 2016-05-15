using L2dotNET.GameService.managers;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2.effects
{
    class a_transform : TEffect
    {
        private int transformId;
        public a_transform(int transformId)
        {
            this.transformId = transformId;
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            if (target is L2Player)
                TransformManager.getInstance().transformTo(transformId, (L2Player)target, -1);

            return nothing;
        }

        public override TEffectResult onEnd(world.L2Character caster, world.L2Character target)
        {
            if (target is L2Player)
                ((L2Player)target).untransform();

            return nothing;
        }

        public override bool canUse(world.L2Character caster)
        {
            if (!(caster is L2Player))
                return false;

            L2Player player = (L2Player)caster;
            if (player.Summon != null)
            {   
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_POLYMORPH_WHEN_SUMMONED_SERVITOR);
                return false;
            }

            if (player.isSittingInProgress() || player.isSitting())
            {   
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_TRANSFORM_WHILE_SITTING);
                player.sendActionFailed();
                return false;
            }

            if (player.isInWater())
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_POLYMORPH_INTO_THE_DESIRED_FORM_IN_WATER);
                player.sendActionFailed();
                return false;
            }

            if (player.MountType > 0)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_POLYMORPH_WHILE_RIDING_PET);
                player.sendActionFailed();
                return false;
            }

            if (player.TransformID != 0)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.ALREADY_POLYMORPHED_CANNOT_POLYMORPH_AGAIN);
                player.sendActionFailed();
                return false;
            }

            if (player.isOnShip() || player.Boat != null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_POLYMORPH_WHILE_RIDING_BOAT);
                player.sendActionFailed();
                return false;
            }

            return true;
        }
    }
}
