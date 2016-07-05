using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestExRqItemLink : GameServerNetworkRequest
    {
        private int _objectId;

        public RequestExRqItemLink(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _objectId = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Item item = RqItemManager.GetInstance().GetItem(_objectId);
            if (item == null)
            {
                player.SendMessage("That item was deleted or modifyed.");
            }
            else
            {
                player.SendPacket(new ExRpItemLink(item));
            }
        }
    }
}