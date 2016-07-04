namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class LoginAuth : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0xA1);
            WriteH(Config.Config.Instance.ServerConfig.Port);
            WriteS(Config.Config.Instance.ServerConfig.Host);
            WriteS("");
            WriteS(Config.Config.Instance.ServerConfig.AuthCode);
            WriteD(0);
            WriteH(Config.Config.Instance.ServerConfig.MaxPlayers);
            WriteC(Config.Config.Instance.ServerConfig.IsGmOnly ? 0x01 : 0x00);
            WriteC(Config.Config.Instance.ServerConfig.IsTestServer ? 0x01 : 0x00);
        }
    }
}