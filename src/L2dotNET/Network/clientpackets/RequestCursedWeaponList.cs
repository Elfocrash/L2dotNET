using System;

namespace L2dotNET.Network.clientpackets
{
    class RequestCursedWeaponList : PacketBase
    {
        private readonly GameClient _client;

        public RequestCursedWeaponList(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override void RunImpl()
        {
            //int[] ids = CursedWeapons.GetInstance().GetWeaponIds();

            //_client.SendPacket(new ExCursedWeaponList(ids));
        }
    }
}