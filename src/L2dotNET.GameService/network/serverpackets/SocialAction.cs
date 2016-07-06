namespace L2dotNET.GameService.Network.Serverpackets
{
    class SocialAction : GameServerNetworkPacket
    {
        private readonly int _social;
        private readonly int _id;

        public SocialAction(int id, int social)
        {
            _social = social;
            _id = id;
        }

        protected internal override void Write()
        {
            WriteC(0x2d);
            WriteD(_id);
            WriteD(_social);
        }
    }
}