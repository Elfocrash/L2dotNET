using System;
using L2dotNET.Utility;

namespace L2dotNET.Network
{
    /// <summary>
    /// Represents client packet structure.
    /// </summary>
    public struct Packet
    {
        /// <summary>
        /// Default <see cref="Packet"/> overflow value.
        /// </summary>
        private const int m_DefaultOverflowValue = 128;

        /// <summary>
        /// <see cref="Packet"/> buffer.
        /// </summary>
        private byte[] m_Buffer;

        /// <summary>
        /// <see cref="Packet"/> reader / writer offset.
        /// </summary>
        private int m_Offset;

        /// <summary>
        /// Indicates if <see cref="Packet"/> was received or created to be sent.
        /// </summary>
        private readonly bool m_ReceivedPacket;

        /// <summary>
        /// Initializes new instance of <see cref="Packet"/> (received packet).
        /// </summary>
        /// <param name="headerOffset"><see cref="Packet"/> header offset (for opcodes).</param>
        /// <param name="buffer"><see cref="Packet"/> buffer.</param>
        public Packet(int headerOffset, byte[] buffer)
        {
            m_ReceivedPacket = true;
            m_Buffer = buffer;
            m_Offset = headerOffset;
        }

        /// <summary>
        /// Initializes new instance of <see cref="Packet"/> (packet to send).
        /// </summary>
        /// <param name="opcodes">Packet opcodes.</param>
        public Packet(params byte[] opcodes)
        {
            m_ReceivedPacket = false;
            m_Buffer = opcodes;
            m_Offset = opcodes.Length;
        }

        /// <summary>
        /// Writes <see cref="byte"/> value into packet buffer.
        /// </summary>
        /// <param name="v"><see cref="byte"/> value to write.</param>
        public unsafe void WriteByte(byte v)
        {
            ValidateBufferSize(sizeof(byte));

            fixed (byte* buf = m_Buffer)
                *(buf + m_Offset++) = v;
        }

        /// <summary>
        /// Writes array of <see cref="byte"/> values into packet buffer.
        /// </summary>
        /// <param name="v">Array of <see cref="byte"/> values.</param>
        public void WriteByte(params byte[] v)
        {
            WriteBytesArray(v);
        }

        /// <summary>
        /// Writes array of <see cref="byte"/> into packet buffer.
        /// </summary>
        /// <param name="v">Array of <see cref="byte"/> values.</param>
        public unsafe void WriteBytesArray(byte[] v)
        {
            int length = v.Length;

            ValidateBufferSize(length);

            L2Buffer.Copy(v, 0, m_Buffer, m_Offset, length);
            m_Offset += length;
        }

        /// <summary>
        /// Writes <see cref="short"/> value into packet buffer.
        /// </summary>
        /// <param name="v"><see cref="short"/> value.</param>
        public unsafe void WriteShort(short v)
        {
            ValidateBufferSize(sizeof(short));

            fixed (byte* buf = m_Buffer)
                *((short*)(buf + m_Offset)) = v;

            m_Offset += sizeof(short);
        }

        /// <summary>
        /// Writes array of <see cref="short"/> values into packet buffer.
        /// </summary>
        /// <param name="v">Array of <see cref="short"/> values.</param>
        public unsafe void WriteShort(params short[] v)
        {
            int length = v.Length * sizeof(short);

            ValidateBufferSize(length);

            fixed (byte* buf = m_Buffer)
                fixed (short* w = v)
                    L2Buffer.UnsafeCopy(w, length, buf, ref m_Offset);
        }

        /// <summary>
        /// Writes <see cref="int"/> value to packet buffer.
        /// </summary>
        /// <param name="v"><see cref="int"/> value.</param>
        public unsafe void WriteInt(int v)
        {
            ValidateBufferSize(sizeof(int));

            fixed (byte* buf = m_Buffer)
                *(int*)(buf + m_Offset) = v;

            m_Offset += sizeof(int);
        }

        /// <summary>
        /// Writes array of <see cref="int"/> values into packet buffer.
        /// </summary>
        /// <param name="v">Array of <see cref="int"/> values.</param>
        public unsafe void WriteInt(params int[] v)
        {
            int length = v.Length * sizeof(int);

            ValidateBufferSize(Length);

            fixed (byte* buf = m_Buffer)
                fixed (int* w = v)
                    L2Buffer.UnsafeCopy(w, length, buf, ref m_Offset);
        }

        /// <summary>
        /// Writes <see cref="double"/> value into packet buffer.
        /// </summary>
        /// <param name="v"><see cref="double"/> value.</param>
        public unsafe void WriteDouble(double v)
        {
            ValidateBufferSize(sizeof(double));

            fixed (byte* buf = m_Buffer)
                *(double*)(buf + m_Offset) = v;

            m_Offset += sizeof(double);
        }

