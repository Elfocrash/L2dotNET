namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class PlayerCount : GameServerNetworkPacket
    {
        private readonly short _cnt;

        public PlayerCount(short cnt)
        {
            _cnt = cnt;
        }

        protected internal override void Write()
        {
            WriteC(0xA3);
            WriteH(_cnt);
        }
    }
}