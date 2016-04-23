using L2dotNET.Game.world;

namespace L2dotNET.Game.network.l2recv
{
    class RequestAction : GameServerNetworkRequest
    {
        public RequestAction(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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
            _actionId = readC();   // Action identifier : 0-Simple click, 1-Shift click
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            L2Object obj = null;

            if (ServerID == player.ObjID)
                obj = player;
            else
            {
                if(player.knownObjects.ContainsKey(ServerID))
                    obj = player.knownObjects[ServerID];
            }

            if (obj == null)
            {
                player.sendActionFailed();
                return;
            }

            switch (_actionId)
            {
                case 0:
                    obj.onAction(player);
                    break;
                case 1:
                    obj.onActionShift(player);
                    break;
            }
        }
    }
}
