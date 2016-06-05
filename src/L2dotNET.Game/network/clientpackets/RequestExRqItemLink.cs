using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.items;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestExRqItemLink : GameServerNetworkRequest
    {
        private int _objectId;

        public RequestExRqItemLink(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            _objectId = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Item item = RqItemManager.getInstance().getItem(_objectId);
            if (item == null)
            {
                player.sendMessage("That item was deleted or modifyed.");
                return;
            }
            else
            {
                player.sendPacket(new ExRpItemLink(item));
            }
        }
    }
}