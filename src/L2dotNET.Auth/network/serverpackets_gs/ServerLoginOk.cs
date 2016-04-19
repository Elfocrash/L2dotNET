
namespace L2dotNET.Auth.network.serverpackets_gs
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
