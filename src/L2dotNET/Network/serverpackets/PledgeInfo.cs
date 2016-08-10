namespace L2dotNET.Network.serverpackets
{
    class PledgeInfo : GameserverPacket
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

        public override void Write()
        {
            WriteByte(0x89);
            WriteInt(_id);
            WriteString(_name);
            WriteString(_ally);
        }
    }
}