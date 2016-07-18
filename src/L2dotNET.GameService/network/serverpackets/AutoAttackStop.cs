namespace L2dotNET.GameService.Network.Serverpackets
{
    class AutoAttackStop : GameserverPacket
    {
        private readonly int _sId;

        public AutoAttackStop(int sId)
        {
            _sId = sId;
        }

        protected internal override void Write()
        {
            WriteByte(0x2c);
            WriteInt(_sId);
        }
    }
}