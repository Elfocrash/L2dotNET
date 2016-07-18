namespace L2dotNET.GameService.Network.Serverpackets
{
    class Revive : GameserverPacket
    {
        private readonly int _objId;

        public Revive(int objId)
        {
            _objId = objId;
        }

        protected internal override void Write()
        {
            WriteByte(0x07);
            WriteInt(_objId);
        }
    }
}