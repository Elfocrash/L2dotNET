using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAction : PacketBase
    {
        private readonly GameClient _client;
        private int _serverId;
        private int _x;
        private int _y;
        private int _z;
        private int _actionId;

        public RequestAction(Packet packet, GameClient client)
        {
            _client = client;
            _serverId = packet.ReadInt();
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
            _actionId = packet.ReadByte(); // Action identifier : 0-Simple click, 1-Shift click
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            L2Object obj = null;

            if (_serverId == player.ObjId)
            {
                obj = player;
            }
            else
            {
                if (player.KnownObjects.ContainsKey(_serverId))
                {
                    obj = player.KnownObjects[_serverId];
                }
            }

            if (obj == null)
            {
                player.SendActionFailed();
                return;
            }

            switch (_actionId)
            {
                case 0:
                    obj.OnAction(player);
                    break;
                case 1:
                    obj.OnActionShift(player);
                    break;
            }
        }
    }
}