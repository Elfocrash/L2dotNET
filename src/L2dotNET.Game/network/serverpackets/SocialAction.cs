namespace L2dotNET.GameService.network.l2send
{
    class SocialAction : GameServerNetworkPacket
    {
        private readonly int social;
        private readonly int id;

        public SocialAction(int id, int social)
        {
            this.social = social;
            this.id = id;
        }

        protected internal override void write()
        {
            writeC(0x2d);
            writeD(id);
            writeD(social);
        }
    }
}