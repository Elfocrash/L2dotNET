using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
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