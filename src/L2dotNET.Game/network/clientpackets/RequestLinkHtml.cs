using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestLinkHtml : GameServerNetworkRequest
    {
        public RequestLinkHtml(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private string _link;
        public override void read()
        {
            _link = readS();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

           // log.Info($"link to '{ _link }'");

            string file = "";
            int id = 0;
            if (_link.Contains("#"))
            {
                file = _link.Split('#')[0];
                id = int.Parse(_link.Split('#')[1]);
            }
            else
                file = _link;

            int idx = 0;
            if (player.CurrentTarget != null)
                idx = player.CurrentTarget.ObjID;
            else
                idx = player.ObjID;

            player.sendPacket(new NpcHtmlMessage(player, file, idx, id));
        }
    }
}
