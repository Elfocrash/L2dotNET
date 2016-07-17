using System;
using System.IO;
using System.Runtime.Remoting.Contexts;

namespace L2dotNET.GameService.Network
{
    [Synchronization]
    public abstract class GameServerNetworkPacket
    {
        private readonly MemoryStream _stream = new MemoryStream();

        protected void WriteB(byte[] value)
        {
            _stream.Write(value, 0, value.Length);
        }

        protected void WriteB(byte[] value, int offset, int length)
        {
            _stream.Write(value, offset, length);
        }

        protected void WriteD(uint value = 0)
        {
            WriteB(BitConverter.GetBytes(value));
        }

        protected void WriteD(int value = 0)
        {
            WriteB(BitConverter.GetBytes(value));
        }

        protected void WriteD(double value = 0.0)
        {
            WriteB(BitConverter.GetBytes((int)value));
        }

        protected void WriteH(ushort value = 0)
        {
            WriteB(BitConverter.GetBytes(value));
        }

        protected void WriteH(short value = 0)
        {
            WriteB(BitConverter.GetBytes(value));
        }

        protected void WriteH(int value = 0)
        {
            WriteB(BitConverter.GetBytes((short)value));
        }

        protected void WriteC(byte value)
        {
            _stream.WriteByte(value);
        }

        protected void WriteC(int value)
        {
            _stream.WriteByte((byte)value);
        }

        protected void WriteF(double value)
        {
            WriteB(BitConverter.GetBytes(value));
        }

        protected void WriteS(string value)
        {
            if (value != null)
                WriteB(System.Text.Encoding.Unicode.GetBytes(value));

            _stream.WriteByte(0);
            _stream.WriteByte(0);
        }

        protected void WriteQ(long value)
        {
            WriteB(BitConverter.GetBytes(value));
        }

        public byte[] ToByteArray()
        {
            return _stream.ToArray();
        }

        public long Length => _stream.Length;

        protected internal abstract void Write();
    }
}