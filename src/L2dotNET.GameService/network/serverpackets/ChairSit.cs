namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChairSit : GameServerNetworkPacket
    {
        private readonly int _sId;
        private readonly int _staticId;

        public ChairSit(int sId, int staticId)
        {
            this._sId = sId;
            this._staticId = staticId;
        }

        protected internal override void Write()
        {
            WriteC(0xe1);
            WriteD(_sId);
            WriteD(_staticId);
        }
    }
}