namespace L2dotNET.Network.loginauth.send
{
    class LoginAuth : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xA1);
            WriteShort(Config.Config.Instance.ServerConfig.Port);
            WriteString(Config.Config.Instance.ServerConfig.Host);
            WriteString(string.Empty);
            WriteString(Config.Config.Instance.ServerConfig.AuthCode);
            WriteInt(0);
            WriteShort(Config.Config.Instance.ServerConfig.MaxPlayers);
            WriteByte(Config.Config.Instance.ServerConfig.IsGmOnly ? 0x01 : 0x00);
            WriteByte(Config.Config.Instance.ServerConfig.IsTestServer ? 0x01 : 0x00);
        }
    }
}