
namespace L2dotNET.LoginService.Network.OuterNetwork
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
