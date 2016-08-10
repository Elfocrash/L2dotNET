namespace L2dotNET.Network.serverpackets
{
    class PlaySound : GameserverPacket
    {
        private readonly string _file;
        private readonly int _type;
        private const uint X = 0;
        private const uint Y = 0;
        private const uint Z = 0;

        public PlaySound(string file, bool ogg = false)
        {
            _file = file;

            if (ogg)
                _type = 1;
        }

        public override void Write()
        {
            WriteByte(0x9e);
            WriteInt(_type);
            WriteString(_file);
            WriteInt(0);
            WriteInt(0);
            WriteInt(X);
            WriteInt(Y);
            WriteInt(Z);
        }
    }
}