using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestExRqItemLink : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;

        public RequestExRqItemLink(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _objectId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
            });

            //L2Item item = RqItemManager.GetInstance().GetItem(_objectId);
            //if (item == null)
            //    player.SendMessage("That item was deleted or modifyed.");
            //else
            //{
            //    player.SendPacket(ExRpItemLink.ToPacket(item));
            //}
        }
    }
}