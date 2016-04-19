
namespace L2dotNET.Game.network.loginauth.send
{
    class PremiumStatusUpdate : GameServerNetworkPacket
    {
        private string account;
        private byte status;
        private long points;
        public PremiumStatusUpdate(string account, byte status, long points)
        {
            this.account = account;
            this.status = status;
            this.points = points;
        }

        protected internal override void write()
        {
            writeC(0xA4);
            writeS(account.ToLower());
            writeC(status);
            writeQ(points);
        }
    }
}
