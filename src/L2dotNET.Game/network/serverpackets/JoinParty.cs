
namespace L2dotNET.GameService.network.l2send
{
    class JoinParty : GameServerNetworkPacket
    {
        private int response;
        public JoinParty(int response)
        {
            this.response = response;
        }

        protected internal override void write()
        {
            writeC(0x3a);
            writeD(response);
        }
    }
}
