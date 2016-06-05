namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class LoginAuth : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xA1);
            writeH(Config.Config.Instance.serverConfig.Port);
            writeS(Config.Config.Instance.serverConfig.Host);
            writeS("");
            writeS(Config.Config.Instance.serverConfig.AuthCode);
            writeD(0);
            writeH(Config.Config.Instance.serverConfig.MaxPlayers);
            writeC(Config.Config.Instance.serverConfig.IsGmOnly ? 0x01 : 0x00);
            writeC(Config.Config.Instance.serverConfig.IsTestServer ? 0x01 : 0x00);
        }
    }
}