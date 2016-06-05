namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class PlayerCount : GameServerNetworkPacket
    {
        private readonly short cnt;

        public PlayerCount(short cnt)
        {
            this.cnt = cnt;
        }

        protected internal override void write()
        {
            writeC(0xA3);
            writeH(cnt);
        }
    }
}