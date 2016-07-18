using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowDelete : GameserverPacket
    {
        private readonly int _id;
        private readonly string _name;

        public PartySmallWindowDelete(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public override void Write()
        {
            WriteByte(0x51);
            WriteInt(_id);
            WriteString(_name);
        }
    }
}