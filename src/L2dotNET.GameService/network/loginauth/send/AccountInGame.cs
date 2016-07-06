namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class AccountInGame : GameServerNetworkPacket
    {
        private readonly string _account;
        private readonly bool _status;

        public AccountInGame(string account, bool status)
        {
            _account = account;
            _status = status;
        }

        protected internal override void Write()
        {
            WriteC(0x03);
            WriteS(_account.ToLower());
            WriteC(_status ? (byte)1 : (byte)0);
        }
    }
}