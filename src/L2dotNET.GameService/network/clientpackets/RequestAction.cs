using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAction : GameServerNetworkRequest
    {
        public RequestAction(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int ServerID;
        private int _X;
        private int _Y;
        private int _Z;
        private int _actionId;

        public override void read()
        {
            ServerID = readD();
            _X = readD();
            _Y = readD();
            _Z = readD();
            _actionId = readC(); // Action identifier : 0-Simple click, 1-Shift click
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            L2Object obj = null;

            if (ServerID == player.ObjId)
                obj = player;
            else
            {
                if (player.KnownObjects.ContainsKey(ServerID))
                    obj = player.KnownObjects[ServerID];
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