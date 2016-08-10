namespace L2dotNET.Network.serverpackets
{
    class AutoAttackStart : GameserverPacket
    {
        private readonly int _sId;

        public AutoAttackStart(int sId)
        {
            _sId = sId;
        }

        public override void Write()
        {
            WriteByte(0x2b);
            WriteInt(_sId);
        }
    }
}