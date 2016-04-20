
namespace L2dotNET.Game.network.loginauth.send
{
    class AccountInGame : GameServerNetworkPacket
    {
        private string account;
        private bool status;
        public AccountInGame(string account, bool status)
        {
            this.account = account;
            this.status = status;
        }

        protected internal override void write()
        {
            writeC(0x03);
            writeS(account.ToLower());
            writeC(status ? (byte)1 : (byte)0);
        }
    }
}
