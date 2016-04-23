using System.Collections.Generic;
using L2dotNET.Game.model.items;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class RequestPostItemList : GameServerNetworkRequest
    {
        public RequestPostItemList(GameClient client, byte[] data)
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

            List<L2Item> list = new List<L2Item>();
            foreach (L2Item item in player.getAllNonQuestItems())
            {
                if (item.Template.is_trade == 0 || item.AugmentationID > 0 || item._isEquipped == 1)
                    continue;

                list.Add(item);
            }

            player.sendPacket(new ExPostItemList(list));
        }
    }
}
