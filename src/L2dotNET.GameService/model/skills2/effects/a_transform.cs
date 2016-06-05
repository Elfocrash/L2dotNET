using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class a_transform : TEffect
    {
        private readonly int transformId;

        public a_transform(int transformId)
        {
            this.transformId = transformId;
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            if (target is L2Player)
                TransformManager.getInstance().transformTo(transformId, (L2Player)target, -1);

            return nothing;
        }

        public override TEffectResult onEnd(L2Character caster, L2Character target)
        {
            if (target is L2Player)
                ((L2Player)target).untransform();

            return nothing;
        }

        public override bool canUse(L2Character caster)
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