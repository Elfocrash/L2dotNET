namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowDelete : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly string _name;

        public PartySmallWindowDelete(int id, string name)
        {
            _id = id;
            _name = name;
        }

        protected internal override void Write()
        {
            WriteC(0x51);
            WriteD(_id);
            WriteS(_name);
        }
    }
}