using L2dotNET.model.player;
using L2dotNET.world;
using log4net;
using L2dotNET.Utility;

namespace L2dotNET.Network.clientpackets
{
    class RequestAction : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _actionId;

        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestAction));

        public RequestAction(Packet packet, GameClient client)
        {
            _client = client;
            _objectId = packet.ReadInt();
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
            _actionId = packet.ReadByte(); // Action identifier : 0-Simple click, 1-Shift click
        }

       

        public override void RunImpl()
        {
            
            L2Player player = _client.CurrentPlayer;

            L2Object obj = null;

            if (_objectId == player.ObjId)
                obj = player;
            else
            {
                if (L2World.Instance.GetObject(_objectId) != null)
                    obj = L2World.Instance.GetObject(_objectId);
            }
            //fixed nullreference exception when obj is null
            Log.Debug($"Action Requested with { Utilz.GetTypeLower(obj).ToString() }  of ID : { _objectId.ToString()}");

            if(obj==null)
            {
                Log.Debug("Action Requested Failed");
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