namespace L2dotNET.Network.serverpackets
{
    class AutoAttackStop : GameserverPacket
    {
        private readonly int _sId;

        public AutoAttackStop(int sId)
        {
            _sId = sId;
        }

        public override void Write()
        {
            WriteByte(0x2c);
            WriteInt(_sId);
        }
    }
}