        /// <summary>
        /// Writes array of <see cref="double"/> values into packet buffer.
        /// </summary>
        /// <param name="v">Array of <see cref="double"/> values.</param>
        public unsafe void WriteDouble(params double[] v)
        {
            int length = v.Length * sizeof(double);

            ValidateBufferSize(length);

            fixed (byte* buf = m_Buffer)
                fixed (double* w = v)
                    L2Buffer.UnsafeCopy(w, length, buf, ref m_Offset);
        }

        /// <summary>
        /// Writes <see cref="long"/> value into packet buffer.
        /// </summary>
        /// <param name="v"><see cref="long"/> value.</param>
        public unsafe void WriteLong(long v)
        {
            ValidateBufferSize(sizeof(long));

            fixed (byte* buf = m_Buffer)
                *((long*)(buf + m_Offset)) = v;

            m_Offset += sizeof(long);
        }

        /// <summary>
        /// Writes array of <see cref="long"/> values into packet buffer.
        /// </summary>
        /// <param name="v">Array of <see cref="long"/> values.</param>
        public unsafe void WriteLong(params long[] v)
        {
            int length = v.Length * sizeof(long);

            ValidateBufferSize(length);

            fixed (byte* buf = m_Buffer)
                fixed (long* w = v)
                    L2Buffer.UnsafeCopy(w, length, buf, ref m_Offset);
        }

        /// <summary>
        /// Writes <see cref="string"/> object into packet buffer.
        /// </summary>
        /// <param name="s"><see cref="string"/> value.</param>
        public unsafe void WriteString(string s)
        {
            s += '\0';
            int length = s.Length * sizeof(char);

            ValidateBufferSize(length);

            fixed (byte* buf = m_Buffer)
                fixed (char* w = s)
                    L2Buffer.UnsafeCopy(w, length, buf, ref m_Offset);
        }

        /// <summary>
        /// Writes array of <see cref="string"/> values to packet buffer.
        /// </summary>
        /// <param name="s">Array of <see cref="string"/> values.</param>
        public unsafe void WriteString(params string[] s)
        {
            string v = string.Empty;

            foreach (string t in s)
                v += t + '\0';

            int length = v.Length * sizeof(char);

            ValidateBufferSize(length);

            fixed (byte* buf = m_Buffer)
                fixed (char* w = v)
                    L2Buffer.UnsafeCopy(w, length, buf, ref m_Offset);
        }

        /// <summary>
        /// Writes <see cref="bool"/> value to packet buffer. (Inner network only)
        /// </summary>
        /// <param name="v"><see cref="bool"/> value.</param>
        public void InternalWriteBool(bool v)
        {
            WriteByte(v ? (byte)0x01 : (byte)0x00);
        }

        /// <summary>
        /// Writes <see cref="DateTime"/> value to packet buffer. (Inner network only)
        /// </summary>
        /// <param name="v"><see cref="DateTime"/> value.</param>
        public void InternalWriteDateTime(DateTime v)
        {
            WriteLong(v.Ticks);
        }

        /// <summary>
        /// Reads <see cref="byte"/> value from packet buffer.
        /// </summary>
        /// <returns><see cref="byte"/> value.</returns>
        public unsafe byte ReadByte()
        {
            fixed (byte* buf = m_Buffer)
                return *(buf + m_Offset++);
        }

        /// <summary>
        /// Reads array of <see cref="byte"/> values from packet buffer.
        /// </summary>
        /// <param name="length">length of array to read.</param>
        /// <returns>Array of <see cref="byte"/> values.</returns>
        public unsafe byte[] ReadBytesArray(int length)
        {
            byte[] dest = new byte[length];

            fixed (byte* buf = m_Buffer, dst = dest)
                L2Buffer.Copy(buf, length, dst, ref m_Offset);
            return dest;
        }

        public byte[] ReadByteArrayAlt(int length)
        {
            byte[] result = new byte[length];
            Array.Copy(this.GetBuffer(), m_Offset, result, 0, length);
            m_Offset += length;
            return result;
        }

        /// <summary>
        /// Reads <see cref="short"/> value from packet buffer.
        /// </summary>
        /// <returns><see cref="short"/> value.</returns>
        public unsafe short ReadShort()
        {
            fixed (byte* buf = m_Buffer)
            {
                short v = *((short*)(buf + m_Offset));
                m_Offset += sizeof(short);
                return v;
            }
        }

        /// <summary>
        /// Reads <see cref="int"/> value from packet buffer.
        /// </summary>
        /// <returns><see cref="int"/> value.</returns>
        public unsafe int ReadInt()
        {
            fixed (byte* buf = m_Buffer)
            {
                int v = *((int*)(buf + m_Offset));
                m_Offset += sizeof(int);
                return v;
            }
        }

