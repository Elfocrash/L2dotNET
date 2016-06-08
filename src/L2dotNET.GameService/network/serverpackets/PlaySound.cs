namespace L2dotNET.GameService.Network.Serverpackets
{
    class PlaySound : GameServerNetworkPacket
    {
        private readonly string _file;
        private readonly int type;
        private const uint x = 0;
        private const uint y = 0;
        private const uint z = 0;

        public PlaySound(string file, bool ogg = false)
        {
            _file = file;

            if (ogg)
                type = 1;
        }

        protected internal override void write()
        {
            writeC(0x9e);
            writeD(type);
            writeS(_file);
            writeD(0);
            writeD(0);
            writeD(x);
            writeD(y);
            writeD(z);
        }
    }
}