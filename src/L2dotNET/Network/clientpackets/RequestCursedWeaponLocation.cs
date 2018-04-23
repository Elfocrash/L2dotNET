using System;

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

        public override void RunImpl()
        {
            //Not implemented yet
        }
    }
}