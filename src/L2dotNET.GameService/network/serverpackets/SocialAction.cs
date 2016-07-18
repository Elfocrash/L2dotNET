using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SocialAction : GameserverPacket
    {
        private readonly int _social;
        private readonly int _id;

        public SocialAction(int id, int social)
        {
            _social = social;
            _id = id;
        }

        public override void Write()
        {
            WriteByte(0x2d);
            WriteInt(_id);
            WriteInt(_social);
        }
    }
}