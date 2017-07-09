namespace L2dotNET.Network.serverpackets
{
    class CharCreateFail : GameserverPacket
    {
        private readonly CharCreateFailReason _reason;

        public CharCreateFail(CharCreateFailReason reason)
        {
            _reason = reason;
        }

        public override void Write()
        {
            WriteByte(0x1a);
            WriteInt((int)_reason);
        }
    }
}