using System;
using System.IO;

namespace L2dotNET.Network
{
    public abstract class GameserverPacket
    {
        private readonly MemoryStream _stream = new MemoryStream();

        protected void WriteBytesArray(byte[] value)
        {
            _stream.Write(value, 0, value.Length);
        }

        protected void WriteBytesArray(byte[] value, int offset, int length)
        {
            _stream.Write(value, offset, length);
        }

        protected void WriteInt(uint value = 0)
        {
            WriteBytesArray(BitConverter.GetBytes(value));
        }

        protected void WriteInt(int value = 0)
        {
            WriteBytesArray(BitConverter.GetBytes(value));
        }

        protected void WriteInt(double value = 0.0)
        {
            WriteBytesArray(BitConverter.GetBytes((int)value));
        }

        protected void WriteShort(ushort value = 0)
        {
            WriteBytesArray(BitConverter.GetBytes(value));
        }

        protected void WriteShort(short value = 0)
        {
            WriteBytesArray(BitConverter.GetBytes(value));
        }

        protected void WriteShort(int value = 0)
        {
            WriteBytesArray(BitConverter.GetBytes((short)value));
        }

        protected void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        protected void WriteByte(int value)
        {
            _stream.WriteByte((byte)value);
        }

        protected void WriteDouble(double value)
        {
            WriteBytesArray(BitConverter.GetBytes(value));
        }

        protected void WriteString(string value)
        {
            if (value != null)
                WriteBytesArray(System.Text.Encoding.Unicode.GetBytes(value));

            _stream.WriteByte(0);
            _stream.WriteByte(0);
        }

        protected void WriteLong(long value)
        {
            WriteBytesArray(BitConverter.GetBytes(value));
        }

        public byte[] ToByteArray()
        {
            return _stream.ToArray();
        }

        public long Length => _stream.Length;

        public abstract void Write();
    }
}