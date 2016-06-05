namespace L2dotNET.GameService.network.serverpackets
{
    class PledgeInfo : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly string _name;
        private readonly string _ally;

        public PledgeInfo(int id, string name, string ally)
        {
            _id = id;
            _name = name;
            _ally = ally;
        }

        protected internal override void write()
        {
            writeC(0x89);
            writeD(_id);
            writeS(_name);
            writeS(_ally);
        }
    }
}