namespace L2dotNET.GameService.Network.Serverpackets
{
    class PlaySound : GameServerNetworkPacket
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
            {
                _type = 1;
            }
        }

        protected internal override void Write()
        {
            WriteC(0x9e);
            WriteD(_type);
            WriteS(_file);
            WriteD(0);
            WriteD(0);
            WriteD(X);
            WriteD(Y);
            WriteD(Z);
        }
    }
}