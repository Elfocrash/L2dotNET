namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class PremiumStatusUpdate : GameServerNetworkPacket
    {
        private readonly string _account;
        private readonly byte _status;
        private readonly long _points;

        public PremiumStatusUpdate(string account, byte status, long points)
        {
            this._account = account;
            this._status = status;
            this._points = points;
        }

        protected internal override void Write()
        {
            WriteC(0xA4);
            WriteS(_account.ToLower());
            WriteC(_status);
            WriteQ(_points);
        }
    }
}