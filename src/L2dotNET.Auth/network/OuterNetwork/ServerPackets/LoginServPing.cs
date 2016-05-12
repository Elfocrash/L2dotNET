using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService.Network.OuterNetwork
{
    class LoginServPing : SendServerPacket
    {
        protected internal override void write()
        {
            writeC(0xA1);
            writeS("it's me, im alive. thanks for asking");
        }
    }
}
