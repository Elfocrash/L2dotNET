using System;
using System.Threading.Tasks;

namespace L2dotNET.Network.clientpackets
{
    class RequestCursedWeaponLocation : PacketBase
    {
        private readonly GameClient _client;

        public RequestCursedWeaponLocation(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.FromResult(1);
            //Not implemented yet
        }
    }
}