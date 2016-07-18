using L2dotNET.Network;

namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class PlayerCount : GameserverPacket
    {
        private readonly short _cnt;

        public PlayerCount(short cnt)
        {
            _cnt = cnt;
        }

        public override void Write()
        {
            WriteByte(0xA3);
            WriteShort(_cnt);
        }
    }
}