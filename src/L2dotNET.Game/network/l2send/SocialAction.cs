
namespace L2dotNET.Game.network.l2send
{
    class SocialAction : GameServerNetworkPacket
    {
        private int social;
        private int id;
        public SocialAction(int id, int social)
        {
            this.social = social;
            this.id = id;
        }

        protected internal override void write()
        {
            writeC(0x27);
            writeD(id);
            writeD(social);
        }
    }
}
