using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestExRqItemLink : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;

        public RequestExRqItemLink(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _objectId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            L2Item item = RqItemManager.GetInstance().GetItem(_objectId);
            if (item == null)
                player.SendMessage("That item was deleted or modifyed.");
            //else
            //{
            //    player.SendPacket(ExRpItemLink.ToPacket(item));
            //}
        }
    }
}