namespace L2dotNET.Network.clientpackets
{
    class RequestCursedWeaponLocation : PacketBase
    {
        private readonly GameClient _client;

        public RequestCursedWeaponLocation(Packet packet, GameClient client)
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