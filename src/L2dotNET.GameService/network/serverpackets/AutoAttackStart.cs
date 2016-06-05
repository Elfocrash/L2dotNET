namespace L2dotNET.GameService.Network.Serverpackets
{
    class AutoAttackStart : GameServerNetworkPacket
    {
        private readonly int sId;

        public AutoAttackStart(int sId)
        {
            this.sId = sId;
        }

        protected internal override void write()
        {
            writeC(0x2b);
            writeD(sId);
        }
    }
}