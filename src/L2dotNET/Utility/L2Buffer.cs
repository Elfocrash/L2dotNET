using System;

namespace L2dotNET.Utility
{
    /// <summary>
    /// Provides some methods for buffers manipulation.
    /// </summary>
    public static class L2Buffer
    {
        /// <summary>
        /// Copies array of <see cref="byte"/> values from source to destination buffer.
        /// </summary>
        /// <param name="source">Source buffer.</param>
        /// <param name="srcOffset">Source buffer offset.</param>
        /// <param name="destination">Destination buffer.</param>
        /// <param name="destOffset">Destination buffer offset.</param>
        /// <param name="size">Count of bytes to copy.</param>
        /// <returns>Destination buffer.</returns>
        public static unsafe byte[] Copy(byte[] source, int srcOffset, byte[] destination, int destOffset, int size)
        {
            fixed (byte* src = source, dst = destination)
            {
                Copy(src, srcOffset, dst, destOffset, size);
            }

            return destination;
        }

        /// <summary>
        /// Copies array of <see cref="byte"/> values from source to destination buffer.
        /// </summary>
        /// <param name="src">Source buffer reference.</param>
        /// <param name="srcOffset">Source buffer offset.</param>
        /// <param name="dst">Destination buffer reference.</param>
        /// <param name="dstOffset">Destination buffer offset.</param>
        /// <param name="size">Amount of bytes to copy from source to destination buffer.</param>
        public static unsafe void Copy(byte* src, int srcOffset, byte* dst, int dstOffset, int size)
        {
            int index = 0;

            src += srcOffset;
            dst += dstOffset;

            while ((size - index) >= sizeof(long))
            {
                *(long*)(dst + index) = *(long*)(src + index);
                index += sizeof(long);
            }

            while ((size - index) >= sizeof(int))
            {
                *(int*)(dst + index) = *(int*)(src + index);
                index += sizeof(int);
            }

            while ((size - index) >= sizeof(short))
            {
                *(short*)(dst + index) = *(short*)(src + index);
                index += sizeof(short);
            }

            while (index < size)
            {
                *(dst + index) = *(src + index++);
            }

            //srcOffset += size;
        }

        /// <summary>
        /// Copies array of <see cref="byte"/> values to destination buffer.
        /// </summary>
        /// <param name="w">Reference to <see cref="byte"/> values array.</param>
        /// <param name="size">Amount of bytes to write into destination buffer.</param>
        /// <param name="dst">Destination <see cref="byte"/>s buffer reference.</param>
        /// <param name="offset">Destination buffer offset.</param>
        public static unsafe void Copy(byte* w, int size, byte* dst, ref int offset)
        {
            int index = 0;

            dst += offset;
            offset += size;

            while ((size - index) >= sizeof(long))
            {
                *(long*)(dst + index) = *(long*)(w + index);
                index += sizeof(long);
            }

            while ((size - index) >= sizeof(int))
            {
                *(int*)(dst + index) = *(int*)(w + index);
                index += sizeof(int);
            }

            while ((size - index) >= sizeof(short))
            {
                *(short*)(dst + index) = *(short*)(w + index);
                index += sizeof(short);
            }

            while (index < size)
            {
                *(dst + index) = *(w + index++);
            }
        }

