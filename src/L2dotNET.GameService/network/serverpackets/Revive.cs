namespace L2dotNET.GameService.Network.Serverpackets
{
    class Revive : GameServerNetworkPacket
    {
        private readonly int _objId;

        public Revive(int objId)
        {
            _objId = objId;
        }

        protected internal override void Write()
        {
            WriteC(0x07);
            WriteD(_objId);
        }
    }
}