namespace L2dotNET.GameService.network.serverpackets
{
    class AutoAttackStop : GameServerNetworkPacket
    {
        private readonly int sId;

        public AutoAttackStop(int sId)
        {
            this.sId = sId;
        }

        protected internal override void write()
        {
            writeC(0x2c);
            writeD(sId);
        }
    }
}