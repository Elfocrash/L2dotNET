
namespace L2dotNET.GameService.network.l2send
{
    class PlaySound : GameServerNetworkPacket
    {
        private string _file;
        private int type = 0;
        private uint x;
        private uint y;
        private uint z;
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
