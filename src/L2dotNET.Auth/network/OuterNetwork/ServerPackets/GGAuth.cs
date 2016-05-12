
namespace L2dotNET.LoginService.Network.OuterNetwork
{
    public class GGAuth : SendBasePacket
    {
        public GGAuth(LoginClient Client)
        {
            base.makeme(Client);
        }

        protected internal override void write()
        {
            writeC(0x0b);
            writeD(lc.SessionId);
            writeD(0x00);
            writeD(0x00);
            writeD(0x00);
            writeD(0x00);
        }
    }
}
