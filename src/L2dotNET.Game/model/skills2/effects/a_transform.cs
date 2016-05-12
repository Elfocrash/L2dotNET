using L2dotNET.GameService.managers;

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
                player.sendSystemMessage(2062);//You cannot polymorph when you have summoned a servitor/pet.
                return false;
            }

            if (player.isSittingInProgress() || player.isSitting())
            {
                player.sendSystemMessage(2283);//You cannot transform while sitting.
                player.sendActionFailed();
                return false;
            }

            if (player.isInWater())
            {
                player.sendSystemMessage(2060);//You cannot polymorph into the desired form in water.
                player.sendActionFailed();
                return false;
            }

            if (player.MountType > 0)
            {
                player.sendSystemMessage(2063);//You cannot polymorph while riding a pet.
                player.sendActionFailed();
                return false;
            }

            if (player.TransformID != 0)
            {
                player.sendSystemMessage(2058);//You already polymorphed and cannot polymorph again.
                player.sendActionFailed();
                return false;
            }

            if (player.isOnShip() || player.Boat != null)
            {
                player.sendSystemMessage(2182);//You cannot polymorph while riding a boat.
                player.sendActionFailed();
                return false;
            }

            return true;
        }
    }
}
