using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAutoSoulShot : GameServerNetworkRequest
    {
        private int _itemId;
        private int _type;

        public RequestAutoSoulShot(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _itemId = ReadD();
            _type = ReadD(); //1 - enable
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Item item = player.Inventory.GetItemByItemId(_itemId);
        }
    }
}