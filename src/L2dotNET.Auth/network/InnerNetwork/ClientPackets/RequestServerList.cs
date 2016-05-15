
using L2dotNET.LoginService.Network.OuterNetwork;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestServerList : ReceiveBasePacket
    {
        public RequestServerList(LoginClient Client, byte[] data)
        {
            base.CreatePacket(Client, data);
        }

        int login1, login2;
        public override void Read()
        {
            login1 = ReadInt();
            login2 = ReadInt();
        }

        public override void Run()
        {
            if (Client.login1 != login1 && Client.login2 != login2)
            {
                Client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCESS_FAILED));
                return;
            }

            Client.Send(ServerList.ToPacket(Client));
        }
    }
}
