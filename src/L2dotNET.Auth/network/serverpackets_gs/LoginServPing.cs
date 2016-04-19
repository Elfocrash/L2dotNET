using L2dotNET.Auth.gscommunication;

namespace L2dotNET.Auth.network.serverpackets_gs
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
