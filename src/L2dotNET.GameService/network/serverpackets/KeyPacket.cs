namespace L2dotNET.GameService.Network.Serverpackets
{
    class KeyPacket : GameServerNetworkPacket
    {
        private readonly byte[] _key;
        private byte _next;

        public KeyPacket(GameClient client, byte n)
        {
            _key = client.EnableCrypt();
            _next = n;
        }

        protected internal override void Write()
        {
            WriteC(0x00);
            WriteC(0x01);
            WriteB(_key);
            WriteD(0x01);
            WriteD(0x01);
        }
    }
}