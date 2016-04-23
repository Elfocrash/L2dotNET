
namespace L2dotNET.Game.network.l2send
{
    class DeleteObject : GameServerNetworkPacket
    {
        private int _id;
        public DeleteObject(int id)
        {
            _id = id;
        }

        protected internal override void write()
        {
            writeC(0x12);
            writeD(_id);
            writeD(0);
        }
    }
}
