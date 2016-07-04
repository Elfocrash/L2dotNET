namespace L2dotNET.GameService.Network.Serverpackets
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

        protected internal override void Write()
        {
            WriteC(0x89);
            WriteD(_id);
            WriteS(_name);
            WriteS(_ally);
        }
    }
}