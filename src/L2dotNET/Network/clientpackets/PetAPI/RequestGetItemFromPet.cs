using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.PetAPI
{
    class RequestGetItemFromPet : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;
        private int _count;
        private readonly int _equipped;

        public RequestGetItemFromPet(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _objectId = packet.ReadInt();
            _count = packet.ReadInt();
            if (_count < 0)
                _count = 1;
            _equipped = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
            });
        }
    }
}