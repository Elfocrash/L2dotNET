namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class LoginServPing : GameServerNetworkPacket
    {
        public string version;
        private readonly int build;

        public LoginServPing(AuthThread th)
        {
            this.version = th.version;
            this.build = th.build;
        }

        protected internal override void write()
        {
            writeC(0xA0);
            writeS(version);
            writeD(build);
        }
    }
}