        /// <summary>
        /// Copies array of <see cref="short"/> values to destination buffer.
        /// </summary>
        /// <param name="w">Reference to <see cref="short"/> values array.</param>
        /// <param name="size">Amount of bytes to write into destination buffer.</param>
        /// <param name="dst">Destination <see cref="byte"/>s buffer reference.</param>
        /// <param name="offset">Destination buffer offset.</param>
        public static unsafe void UnsafeCopy(short* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        /// <summary>
        /// Returns new array of <see cref="short"/> values, copied from the beginning of provided <paramref name="src"/> pointer.
        /// </summary>
        /// <param name="src">Source <see cref="short"/> values array pointer.</param>
        /// <param name="length">Length of array to copy.</param>
        /// <returns>New array of <see cref="short"/> values, copied from the beginning of provided <paramref name="src"/> pointer.</returns>
        public static unsafe short[] SpecialCopy(short* src, int length)
        {
            short[] destiny = new short[length];

            fixed (short* dst = destiny)
            {
                int index = 0;

                while ((length - index) >= (sizeof(long) / sizeof(short)))
                {
                    *(long*)(dst + index) = *(long*)(src + index);
                    index += sizeof(long) / sizeof(short);
                }

                while ((length - index) >= (sizeof(int) / sizeof(short)))
                {
                    *(int*)(dst + index) = *(int*)(src + index);
                    index += sizeof(int) / sizeof(short);
                }

                while ((length - index) > 0)
                {
                    *(dst + index) = *(src + index++);
                }
            }

            return destiny;
        }

        /// <summary>
        /// Copies array of <see cref="int"/> values to destination buffer.
        /// </summary>
        /// <param name="w">Reference to <see cref="int"/> values array.</param>
        /// <param name="size">Amount of bytes to write into destination buffer.</param>
        /// <param name="dst">Destination <see cref="byte"/>s buffer reference.</param>
        /// <param name="offset">Destination buffer offset.</param>
        public static unsafe void UnsafeCopy(int* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        /// <summary>
        /// Copies array of <see cref="double"/> values to destination buffer.
        /// </summary>
        /// <param name="w">Reference to <see cref="double"/> values array.</param>
        /// <param name="size">Amount of bytes to write into destination buffer.</param>
        /// <param name="dst">Destination buffer reference.</param>
        /// <param name="offset">Destination buffer offset.</param>
        public static unsafe void UnsafeCopy(double* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        /// <summary>
        /// Copies array of <see cref="long"/> values to destination buffer.
        /// </summary>
        /// <param name="w">Reference to <see cref="long"/> values array.</param>
        /// <param name="size">Amount of bytes to write into destination buffer.</param>
        /// <param name="dst">Destination buffer reference.</param>
        /// <param name="offset">Destination buffer offset.</param>
        public static unsafe void UnsafeCopy(long* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        /// <summary>
        /// Copies array of <see cref="char"/> values to destination buffer.
        /// </summary>
        /// <param name="w">Reference to <see cref="char"/> values array.</param>
        /// <param name="size">Amount of bytes to write into destination buffer.</param>
        /// <param name="dst">Destination buffer reference.</param>
        /// <param name="offset">Destination buffer offset.</param>
        public static unsafe void UnsafeCopy(char* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        /// <summary>
        /// Cuts some bytes from source buffer.
        /// </summary>
        /// <param name="source">Source buffer.</param>
        /// <param name="startIndex">Source start size.</param>
        /// <param name="size">Count of bytes to cut.</param>
        /// <returns>Result buffer.</returns>
        public static byte[] Cut(byte[] source, int startIndex, int size)
        {
            return Copy(source, startIndex, new byte[size], 0, size);
        }

        /// <summary>
        /// Replaces part of buffer with replacement buffer.
        /// </summary>
        /// <param name="buffer">Destination buffer.</param>
        /// <param name="index">Destination replacement size.</param>
        /// <param name="replacement">Replacement buffer.</param>
        /// <param name="size">Replacement bytes count.</param>
        /// <returns>Destination buffer.</returns>
        public static byte[] Replace(byte[] buffer, int index, byte[] replacement, int size)
        {
            return Copy(replacement, 0, buffer, index, size);
        }

        /// <summary>
        /// Extends buffer.
        /// </summary>
        /// <param name="source">Source buffer.</param>
        /// <param name="sourceIndex">Source buffer start-copy size.</param>
        /// <param name="neededLength">Result buffer capacity.</param>
        /// <returns>Extended buffer.</returns>
        public static byte[] Extend(ref byte[] source, int sourceIndex, int neededLength)
        {
            return source = Copy(source, 0, new byte[neededLength], sourceIndex, neededLength);
        }

        /// <summary>
        /// Extends buffer.
        /// </summary>
        /// <param name="source">Source buffer.</param>
        /// <param name="additionalLength">Additional capacity.</param>
        /// <returns>Extended buffer.</returns>
        public static byte[] Extend(ref byte[] source, int additionalLength)
        {
            return source = Copy(source, 0, new byte[source.Length + additionalLength], 0, source.Length);
        }

        /// <summary>
        /// Gives string representation of <see cref="byte"/> values buffer.
        /// </summary>
        /// <param name="buffer"><see cref="byte"/> values array.</param>
        /// <returns>String representation of <see cref="byte"/> values array.</returns>
        public static string ToString(byte[] buffer)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Buffer dump, length: {0}{1}Index   |---------------------------------------------|  |--------------|{1}", buffer.Length, Environment.NewLine);

            string hex = string.Empty,
                   data = string.Empty;
            int i,
                index = 0;

            while (index < buffer.Length)
            {
                for (i = 0; (i < 16) && ((index + i) < buffer.Length); i++)
                {
                    hex += buffer[index + i].ToString("x2") + " ";

                    if ((buffer[i + index] > 31) && (buffer[i + index] < 127))
                    {
                        data += (char)buffer[i + index];
                    }
                    else
                    {
                        data += ".";
                    }
                }

                while (i < 16)
                {
                    hex += "   ";
                    i++;
                }

                sb.Append($"{index.ToString("X5")}   {hex} {data}{Environment.NewLine}");
                index += 16;
            }

            sb.Append("        |---------------------------------------------|  |--------------|");

            return sb.ToString();
        }

        /// <summary>
        /// Converts <see cref="byte"/> values from one array to <see cref="char"/> values array (<see cref="string"/>).
        /// </summary>
        /// <param name="src"><see cref="byte"/> values array reference.</param>
        /// <param name="srcOffset"><see cref="byte"/> values array offset.</param>
        /// <param name="dst">Destination string.</param>
        /// <param name="bytesCount">Amount of <see cref="byte"/> values to convert to <see cref="char"/> values.</param>
        public static unsafe void GetTrimmedString(byte* src, int srcOffset, ref string dst, int bytesCount)
        {
            bytesCount = srcOffset + bytesCount;

            while ((srcOffset < bytesCount) && (src[srcOffset] != 0))
            {
                dst += (char)src[srcOffset++];
            }
        }

        /// <summary>
        /// Converts <see cref="byte"/> values array to <see cref="char"/> values array (<see cref="string"/>).
        /// </summary>
        /// <param name="src">Source <see cref="byte"/> values buffer.</param>
        /// <param name="srcOffset">Source buffer offset.</param>
        /// <param name="maxLength">Max source buffer position, that method can reach.</param>
        public static unsafe string GetTrimmedString(byte* src, ref int srcOffset, int maxLength)
        {
            string dst = string.Empty;

            while ((src[srcOffset] != 0) && ((srcOffset + sizeof(char)) < maxLength))
            {
                dst += (char)src[srcOffset];
                srcOffset += sizeof(char);
            }

            srcOffset += sizeof(char);

            return dst;
        }

        /// <summary>
        /// Copies array of generic values from one array to an other.
        /// </summary>
        /// <typeparam name="TU">Some generic type.</typeparam>
        /// <param name="source">Source array.</param>
        /// <param name="srcOffset">Source array offset.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="dstOffset">Destination array offset.</param>
        /// <param name="length">Values to copy count.</param>
        /// <returns>Copied array of generic values.</returns>
        public static TU[] Copy<TU>(TU[] source, long srcOffset, TU[] destination, long dstOffset, long length)
        {
            if ((length > (source.Length - srcOffset)) || (length > (destination.Length - dstOffset)))
            {
                throw new InvalidOperationException();
            }

            length += srcOffset;

            while (srcOffset < length)
            {
                destination[dstOffset++] = source[srcOffset++];
            }

            return destination;
        }
    }
}