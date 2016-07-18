using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExSetPartyLooting : GameserverPacket
    {
        private readonly int _result;
        private readonly int _mode;

        public ExSetPartyLooting(short voteId)
        {
            if (voteId == -1)
                return;

            _result = 1;
            _mode = voteId;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xBF);
            WriteInt(_result);
            WriteInt(_mode);
        }
    }
}