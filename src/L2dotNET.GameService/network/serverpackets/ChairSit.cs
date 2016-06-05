namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChairSit : GameServerNetworkPacket
    {
        private readonly int sId;
        private readonly int staticId;

        public ChairSit(int sId, int staticId)
        {
            this.sId = sId;
            this.staticId = staticId;
        }

        protected internal override void write()
        {
            writeC(0xe1);
            writeD(sId);
            writeD(staticId);
        }
    }
}