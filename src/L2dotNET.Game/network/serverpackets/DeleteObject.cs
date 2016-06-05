
namespace L2dotNET.GameService.network.l2send
{
    class DeleteObject : GameServerNetworkPacket
    {
        private readonly int _id;
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
