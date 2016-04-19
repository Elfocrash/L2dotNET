using L2dotNET.Game.managers;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class RequestSentPost : GameServerNetworkRequest
    {
        private int _msgId;
        public RequestSentPost(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            _msgId = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (!player.isInPeace())
            {
                player.sendSystemMessage(3066); //You cannot receive or send mail with attached items in non-peace zone regions.
                player.sendActionFailed();
                return;
            }

            MailMessage mm = MailManager.getInstance().getMessageMy(player.ObjID, _msgId);

            if (mm == null)
            {
                player.sendMessage("Your mail was not found.");
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new ExShowSentPost(mm));
        }
    }
}
