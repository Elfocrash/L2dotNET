namespace L2dotNET.GameService.Network.Serverpackets
{
    class DeleteObject : GameServerNetworkPacket
    {
        private readonly int _id;

        public DeleteObject(int id)
        {
            _id = id;
        }

        protected internal override void Write()
        {
            WriteC(0x12);
            WriteD(_id);
            WriteD(0);
        }
    }
}