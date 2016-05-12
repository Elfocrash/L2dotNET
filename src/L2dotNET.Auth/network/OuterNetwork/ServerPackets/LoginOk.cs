
namespace L2dotNET.LoginService.Network.OuterNetwork
{
    public class LoginOk : SendBasePacket
    {
        public LoginOk(LoginClient Client)
        {
            base.makeme(Client);
        }

        protected internal override void write()
        {
            writeC(0x03);
            writeD(lc.login1);
            writeD(lc.login2);
            writeD(0x00);
            writeD(0x00);
            writeD(0x000003ea);
            writeD(0x00);
            writeD(0x00);
            writeD(0x00);
            writeB(new byte[16]);
        }
    }
}
