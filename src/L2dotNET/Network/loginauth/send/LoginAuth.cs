namespace L2dotNET.Network.loginauth.send
{
    class LoginAuth : GameserverPacket
    {
        private readonly Config.Config _config;

        public LoginAuth(Config.Config config)
        {
            _config = config;
        }

        public override void Write()
        {
            WriteByte(0xA1);
            WriteShort(_config.ServerConfig.Port);
            WriteString(_config.ServerConfig.Host);
            WriteString(string.Empty);
            WriteString(_config.ServerConfig.AuthKey);
            WriteInt(0);
            WriteShort(_config.ServerConfig.MaxPlayers);
            WriteByte(_config.ServerConfig.IsGmOnly ? 0x01 : 0x00);
            WriteByte(_config.ServerConfig.IsTestServer ? 0x01 : 0x00);
        }
    }
}