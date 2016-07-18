using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChairSit : GameserverPacket
    {
        private readonly int _sId;
        private readonly int _staticId;

        public ChairSit(int sId, int staticId)
        {
            _sId = sId;
            _staticId = staticId;
        }

        public override void Write()
        {
            WriteByte(0xe1);
            WriteInt(_sId);
            WriteInt(_staticId);
        }
    }
}