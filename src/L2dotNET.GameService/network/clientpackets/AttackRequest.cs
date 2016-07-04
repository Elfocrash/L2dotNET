using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class AttackRequest : GameServerNetworkRequest
    {
        public AttackRequest(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int _objectId;
        private int _originX;
        private int _originY;
        private int _originZ;
        private int _attackId;

        public override void read()
        {
            _objectId = readD();
            _originX = readD();
            _originY = readD();
            _originZ = readD();
            _attackId = readC(); // 0 for simple click   1 for shift-click
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            if (_objectId == player.ObjId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CANNOT_USE_ON_YOURSELF);
                player.SendActionFailed();
                return;
            }

            L2Object obj = player.KnownObjects[_objectId];

            if (obj == null)
            {
                player.SendActionFailed();
                return;
            }

            //if (obj is L2Npc)
            //{
            //    if (((L2Npc)obj).Template._can_be_attacked == 0)
            //    {
            //        player.sendSystemMessage(144);//That is the incorrect target.
            //        player.sendActionFailed();
            //        return;
            //    }
            //}

            obj.OnForcedAttack(player);
        }
    }
}