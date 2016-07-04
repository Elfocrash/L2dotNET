namespace L2dotNET.GameService.Network.Serverpackets
{
    class JoinParty : GameServerNetworkPacket
    {
        private readonly int _response;

        public JoinParty(int response)
        {
            this._response = response;
        }

        protected internal override void Write()
        {
            WriteC(0x3a);
            WriteD(_response);
        }
    }
}