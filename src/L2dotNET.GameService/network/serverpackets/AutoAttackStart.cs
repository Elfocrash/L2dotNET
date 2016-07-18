namespace L2dotNET.GameService.Network.Serverpackets
{
    class AutoAttackStart : GameserverPacket
    {
        private readonly int _sId;

        public AutoAttackStart(int sId)
        {
            _sId = sId;
        }

        protected internal override void Write()
        {
            WriteByte(0x2b);
            WriteInt(_sId);
        }
    }
}