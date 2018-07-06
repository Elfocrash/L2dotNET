using L2dotNET.Utility;

namespace L2dotNET.Network.loginauth.send
{
    class LoginServPing : GameserverPacket
    {
        private readonly AuthThread _authThread;

        public LoginServPing(AuthThread authThread)
        {
            _authThread = authThread;
            _authThread.RandomPingKey = RandomThreadSafe.Instance.Next();
        }

        public override void Write()
        {
            WriteByte(0xA0);
            WriteInt(_authThread.RandomPingKey);
        }
    }
}