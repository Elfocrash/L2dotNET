namespace L2dotNET.GameService.Network.Serverpackets
{
    class NetPing : GameServerNetworkPacket
    {
        private readonly int _request;

        public NetPing(int request)
        {
            this._request = request;
        }

        protected internal override void Write()
        {
            WriteC(0xd9);
            WriteD(_request);
        }
    }
}