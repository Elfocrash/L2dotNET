namespace L2dotNET.GameService.Network.Serverpackets
{
    class AutoAttackStart : GameServerNetworkPacket
    {
        private readonly int _sId;

        public AutoAttackStart(int sId)
        {
            this._sId = sId;
        }

        protected internal override void Write()
        {
            WriteC(0x2b);
            WriteD(_sId);
        }
    }
}