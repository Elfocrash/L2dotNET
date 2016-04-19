using System;
using System.IO;
using L2dotNET.Auth.gscommunication;

namespace L2dotNET.Auth
{
    public abstract class SendServerPacket
    {
        private MemoryStream mstream;
        public L2Server server;

        public SendServerPacket()
        {
            mstream = new MemoryStream();
        }

        public void makeme(L2Server server)
        {
            this.mstream = new MemoryStream();
            this.server = server;
        }

        protected void writeB(byte[] value)
        {
            mstream.Write(value, 0, value.Length);
        }

        protected void writeB(byte[] value, int Offset, int Length)
        {
            mstream.Write(value, Offset, Length);
        }

        protected void writeD(uint value)
        {
            writeB(BitConverter.GetBytes(value));
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

        protected void writeC(int value)
        {
            mstream.WriteByte((byte)value);
        }

        protected void writeF(double value)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeS(string value)
        {
            if (value != null)
            {
                writeB(System.Text.Encoding.Unicode.GetBytes(value));
            }

            mstream.WriteByte(0);
            mstream.WriteByte(0);
        }

        protected void writeQ(long value)
        {
            writeB(BitConverter.GetBytes(value));
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
