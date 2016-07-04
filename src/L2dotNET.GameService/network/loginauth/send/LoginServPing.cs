namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class LoginServPing : GameServerNetworkPacket
    {
        public string Version;
        private readonly int _build;

        public LoginServPing(AuthThread th)
        {
            Version = th.Version;
            _build = th.Build;
        }

        protected internal override void Write()
        {
            WriteC(0xA0);
            WriteS(Version);
            WriteD(_build);
        }
    }
}