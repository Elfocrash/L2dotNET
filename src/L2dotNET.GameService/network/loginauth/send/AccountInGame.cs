namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class AccountInGame : GameserverPacket
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
            WriteByte(0x03);
            WriteString(_account.ToLower());
            WriteByte(_status ? (byte)1 : (byte)0);
        }
    }
}