using System;
using System.IO;
using System.Text;

namespace L2dotNET.LoginService
{
    public abstract class SendBasePacket
    {
        private MemoryStream mstream;
        public LoginClient lc;

        public void makeme(LoginClient client)
        {
            mstream = new MemoryStream();
            lc = client;
        }

        protected void writeB(byte[] value)
        {
            mstream.Write(value, 0, value.Length);
        }

        protected void writeD(int value)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeH(short value)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeC(byte value)
        {
            mstream.WriteByte(value);
        }

        protected void writeF(double value)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeQ(long value)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeS(string value)
        {
            if (value != null)
                writeB(Encoding.Unicode.GetBytes(value));

            writeH(0);
        }

        public byte[] ToByteArray()
        {
            return mstream.ToArray();
        }

        public long Length
        {
            get { return mstream.Length; }
        }

        protected internal abstract void write();
    }

}
