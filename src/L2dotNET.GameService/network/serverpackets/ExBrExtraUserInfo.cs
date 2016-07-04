namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBrExtraUserInfo : GameServerNetworkPacket
    {
        private readonly int _playerId;
        private readonly int _value;

        public ExBrExtraUserInfo(int id, int value)
        {
            _playerId = id;
            this._value = value;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0xcf);
            WriteD(_playerId);
            WriteD(_value); // event effect id
            //writeC(0x00);		// Event flag, added only if event is active
        }
    }
}