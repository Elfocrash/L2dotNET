namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExAskModifyPartyLooting : GameServerNetworkPacket
    {
        private readonly string _leader;
        private readonly byte _mode;

        public ExAskModifyPartyLooting(string leader, byte mode)
        {
            this._leader = leader;
            this._mode = mode;
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xBE);
            WriteS(_leader);
            WriteD(_mode);
        }
    }
}