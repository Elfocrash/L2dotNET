
namespace L2dotNET.Game.network.l2send
{
    class KeyPacket : GameServerNetworkPacket
    {
        private byte[] key;
        private byte next;
        public KeyPacket(GameClient client, byte n)
        {
            key = client.enableCrypt();
            next = n;
        }

        protected internal override void write()
        {
            writeC(0x00);
            writeC(0x01);
            writeB(key);
            writeD(0x01);
            writeD(0x01);
        }
    }
}
