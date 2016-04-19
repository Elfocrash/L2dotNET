
namespace L2dotNET.Auth.network.serverpackets_gs
{
    class ServerLoginFail : SendServerPacket
    {
        private string code;
        public ServerLoginFail(string code)
        {
            this.code = code;
        }

        protected internal override void write()
        {
            writeC(0xA5);
            writeS(code);
        }
    }
}
