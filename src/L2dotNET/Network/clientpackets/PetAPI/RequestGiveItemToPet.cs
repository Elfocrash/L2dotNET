using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.PetAPI
{
    class RequestGiveItemToPet : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;
        private int _num;

        public RequestGiveItemToPet(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _sId = packet.ReadInt();
            _num = packet.ReadInt();
            if (_num < 0)
                _num = 1;
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