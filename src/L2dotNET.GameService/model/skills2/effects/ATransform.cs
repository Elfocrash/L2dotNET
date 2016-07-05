using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class ATransform : Effect
    {
        private readonly int _transformId;

        public ATransform(int transformId)
        {
            _transformId = transformId;
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (target is L2Player)
            {
                TransformManager.GetInstance().TransformTo(_transformId, (L2Player)target, -1);
            }

            return Nothing;
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            if (target is L2Player)
            {
                ((L2Player)target).Untransform();
            }

            return Nothing;
        }

        public override bool CanUse(L2Character caster)
        {
            if (!(caster is L2Player))
            {
                return false;
            }

            L2Player player = (L2Player)caster;
            if (player.Summon != null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotPolymorphWhenSummonedServitor);
                return false;
            }

            if (player.IsSittingInProgress() || player.IsSitting())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotTransformWhileSitting);
                player.SendActionFailed();
                return false;
            }

            if (player.IsInWater())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotPolymorphIntoTheDesiredFormInWater);
                player.SendActionFailed();
                return false;
            }

            if (player.MountType > 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotPolymorphWhileRidingPet);
                player.SendActionFailed();
                return false;
            }

            if (player.TransformId != 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.AlreadyPolymorphedCannotPolymorphAgain);
                player.SendActionFailed();
                return false;
            }

            if (player.IsOnShip() || (player.Boat != null))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotPolymorphWhileRidingBoat);
                player.SendActionFailed();
                return false;
            }

            return true;
        }
    }
}