        /// <summary>
        /// Reads <see cref="double"/> value from packet buffer.
        /// </summary>
        /// <returns><see cref="double"/> value.</returns>
        public unsafe double ReadDouble()
        {
            fixed (byte* buf = m_Buffer)
            {
                double v = *((double*)(buf + m_Offset));
                m_Offset += sizeof(double);
                return v;
            }
        }

        /// <summary>
        /// Reads <see cref="long"/> value from packet buffer.
        /// </summary>
        /// <returns><see cref="long"/> value.</returns>
        public unsafe long ReadLong()
        {
            fixed (byte* buf = m_Buffer)
            {
                long v = *((long*)(buf + m_Offset));
                m_Offset += sizeof(long);
                return v;
            }
        }

        /// <summary>
        /// Reads <see cref="string"/> value from packet buffer.
        /// </summary>
        /// <returns><see cref="string"/> value.</returns>
        public unsafe string ReadString()
        {
            fixed (byte* buf = m_Buffer)
                return L2Buffer.GetTrimmedString(buf, ref m_Offset, m_Buffer.Length);
        }

        /// <summary>
        /// Reads <see cref="bool"/> value from packet buffer. (Inner network only)
        /// </summary>
        /// <returns><see cref="bool"/> value.</returns>
        public bool InternalReadBool()
        {
            return ReadByte() == 0x01;
        }

        /// <summary>
        /// Reads <see cref="DateTime"/> value from packet buffer. (Inner network only)
        /// </summary>
        /// <returns><see cref="DateTime"/> value.</returns>
        public DateTime InternalReadDateTime()
        {
            return new DateTime(ReadLong());
        }

        /// <summary>
        /// Validates buffer capacity before writing into it.
        /// </summary>
        /// <param name="nextValueLength">length of next bytes sequence to write into buffer.</param>
        private void ValidateBufferSize(int nextValueLength)
        {
            if (m_Offset + nextValueLength > m_Buffer.Length)
                L2Buffer.Extend(ref m_Buffer, nextValueLength + m_DefaultOverflowValue);
        }

        /// <summary>
        /// Resizes <see cref="Packet"/> buffer to it's actual capacity and appends buffer length to the beginning of <see cref="Packet"/> buffer.
        /// </summary>
        /// <param name="headerSize"><see cref="Packet"/> header (opcodes) capacity.</param>
        public unsafe void Prepare(int headerSize)
        {
            m_Offset += headerSize;

            L2Buffer.Extend(ref m_Buffer, headerSize, m_Offset);

            fixed (byte* buf = m_Buffer)
            {
                if (headerSize == sizeof(short))
                    *((short*)(buf)) = (short)m_Offset;
                else
                    *((int*)(buf)) = m_Offset;
            }
        }

        /// <summary>
        /// Returns packet buffer.
        /// </summary>
        /// <returns>Packet buffer.</returns>
        public byte[] GetBuffer()
        {
            return m_Buffer;
        }

        /// <summary>
        /// Returns packet buffer.
        /// </summary>
        /// <param name="skipFirstBytesCount">Amount of first bytes to skip.</param>
        /// <returns>Buffer without provided amount of first bytes.</returns>
        public byte[] GetBuffer(int skipFirstBytesCount)
        {
            return L2Buffer.Copy(m_Buffer, skipFirstBytesCount, new byte[m_Buffer.Length - skipFirstBytesCount], 0, m_Buffer.Length - skipFirstBytesCount);
        }

        /// <summary>
        /// Moves <see cref="Packet"/> offset position.
        /// </summary>
        /// <param name="size">Additional offset length.</param>
        public void MoveOffset(int size)
        {
            m_Offset += size;
        }

        /// <summary>
        /// Gets first packet opcode.
        /// </summary>
        public unsafe byte FirstOpcode
        {
            get
            {
                fixed (byte* buf = m_Buffer)
                    return *(buf);
            }
        }

        /// <summary>
        /// Gets second packet opcode.
        /// </summary>
        public unsafe int SecondOpcode
        {
            get
            {
                fixed (byte* buf = m_Buffer)
                    return *(byte*)(buf + 1);
            }
        }

        /// <summary>
        /// Gets packet capacity.
        /// </summary>
        public int Length
        {
            get { return m_ReceivedPacket ? m_Buffer.Length : m_Offset; }
        }

        /// <summary>
        /// Returns string representation of current packet.
        /// </summary>
        /// <returns>String representation of current packet.</returns>
        public override string ToString()
        {
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendLine("Packet dump:");
            //sb.AppendFormat("1s op: {0}{2}2d op: {1}{2}", FirstOpcode, SecondOpcode, Environment.NewLine);
            //sb.Append(L2Buffer.ToString(m_Buffer));
            //return sb.ToString();
            return L2Buffer.ToString(m_Buffer);
        }
    }
}