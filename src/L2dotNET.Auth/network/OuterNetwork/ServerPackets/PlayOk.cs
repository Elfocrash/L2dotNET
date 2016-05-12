
namespace L2dotNET.LoginService.Network.OuterNetwork
{
    public class PlayOk : SendBasePacket
    {
        public PlayOk(LoginClient Client)
        {
            base.makeme(Client);
        }

        protected internal override void write()
        {
            writeC(0x07);
            writeD(lc.play1);
            writeD(lc.play1);
        }
    }
}
