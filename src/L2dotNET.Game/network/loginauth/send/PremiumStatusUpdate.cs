
namespace L2dotNET.GameService.network.loginauth.send
{
    class PremiumStatusUpdate : GameServerNetworkPacket
    {
        private readonly string account;
        private readonly byte status;
        private readonly long points;
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
