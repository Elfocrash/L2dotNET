
namespace L2dotNET.GameService.network.loginauth.send
{
    class LoginAuth : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xA1);
            writeH(Config.Instance.serverConfig.Port);
            writeS(Config.Instance.serverConfig.Host);
            writeS("");
            writeS(Config.Instance.serverConfig.AuthCode);
            writeD(0);
            writeH(Config.Instance.serverConfig.MaxPlayers);
            writeC(Config.Instance.serverConfig.IsGmOnly ? 0x01 : 0x00);
            writeC(Config.Instance.serverConfig.IsTestServer ? 0x01 : 0x00);
        }
    }
}
