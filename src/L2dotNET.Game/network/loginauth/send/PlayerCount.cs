
namespace L2dotNET.Game.network.loginauth.send
{
    class PlayerCount : GameServerNetworkPacket
    {
        private short cnt;
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
