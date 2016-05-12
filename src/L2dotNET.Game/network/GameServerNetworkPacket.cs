using System;
using System.IO;
using System.Runtime.Remoting.Contexts;

namespace L2dotNET.GameService.network
{
    [Synchronization]
    public abstract class GameServerNetworkPacket
    {
        private MemoryStream stream = new MemoryStream();

        protected void writeB(byte[] value)
        {
            stream.Write(value, 0, value.Length);
        }

        protected void writeB(byte[] value, int Offset, int Length)
        {
            stream.Write(value, Offset, Length);
        }

        protected void writeD(uint value = 0)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeD(int value = 0)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeD(double value = 0.0)
        {
            writeB(BitConverter.GetBytes((int)value));
        }

        protected void writeH(ushort value = 0)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeH(short value = 0)
        {
            writeB(BitConverter.GetBytes(value));
        }

        protected void writeH(int value = 0)
        {
            writeB(BitConverter.GetBytes((short)value));
        }

        protected void writeC(byte value)
        {
            stream.WriteByte(value);
        }

        protected void writeC(int value)
        {
            stream.WriteByte((byte)value);
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

            stream.WriteByte(0);
            stream.WriteByte(0);
        }

        protected void writeQ(long value)
        {
            writeB(BitConverter.GetBytes(value));
        }

        public byte[] ToByteArray()
        {
            return stream.ToArray();
        }

        public long Length
        {
            get { return stream.Length; }
        }

        protected internal abstract void write();
    }
}
