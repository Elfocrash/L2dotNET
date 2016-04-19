
namespace L2dotNET.Game.network.l2send
{
    class ExBrExtraUserInfo : GameServerNetworkPacket
    {
        private int playerId;
        private int value;
        public ExBrExtraUserInfo(int id, int value)
        {
            this.playerId = id;
            this.value = value;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xcf);
            writeD(playerId);
            writeD(value);		// event effect id
            //writeC(0x00);		// Event flag, added only if event is active
        }
    }
}
