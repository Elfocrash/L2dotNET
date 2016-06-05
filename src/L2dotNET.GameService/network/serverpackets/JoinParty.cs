namespace L2dotNET.GameService.Network.Serverpackets
{
    class JoinParty : GameServerNetworkPacket
    {
        private readonly int response;

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