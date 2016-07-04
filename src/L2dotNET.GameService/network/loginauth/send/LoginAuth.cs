namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class LoginAuth : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xA1);
            writeH(Config.Config.Instance.ServerConfig.Port);
            writeS(Config.Config.Instance.ServerConfig.Host);
            writeS("");
            writeS(Config.Config.Instance.ServerConfig.AuthCode);
            writeD(0);
            writeH(Config.Config.Instance.ServerConfig.MaxPlayers);
            writeC(Config.Config.Instance.ServerConfig.IsGmOnly ? 0x01 : 0x00);
            writeC(Config.Config.Instance.ServerConfig.IsTestServer ? 0x01 : 0x00);
        }
    }
}