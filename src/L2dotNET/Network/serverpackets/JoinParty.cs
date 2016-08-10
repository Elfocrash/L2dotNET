namespace L2dotNET.Network.serverpackets
{
    class JoinParty : GameserverPacket
    {
        private readonly int _response;

        public JoinParty(int response)
        {
            _response = response;
        }

        public override void Write()
        {
            WriteByte(0x3a);
            WriteInt(_response);
        }
    }
}