namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBrExtraUserInfo : GameServerNetworkPacket
    {
        private readonly int playerId;
        private readonly int value;

        public ExBrExtraUserInfo(int id, int value)
        {
            playerId = id;
            this.value = value;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xcf);
            writeD(playerId);
            writeD(value); // event effect id
            //writeC(0x00);		// Event flag, added only if event is active
        }
    }
}