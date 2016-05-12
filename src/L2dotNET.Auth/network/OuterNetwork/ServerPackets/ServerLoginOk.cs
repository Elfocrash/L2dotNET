
namespace L2dotNET.LoginService.Network.OuterNetwork
{
    class ServerLoginOk : SendServerPacket
    {
        protected internal override void write()
        {
            writeC(0xA6);
            writeS("auth complete");
        }
    }
}
