using L2dotNET.Game.network.l2send;
using L2dotNET.Game.managers;

namespace L2dotNET.Game.network.l2recv
{
    class RequestReceivedPost : GameServerNetworkRequest
    {
        private int MailID;
        public RequestReceivedPost(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            MailID = readD();
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

            MailMessage mm = MailManager.getInstance().getMessageNotmy(player.ObjID, MailID);

            if (mm == null)
            {
                player.sendMessage("Your mail #"+MailID+" was not found.");
                player.sendActionFailed();
                return;
            }

            mm.NotOpend = 0;

            player.sendPacket(new ExShowReceivedPost(mm));
            player.sendPacket(new ExChangePostState(true, MailID, ExChangePostState.Readed));
        }
    }
}
