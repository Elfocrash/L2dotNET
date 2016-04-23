
namespace L2dotNET.Auth.serverpackets
{
    public class Init : SendBasePacket
    {
        public Init(LoginClient Client)
        {
            base.makeme(Client);
        }

        protected internal override void write()
        {
            writeC(0x00);
            writeD(lc.SessionId);
            writeD(0x0000c621); // protocol revision

            writeB(lc.RsaPair._scrambledModulus); // RSA Public Key

            // unk GG related?
            writeD(0x29DD954E);
            writeD(0x77C39CFC);
            writeD(unchecked((int)0x97ADB620));
            writeD(0x07BDE0F7);

            writeB(lc.BlowfishKey); // BlowFish key
            writeC(0x00); // null termination ;)
        }
    }
}
