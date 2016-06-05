namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class AccountInGame : GameServerNetworkPacket
    {
        private readonly string account;
        private readonly bool status;

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