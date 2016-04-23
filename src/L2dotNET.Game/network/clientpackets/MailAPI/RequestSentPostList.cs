using System.Collections.Generic;
using L2dotNET.Game.managers;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class RequestSentPostList : GameServerNetworkRequest
    {
        public RequestSentPostList(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            // nothing
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

            List<MailMessage> list = MailManager.getInstance().getOutbox(player.ObjID);
            player.sendPacket(new ExShowSentPostList(player.ObjID, list));
        }
    }
}
