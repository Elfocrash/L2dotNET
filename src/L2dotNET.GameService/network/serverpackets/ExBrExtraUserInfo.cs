using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBrExtraUserInfo : GameserverPacket
    {
        private readonly int _playerId;
        private readonly int _value;

        public ExBrExtraUserInfo(int id, int value)
        {
            _playerId = id;
            _value = value;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0xcf);
            WriteInt(_playerId);
            WriteInt(_value); // event effect id
            //writeC(0x00);		// Event flag, added only if event is active
        }
    }
}