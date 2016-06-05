using System;

namespace L2dotNET.Utility
{
    /// <summary>
    /// Provides common randomization methods.
    /// </summary>
    public static class L2Random
    {
        /// <summary>
        /// Internal <see cref="Random"/> object.
        /// </summary>
        private static readonly Random m_Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Returns random <see cref="int"/> value.
        /// </summary>
        /// <returns>Random <see cref="int"/> value.</returns>
        public static unsafe int Next()
        {
            return m_Random.Next();
        }

        /// <summary>
        /// Returns random <see cref="int"/> value.
        /// </summary>
        /// <param name="max">Max result value.</param>
        /// <returns>Random <see cref="int"/> value.</returns>
        public static unsafe int Next(int max)
        {
            return m_Random.Next(0, max);
        }

        /// <summary>
        /// Returns randomly generated array of <see cref="byte"/> values.
        /// </summary>
        /// <param name="count">Array length.</param>
        /// <returns>Randomly generated array of <see cref="byte"/> values.</returns>
        public static byte[] NextBytes(int count)
        {
            byte[] buffer = new byte[count];
            return NextBytes(ref buffer);
        }

        /// <summary>
        /// Returns randomly generated array of <see cref="byte"/> values.
        /// </summary>
        /// <param name="buffer">Array of <see cref="byte"/> values to randomize.</param>
        /// <returns>Randomly generated array of <see cref="byte"/> values.</returns>
        public static unsafe byte[] NextBytes(ref byte[] buffer)
        {
            int i = buffer.Length, j = 0, k;

            fixed (byte* buf = buffer)
            {
                while (j <= i - sizeof(int))
                {
                    k = m_Random.Next();
                    *(int*)(buf + j) = *((int*)(&k));
                    j += sizeof(int);
                }

                while (j != i)
                {
                    k = m_Random.Next();
                    *(buf + j) = *(((byte*)&k) + j++ % sizeof(int));
                }
            }

            return buffer;
        }
    }
}