namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class PremiumStatusUpdate : GameserverPacket
    {
        private readonly string _account;
        private readonly byte _status;
        private readonly long _points;

        public PremiumStatusUpdate(string account, byte status, long points)
        {
            _account = account;
            _status = status;
            _points = points;
        }

        protected internal override void Write()
        {
            WriteByte(0xA4);
            WriteString(_account.ToLower());
            WriteByte(_status);
            WriteLong(_points);
        }
    }
}