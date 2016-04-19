
namespace L2dotNET.Game.network.loginauth.send
{
    class LoginAuth : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xA1);
            writeH(Cfg.SERVER_PORT);
            writeS(Cfg.SERVER_HOST);
            writeS("");
            writeS(Cfg.auth_code);
            writeD(0);
            writeH(Cfg.max_players);
            writeC(Cfg.gmonly);
            writeC(Cfg.test);
        }
    }
}
