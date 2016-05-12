using L2dotNET.Auth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L2dotNET.Auth.Utils
{
    /// <summary>
    /// Login service utilities.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// MD5 crypto service provider.
        /// </summary>
        private static readonly System.Security.Cryptography.MD5CryptoServiceProvider CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();

        /// <summary>
        /// <para>Returns password hash.</para>
        /// <para>Hash compute times count is defined is server settings as PasswordProtectionLevel.</para>
        /// </summary>
        /// <param name="password">Plain text password.</param>
        //internal static string HashPassword(string password)
        //{
        //    for (byte i = 0; i < Settings.Default.LoginServicePasswordsProtectionLevel && i < byte.MaxValue; i++)
        //        password = BitConverter.ToString(CSP.ComputeHash(Encoding.Default.GetBytes(password)));
        //    return password;
        //}

        /// <summary>
        /// User login regular expression.
        /// </summary>
        private static readonly Regex UserLoginRegex = new Regex("[A-Za-z0-9]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        /// <summary>
        /// Validates user login.
        /// </summary>
        /// <param name="login">User login to validate.</param>
        /// <returns>True, if user login matches logins regex, otherwise false.</returns>
        internal static bool IsValidUserLogin(string login)
        {
            return UserLoginRegex.IsMatch(login);
        }
    }
}
namespace L2.Net.LoginService.Crypt
{

    #region Crypt

    public sealed class NewCrypt
    {
        private BlowfishEngine m_Crypt;
        private BlowfishEngine m_Decrypt;

        public NewCrypt(byte[] blowfishKey)
        {
            m_Crypt = new BlowfishEngine();
            m_Crypt.init(true, blowfishKey);
            m_Decrypt = new BlowfishEngine();
            m_Decrypt.init(false, blowfishKey);
        }

        public static bool VerifyChecksum(byte[] raw)
        {
            return NewCrypt.VerifyChecksum(raw, 0, raw.Length);
        }

        public static bool VerifyChecksum(byte[] raw, int offset, int size)
        {
            // check if capacity is multiple of 4 and if there is more then only the checksum
            if ((size & 3) != 0 || size <= 4)
                return false;

            long chksum = 0;
            int count = size - 4;
            long check = -1;
            int i;

            for (i = offset; i < count; i += 4)
            {
                check = raw[i] & 0xff;
                check |= raw[i + 1] << 8 & 0xff00L;
                check |= raw[i + 2] << 0x10 & 0xff0000L;
                check |= raw[i + 3] << 0x18 & 0xff000000;

                chksum ^= check;
            }

            check = raw[i] & 0xff;
            check |= raw[i + 1] << 8 & 0xff00L;
            check |= raw[i + 2] << 0x10 & 0xff0000L;
            check |= raw[i + 3] << 0x18 & 0xff000000;

            return check == chksum;
        }

        public static void AppendChecksum(byte[] raw)
        {
            NewCrypt.AppendChecksum(raw, 0, raw.Length);
        }

        public static void AppendChecksum(byte[] raw, int offset, int size)
        {
            long chksum = 0;
            int count = size - 4;
            long ecx;
            int i;

            for (i = offset; i < count; i += 4)
            {
                ecx = raw[i] & 0xff;
                ecx |= raw[i + 1] << 8 & 0xff00L;
                ecx |= raw[i + 2] << 0x10 & 0xff0000L;
                ecx |= raw[i + 3] << 0x18 & 0xff000000;

                chksum ^= ecx;
            }

            ecx = raw[i] & 0xff;
            ecx |= raw[i + 1] << 8 & 0xff00L;
            ecx |= raw[i + 2] << 0x10 & 0xff0000L;
            ecx |= raw[i + 3] << 0x18 & 0xff000000;

            raw[i] = (byte)(chksum & 0xff);
            raw[i + 1] = (byte)(chksum >> 0x08 & 0xff);
            raw[i + 2] = (byte)(chksum >> 0x10 & 0xff);
            raw[i + 3] = (byte)(chksum >> 0x18 & 0xff);
        }

        public static void AppendChecksum(byte[] raw, out byte[] dest, int offset, int size)
        {
            dest = new byte[raw.Length];

            long chksum = 0;
            int count = size - 4;
            long ecx;
            int i;

            for (i = offset; i < count; i += 4)
            {
                ecx = raw[i] & 0xff;
                ecx |= raw[i + 1] << 8 & 0xff00L;
                ecx |= raw[i + 2] << 0x10 & 0xff0000L;
                ecx |= raw[i + 3] << 0x18 & 0xff000000;

                chksum ^= ecx;
            }

            ecx = raw[i] & 0xff;
            ecx |= raw[i + 1] << 8 & 0xff00L;
            ecx |= raw[i + 2] << 0x10 & 0xff0000L;
            ecx |= raw[i + 3] << 0x18 & 0xff000000;

            raw[i] = (byte)(chksum & 0xff);
            raw[i + 1] = (byte)(chksum >> 0x08 & 0xff);
            raw[i + 2] = (byte)(chksum >> 0x10 & 0xff);
            raw[i + 3] = (byte)(chksum >> 0x18 & 0xff);

            Array.Copy(raw, dest, raw.Length);
        }

        /**
         * Packet is first XOR encoded with <code>key</code>
         * Then, the last 4 bytes are overwritten with the the XOR "key".
         * Thus this assume that there is enough room for the key to fit without overwriting data.
         * @param raw The raw bytes to be encrypted
         * @param key The 4 bytes (int) XOR key
         */
        public static void EncXORPass(byte[] raw, int key)
        {
            NewCrypt.EncXORPass(raw, 0, raw.Length, key);
        }

        public static void EncXORPass(byte[] raw, out byte[] dest, int key, int length)
        {
            dest = new byte[length];
            NewCrypt.EncXORPass(raw, out dest, 0, length, key);
        }

        /**
         * Packet is first XOR encoded with <code>key</code>
         * Then, the last 4 bytes are overwritten with the the XOR "key".
         * Thus this assume that there is enough room for the key to fit without overwriting data.
         * @param raw The raw bytes to be encrypted
         * @param offset The begining of the data to be encrypted
         * @param capacity Length of the data to be encrypted
         * @param key The 4 bytes (int) XOR key
         */
        public static void EncXORPass(byte[] raw, int offset, int size, int key)
        {
            int stop = size - 8;
            int pos = 4 + offset;
            int edx;
            int ecx = key; // Initial xor key


            while (pos < stop)
            {
                //edx = BitConverter.ToInt32(raw[center+4], 0);

                edx = (raw[pos] & 0xFF);
                edx |= (raw[pos + 1] & 0xFF) << 8;
                edx |= (raw[pos + 2] & 0xFF) << 16;
                edx |= (raw[pos + 3] & 0xFF) << 24;

                ecx += edx;

                edx ^= ecx;

                raw[pos++] = (byte)(edx & 0xFF);
                raw[pos++] = (byte)(edx >> 8 & 0xFF);
                raw[pos++] = (byte)(edx >> 16 & 0xFF);
                raw[pos++] = (byte)(edx >> 24 & 0xFF);
            }

            raw[pos++] = (byte)(ecx & 0xFF);
            raw[pos++] = (byte)(ecx >> 8 & 0xFF);
            raw[pos++] = (byte)(ecx >> 16 & 0xFF);
            raw[pos++] = (byte)(ecx >> 24 & 0xFF);
        }

        public static void EncXORPass(byte[] raw, out byte[] dest, int offset, int size, int key)
        {
            int stop = size - 8;
            int pos = 4 + offset;
            int edx;
            int ecx = key; // Initial xor key

            while (pos < stop)
            {
                edx = (raw[pos] & 0xFF);
                edx |= (raw[pos + 1] & 0xFF) << 8;
                edx |= (raw[pos + 2] & 0xFF) << 16;
                edx |= (raw[pos + 3] & 0xFF) << 24;

                ecx += edx;

                edx ^= ecx;

                raw[pos++] = (byte)(edx & 0xFF);
                raw[pos++] = (byte)(edx >> 8 & 0xFF);
                raw[pos++] = (byte)(edx >> 16 & 0xFF);
                raw[pos++] = (byte)(edx >> 24 & 0xFF);
            }

            raw[pos++] = (byte)(ecx & 0xFF);
            raw[pos++] = (byte)(ecx >> 8 & 0xFF);
            raw[pos++] = (byte)(ecx >> 16 & 0xFF);
            raw[pos++] = (byte)(ecx >> 24 & 0xFF);

            dest = raw;
        }

        public byte[] Decrypt(byte[] raw)
        {
            byte[] result = new byte[raw.Length];
            int count = raw.Length / 8;

            for (int i = 0; i < count; i++)
                m_Decrypt.processBlock(raw, i * 8, result, i * 8);

            return result;
        }

        public void Decrypt(byte[] raw, int offset, int size)
        {
            byte[] result = new byte[size];
            int count = size / 8;

            for (int i = 0; i < count; i++)
                m_Decrypt.processBlock(raw, offset + i * 8, result, i * 8);

            Buffer.BlockCopy(result, 0, raw, offset, size);
        }

        public byte[] Encrypt(byte[] raw)
        {
            int count = raw.Length / 8;
            byte[] result = new byte[raw.Length];

            for (int i = 0; i < count; i++)
                m_Crypt.processBlock(raw, i * 8, result, i * 8);

            return result;
        }

        public void Encrypt(byte[] raw, int offset, int size)
        {
            int count = size / 8;
            byte[] result = new byte[size];

            for (int i = 0; i < count; i++)
                m_Crypt.processBlock(raw, offset + i * 8, result, i * 8);

            Buffer.BlockCopy(result, 0, raw, offset, size);
        }
    }

    public sealed class LoginCrypt
    {
        private static byte[] STATIC_BLOWFISH_KEY =
            {
                (byte) 0x6b, (byte) 0x60, (byte) 0xcb, (byte) 0x5b,
                (byte) 0x82, (byte) 0xce, (byte) 0x90, (byte) 0xb1,
                (byte) 0xcc, (byte) 0x2b, (byte) 0x6c, (byte) 0x55,
                (byte) 0x6c, (byte) 0x6c, (byte) 0x6c, (byte) 0x6c
            };

        private NewCrypt m_StaticCrypt = new NewCrypt(STATIC_BLOWFISH_KEY);
        private NewCrypt m_Crypt;
        private bool m_Static = true;

        public void setKey(byte[] key)
        {
            m_Crypt = new NewCrypt(key);
        }

        public bool decrypt(byte[] raw, int offset, int size)
        {
            m_Crypt.Decrypt(raw, offset, size);
            return NewCrypt.VerifyChecksum(raw, offset, size);
        }

        public void decrypt(byte[] raw, int rawoffset, out byte[] dest, int destoffset, int length) // throws IOException
        {
            dest = new byte[length];
            Buffer.BlockCopy(raw, rawoffset, dest, destoffset, length);
            m_Crypt.Decrypt(dest);
        }

        public long checkSum(byte[] raw, int offset, int length)
        {
            long chksum = 0;

            for (int i = 0; i < length; i += 4)
            {
                long ecx = raw[offset++] & 0xff;
                ecx |= ((long)raw[offset++]) << 8 & 0xff00;
                ecx |= ((long)raw[offset++]) << 0x10 & 0xff0000;
                ecx |= ((long)raw[offset++]) << 0x18 & 0xff000000;

                chksum ^= ecx;
            }
            return chksum;
        }


        public int encrypt(byte[] raw, int offset, int size)
        {
            size += 4;

            if (m_Static)
            {
                size += 4;
                size += 8 - size % 8;
                NewCrypt.EncXORPass(raw, offset, size, L2Random.Next(16));
                m_StaticCrypt.Encrypt(raw, offset, size);

                m_Static = false;
            }
            else
            {
                size += 8 - size % 8;
                NewCrypt.AppendChecksum(raw, offset, size);
                m_Crypt.Encrypt(raw, offset, size);
            }
            return size;
        }

        public void encrypt(byte[] row, int offset, out byte[] dest, int destoffset, int length)
        {
            length += 4;
            dest = new byte[length];

            Buffer.BlockCopy(row, offset, dest, destoffset, row.Length);

            if (m_Static)
            {
                length += 4;
                length += 8 - length % 8;
                dest = new byte[length];
                NewCrypt.EncXORPass(row, out dest, length, L2Random.Next(16));
                dest = m_StaticCrypt.Encrypt(dest);
                m_Static = false;
            }
            else
            {
                length += 8 - length % 8;
                NewCrypt.AppendChecksum(row, out dest, 0, length);
                dest = m_Crypt.Encrypt(dest);
            }
        }
    }

    public sealed class ScrambledKeyPair
    {
        private static RSAManaged m_RSAManaged;
        private static byte[] m_ScrambledModulus;

        public ScrambledKeyPair(ref RSAParameters pivateKey, ref RSAParameters publicKey)
        {
            m_RSAManaged = new RSAManaged(1024);
            pivateKey = m_RSAManaged.ExportParameters(true);
            publicKey = m_RSAManaged.ExportParameters(false);
            m_ScrambledModulus = ScrambleModulus(publicKey.Modulus);
        }

        private byte[] ScrambleModulus(byte[] modulus)
        {
            byte[] scrambledMod = modulus;

            if (scrambledMod.Length == 0x81 && scrambledMod[0] == 0x00)
            {
                byte[] temp = new byte[0x80];
                Buffer.BlockCopy(scrambledMod, 1, temp, 0, 0x80);
                scrambledMod = temp;
            }
            // step 1 : 0x4d-0x50 <-> 0x00-0x04
            for (int i = 0; i < 4; i++)
            {
                byte temp = scrambledMod[0x00 + i];
                scrambledMod[0x00 + i] = scrambledMod[0x4d + i];
                scrambledMod[0x4d + i] = temp;
            }
            // step 2 : xor first 0x40 bytes with  last 0x40 bytes
            for (int i = 0; i < 0x40; i++)
            {
                scrambledMod[i] = (byte)(scrambledMod[i] ^ scrambledMod[0x40 + i]);
            }
            // step 3 : xor bytes 0x0d-0x10 with bytes 0x34-0x38
            for (int i = 0; i < 4; i++)
            {
                scrambledMod[0x0d + i] = (byte)(scrambledMod[0x0d + i] ^ scrambledMod[0x34 + i]);
            }
            // step 4 : xor last 0x40 bytes with  first 0x40 bytes
            for (int i = 0; i < 0x40; i++)
            {
                scrambledMod[0x40 + i] = (byte)(scrambledMod[0x40 + i] ^ scrambledMod[i]);
            }
            return scrambledMod;
        }

        public byte[] ScrambledModulus
        {
            get
            {
                return m_ScrambledModulus;
            }
        }
    }

    #endregion

    #region RSA

#if INSIDE_CORLIB
    internal
#else
    /// <summary>
    /// Very Big Integer
    /// </summary>
    public
#endif
 class BigInteger
    {

        #region Data Storage

        /// <summary>
        /// The Length of this BigInteger
        /// </summary>
        uint length = 1;

        /// <summary>
        /// The data for this BigInteger
        /// </summary>
        uint[] data;

        #endregion

        #region Constants

        /// <summary>
        /// Default capacity of a BigInteger in bytes
        /// </summary>
        const uint DEFAULT_LEN = 20;

        /// <summary>
        ///        Table of primes below 2000.
        /// </summary>
        /// <remarks>
        ///        <para>
        ///        This table was generated using Mathematica 4.1 using the following function:
        ///        </para>
        ///        <para>
        ///            <code>
        ///            PrimeTable [x_] := Prime [Range [1, PrimePi [x]]]
        ///            PrimeTable [6000]
        ///            </code>
        ///        </para>
        /// </remarks>
        internal static readonly uint[] smallPrimes = {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
            73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151,
            157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233,
            239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317,
            331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419,
            421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503,
            509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607,
            613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701,
            709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811,
            821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911,
            919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997,

            1009, 1013, 1019, 1021, 1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087,
            1091, 1093, 1097, 1103, 1109, 1117, 1123, 1129, 1151, 1153, 1163, 1171, 1181,
            1187, 1193, 1201, 1213, 1217, 1223, 1229, 1231, 1237, 1249, 1259, 1277, 1279,
            1283, 1289, 1291, 1297, 1301, 1303, 1307, 1319, 1321, 1327, 1361, 1367, 1373,
            1381, 1399, 1409, 1423, 1427, 1429, 1433, 1439, 1447, 1451, 1453, 1459, 1471,
            1481, 1483, 1487, 1489, 1493, 1499, 1511, 1523, 1531, 1543, 1549, 1553, 1559,
            1567, 1571, 1579, 1583, 1597, 1601, 1607, 1609, 1613, 1619, 1621, 1627, 1637,
            1657, 1663, 1667, 1669, 1693, 1697, 1699, 1709, 1721, 1723, 1733, 1741, 1747,
            1753, 1759, 1777, 1783, 1787, 1789, 1801, 1811, 1823, 1831, 1847, 1861, 1867,
            1871, 1873, 1877, 1879, 1889, 1901, 1907, 1913, 1931, 1933, 1949, 1951, 1973,
            1979, 1987, 1993, 1997, 1999,

            2003, 2011, 2017, 2027, 2029, 2039, 2053, 2063, 2069, 2081, 2083, 2087, 2089,
            2099, 2111, 2113, 2129, 2131, 2137, 2141, 2143, 2153, 2161, 2179, 2203, 2207,
            2213, 2221, 2237, 2239, 2243, 2251, 2267, 2269, 2273, 2281, 2287, 2293, 2297,
            2309, 2311, 2333, 2339, 2341, 2347, 2351, 2357, 2371, 2377, 2381, 2383, 2389,
            2393, 2399, 2411, 2417, 2423, 2437, 2441, 2447, 2459, 2467, 2473, 2477, 2503,
            2521, 2531, 2539, 2543, 2549, 2551, 2557, 2579, 2591, 2593, 2609, 2617, 2621,
            2633, 2647, 2657, 2659, 2663, 2671, 2677, 2683, 2687, 2689, 2693, 2699, 2707,
            2711, 2713, 2719, 2729, 2731, 2741, 2749, 2753, 2767, 2777, 2789, 2791, 2797,
            2801, 2803, 2819, 2833, 2837, 2843, 2851, 2857, 2861, 2879, 2887, 2897, 2903,
            2909, 2917, 2927, 2939, 2953, 2957, 2963, 2969, 2971, 2999,

            3001, 3011, 3019, 3023, 3037, 3041, 3049, 3061, 3067, 3079, 3083, 3089, 3109,
            3119, 3121, 3137, 3163, 3167, 3169, 3181, 3187, 3191, 3203, 3209, 3217, 3221,
            3229, 3251, 3253, 3257, 3259, 3271, 3299, 3301, 3307, 3313, 3319, 3323, 3329,
            3331, 3343, 3347, 3359, 3361, 3371, 3373, 3389, 3391, 3407, 3413, 3433, 3449,
            3457, 3461, 3463, 3467, 3469, 3491, 3499, 3511, 3517, 3527, 3529, 3533, 3539,
            3541, 3547, 3557, 3559, 3571, 3581, 3583, 3593, 3607, 3613, 3617, 3623, 3631,
            3637, 3643, 3659, 3671, 3673, 3677, 3691, 3697, 3701, 3709, 3719, 3727, 3733,
            3739, 3761, 3767, 3769, 3779, 3793, 3797, 3803, 3821, 3823, 3833, 3847, 3851,
            3853, 3863, 3877, 3881, 3889, 3907, 3911, 3917, 3919, 3923, 3929, 3931, 3943,
            3947, 3967, 3989,

            4001, 4003, 4007, 4013, 4019, 4021, 4027, 4049, 4051, 4057, 4073, 4079, 4091,
            4093, 4099, 4111, 4127, 4129, 4133, 4139, 4153, 4157, 4159, 4177, 4201, 4211,
            4217, 4219, 4229, 4231, 4241, 4243, 4253, 4259, 4261, 4271, 4273, 4283, 4289,
            4297, 4327, 4337, 4339, 4349, 4357, 4363, 4373, 4391, 4397, 4409, 4421, 4423,
            4441, 4447, 4451, 4457, 4463, 4481, 4483, 4493, 4507, 4513, 4517, 4519, 4523,
            4547, 4549, 4561, 4567, 4583, 4591, 4597, 4603, 4621, 4637, 4639, 4643, 4649,
            4651, 4657, 4663, 4673, 4679, 4691, 4703, 4721, 4723, 4729, 4733, 4751, 4759,
            4783, 4787, 4789, 4793, 4799, 4801, 4813, 4817, 4831, 4861, 4871, 4877, 4889,
            4903, 4909, 4919, 4931, 4933, 4937, 4943, 4951, 4957, 4967, 4969, 4973, 4987,
            4993, 4999,

            5003, 5009, 5011, 5021, 5023, 5039, 5051, 5059, 5077, 5081, 5087, 5099, 5101,
            5107, 5113, 5119, 5147, 5153, 5167, 5171, 5179, 5189, 5197, 5209, 5227, 5231,
            5233, 5237, 5261, 5273, 5279, 5281, 5297, 5303, 5309, 5323, 5333, 5347, 5351,
            5381, 5387, 5393, 5399, 5407, 5413, 5417, 5419, 5431, 5437, 5441, 5443, 5449,
            5471, 5477, 5479, 5483, 5501, 5503, 5507, 5519, 5521, 5527, 5531, 5557, 5563,
            5569, 5573, 5581, 5591, 5623, 5639, 5641, 5647, 5651, 5653, 5657, 5659, 5669,
            5683, 5689, 5693, 5701, 5711, 5717, 5737, 5741, 5743, 5749, 5779, 5783, 5791,
            5801, 5807, 5813, 5821, 5827, 5839, 5843, 5849, 5851, 5857, 5861, 5867, 5869,
            5879, 5881, 5897, 5903, 5923, 5927, 5939, 5953, 5981, 5987
        };

        public enum Sign : int
        {
            Negative = -1,
            Zero = 0,
            Positive = 1
        };

        #region Exception Messages
        const string WouldReturnNegVal = "Operation would return a negative value";
        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Create new instance of BigInteger
        /// </summary>
        public BigInteger()
        {
            data = new uint[DEFAULT_LEN];
            this.length = DEFAULT_LEN;
        }

#if !INSIDE_CORLIB
        /// <summary>
        /// Create new instance of BihInteger
        /// </summary>
        /// <param name="sign">Sign</param>
        /// <param name="len">Lenght</param>
        //        [CLSCompliant (false)]
#endif
        public BigInteger(Sign sign, uint len)
        {
            this.data = new uint[len];
            this.length = len;
        }

        /// <summary>
        /// Create new instance of BihInteger
        /// </summary>
        /// <param name="bi"></param>
        public BigInteger(BigInteger bi)
        {
            this.data = (uint[])bi.data.Clone();
            this.length = bi.length;
        }

#if !INSIDE_CORLIB
        //        [CLSCompliant (false)]
#endif
        /// <summary>
        /// Create new instance of BihInteger
        /// </summary>
        /// <param name="bi"></param>
        /// <param name="len"></param>
        public BigInteger(BigInteger bi, uint len)
        {

            this.data = new uint[len];

            for (uint i = 0; i < bi.length; i++)
                this.data[i] = bi.data[i];

            this.length = bi.length;
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Create new instance of BihInteger
        /// </summary>
        /// <param name="inData"></param>
        public BigInteger(byte[] inData)
        {
            length = (uint)inData.Length >> 2;
            int leftOver = inData.Length & 0x3;

            // capacity not multiples of 4
            if (leftOver != 0)
                length++;

            data = new uint[length];

            for (int i = inData.Length - 1, j = 0; i >= 3; i -= 4, j++)
            {
                data[j] = (uint)(
                    (inData[i - 3] << (3 * 8)) |
                    (inData[i - 2] << (2 * 8)) |
                    (inData[i - 1] << (1 * 8)) |
                    (inData[i])
                    );
            }

            switch (leftOver)
            {
                case 1:
                    data[length - 1] = (uint)inData[0];
                    break;
                case 2:
                    data[length - 1] = (uint)((inData[0] << 8) | inData[1]);
                    break;
                case 3:
                    data[length - 1] = (uint)((inData[0] << 16) | (inData[1] << 8) | inData[2]);
                    break;
            }

            this.Normalize();
        }

#if !INSIDE_CORLIB
        //        [CLSCompliant (false)]
#endif
        /// <summary>
        /// Create new instance of BihInteger
        /// </summary>
        /// <param name="inData"></param>
        public BigInteger(uint[] inData)
        {
            length = (uint)inData.Length;

            data = new uint[length];

            for (int i = (int)length - 1, j = 0; i >= 0; i--, j++)
                data[j] = inData[i];

            this.Normalize();
        }

#if !INSIDE_CORLIB
        //        [CLSCompliant (false)]
#endif
        /// <summary>
        /// Create new instance of BihInteger
        /// </summary>
        /// <param name="ui"></param>
        public BigInteger(uint ui)
        {
            data = new uint[] { ui };
        }

#if !INSIDE_CORLIB
        //        [CLSCompliant (false)]
#endif
        /// <summary>
        /// Create new instance of BihInteger
        /// </summary>
        /// <param name="ul"></param>
        public BigInteger(ulong ul)
        {
            data = new uint[2] { (uint)ul, (uint)(ul >> 32) };
            length = 2;

            this.Normalize();
        }

#if !INSIDE_CORLIB
        //        [CLSCompliant (false)]
#endif
        /// <summary>
        /// Operator overriding
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public static implicit operator BigInteger(uint value)
        {
            return (new BigInteger(value));
        }


        public static implicit operator BigInteger(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value");
            return (new BigInteger((uint)value));
        }


        public static implicit operator BigInteger(ulong value)
        {
            return (new BigInteger(value));
        }

        /* This is the BigInteger.Parse method I use. This method works
        because BigInteger.ToString returns the input I gave to Parse. */
        public static BigInteger Parse(string number)
        {
            if (number == null)
                throw new ArgumentNullException("number");

            int i = 0, len = number.Length;
            char c;
            bool digits_seen = false;
            BigInteger val = new BigInteger(0);
            if (number[i] == '+')
            {
                i++;
            }
            else if (number[i] == '-')
            {
                throw new FormatException(WouldReturnNegVal);
            }

            for (; i < len; i++)
            {
                c = number[i];
                if (c == '\0')
                {
                    i = len;
                    continue;
                }
                if (c >= '0' && c <= '9')
                {
                    val = val * 10 + (c - '0');
                    digits_seen = true;
                }
                else
                {
                    if (Char.IsWhiteSpace(c))
                    {
                        for (i++; i < len; i++)
                        {
                            if (!Char.IsWhiteSpace(number[i]))
                                throw new FormatException();
                        }
                        break;
                    }
                    else
                        throw new FormatException();
                }
            }
            if (!digits_seen)
                throw new FormatException();
            return val;
        }

        #endregion

        #region Operators

        public static BigInteger operator +(BigInteger bi1, BigInteger bi2)
        {
            if (bi1 == 0)
                return new BigInteger(bi2);
            else if (bi2 == 0)
                return new BigInteger(bi1);
            else
                return Kernel.AddSameSign(bi1, bi2);
        }

        public static BigInteger operator -(BigInteger bi1, BigInteger bi2)
        {
            if (bi2 == 0)
                return new BigInteger(bi1);

            if (bi1 == 0)
                throw new ArithmeticException(WouldReturnNegVal);

            switch (Kernel.Compare(bi1, bi2))
            {

                case Sign.Zero:
                    return 0;

                case Sign.Positive:
                    return Kernel.Subtract(bi1, bi2);

                case Sign.Negative:
                    throw new ArithmeticException(WouldReturnNegVal);
                default:
                    throw new Exception();
            }
        }

        public static int operator %(BigInteger bi, int i)
        {
            if (i > 0)
                return (int)Kernel.DwordMod(bi, (uint)i);
            else
                return -(int)Kernel.DwordMod(bi, (uint)-i);
        }


        public static uint operator %(BigInteger bi, uint ui)
        {
            return Kernel.DwordMod(bi, (uint)ui);
        }

        public static BigInteger operator %(BigInteger bi1, BigInteger bi2)
        {
            return Kernel.multiByteDivide(bi1, bi2)[1];
        }

        public static BigInteger operator /(BigInteger bi, int i)
        {
            if (i > 0)
                return Kernel.DwordDiv(bi, (uint)i);

            throw new ArithmeticException(WouldReturnNegVal);
        }

        public static BigInteger operator /(BigInteger bi1, BigInteger bi2)
        {
            return Kernel.multiByteDivide(bi1, bi2)[0];
        }

        public static BigInteger operator *(BigInteger bi1, BigInteger bi2)
        {
            if (bi1 == 0 || bi2 == 0)
                return 0;

            //
            // Validate pointers
            //
            if (bi1.data.Length < bi1.length)
                throw new IndexOutOfRangeException("bi1 out of range");
            if (bi2.data.Length < bi2.length)
                throw new IndexOutOfRangeException("bi2 out of range");

            BigInteger ret = new BigInteger(Sign.Positive, bi1.length + bi2.length);

            Kernel.Multiply(bi1.data, 0, bi1.length, bi2.data, 0, bi2.length, ret.data, 0);

            ret.Normalize();
            return ret;
        }

        public static BigInteger operator *(BigInteger bi, int i)
        {
            if (i < 0)
                throw new ArithmeticException(WouldReturnNegVal);
            if (i == 0)
                return 0;
            if (i == 1)
                return new BigInteger(bi);

            return Kernel.MultiplyByDword(bi, (uint)i);
        }

        public static BigInteger operator <<(BigInteger bi1, int shiftVal)
        {
            return Kernel.LeftShift(bi1, shiftVal);
        }

        public static BigInteger operator >>(BigInteger bi1, int shiftVal)
        {
            return Kernel.RightShift(bi1, shiftVal);
        }

        #endregion

        #region Friendly names for operators

        // with names suggested by FxCop 1.30

        public static BigInteger Add(BigInteger bi1, BigInteger bi2)
        {
            return (bi1 + bi2);
        }

        public static BigInteger Subtract(BigInteger bi1, BigInteger bi2)
        {
            return (bi1 - bi2);
        }

        public static int Modulus(BigInteger bi, int i)
        {
            return (bi % i);
        }

#if !INSIDE_CORLIB
        //        [CLSCompliant (false)]
#endif

        public static uint Modulus(BigInteger bi, uint ui)
        {
            return (bi % ui);
        }

        public static BigInteger Modulus(BigInteger bi1, BigInteger bi2)
        {
            return (bi1 % bi2);
        }

        public static BigInteger Divid(BigInteger bi, int i)
        {
            return (bi / i);
        }

        public static BigInteger Divid(BigInteger bi1, BigInteger bi2)
        {
            return (bi1 / bi2);
        }

        public static BigInteger Multiply(BigInteger bi1, BigInteger bi2)
        {
            return (bi1 * bi2);
        }

        public static BigInteger Multiply(BigInteger bi, int i)
        {
            return (bi * i);
        }

        #endregion

        #region Random
        private static RandomNumberGenerator rng;
        private static RandomNumberGenerator Rng
        {
            get
            {
                if (rng == null)
                    rng = RandomNumberGenerator.Create();
                return rng;
            }
        }

        /// <summary>
        /// Generates a new, random BigInteger of the specified capacity.
        /// </summary>
        /// <param name="bits">The number of bits for the new number.</param>
        /// <param name="rng">A random number generator to use to obtain the bits.</param>
        /// <returns>A random number of the specified capacity.</returns>
        public static BigInteger GenerateRandom(int bits, RandomNumberGenerator rng)
        {
            int dwords = bits >> 5;
            int remBits = bits & 0x1F;

            if (remBits != 0)
                dwords++;

            BigInteger ret = new BigInteger(Sign.Positive, (uint)dwords + 1);
            byte[] random = new byte[dwords << 2];

            rng.GetBytes(random);
            Buffer.BlockCopy(random, 0, ret.data, 0, (int)dwords << 2);

            if (remBits != 0)
            {
                uint mask = (uint)(0x01 << (remBits - 1));
                ret.data[dwords - 1] |= mask;

                mask = (uint)(0xFFFFFFFF >> (32 - remBits));
                ret.data[dwords - 1] &= mask;
            }
            else
                ret.data[dwords - 1] |= 0x80000000;

            ret.Normalize();
            return ret;
        }

        /// <summary>
        /// Generates a new, random BigInteger of the specified capacity using the default RNG crypto service provider.
        /// </summary>
        /// <param name="bits">The number of bits for the new number.</param>
        /// <returns>A random number of the specified capacity.</returns>
        public static BigInteger GenerateRandom(int bits)
        {
            return GenerateRandom(bits, Rng);
        }

        /// <summary>
        /// Randomizes the bits in "this" from the specified RNG.
        /// </summary>
        /// <param name="rng">A RNG.</param>
        public void Randomize(RandomNumberGenerator rng)
        {
            if (this == 0)
                return;

            int bits = this.BitCount();
            int dwords = bits >> 5;
            int remBits = bits & 0x1F;

            if (remBits != 0)
                dwords++;

            byte[] random = new byte[dwords << 2];

            rng.GetBytes(random);
            Buffer.BlockCopy(random, 0, data, 0, (int)dwords << 2);

            if (remBits != 0)
            {
                uint mask = (uint)(0x01 << (remBits - 1));
                data[dwords - 1] |= mask;

                mask = (uint)(0xFFFFFFFF >> (32 - remBits));
                data[dwords - 1] &= mask;
            }

            else
                data[dwords - 1] |= 0x80000000;

            Normalize();
        }

        /// <summary>
        /// Randomizes the bits in "this" from the default RNG.
        /// </summary>
        public void Randomize()
        {
            Randomize(Rng);
        }

        #endregion

        #region Bitwise

        public int BitCount()
        {
            this.Normalize();

            uint value = data[length - 1];
            uint mask = 0x80000000;
            uint bits = 32;

            while (bits > 0 && (value & mask) == 0)
            {
                bits--;
                mask >>= 1;
            }
            bits += ((length - 1) << 5);

            return (int)bits;
        }

        /// <summary>
        /// Tests if the specified bit is 1.
        /// </summary>
        /// <param name="bitNum">The bit to test. The least significant bit is 0.</param>
        /// <returns>True if bitNum is set to 1, else false.</returns>
#if !INSIDE_CORLIB
        //        [CLSCompliant (false)]
#endif

        public bool TestBit(uint bitNum)
        {
            uint bytePos = bitNum >> 5;             // divide by 32
            byte bitPos = (byte)(bitNum & 0x1F);    // get the lowest 5 bits

            uint mask = (uint)1 << bitPos;
            return ((this.data[bytePos] & mask) != 0);
        }

        public bool TestBit(int bitNum)
        {
            if (bitNum < 0)
                throw new IndexOutOfRangeException("bitNum out of range");

            uint bytePos = (uint)bitNum >> 5;             // divide by 32
            byte bitPos = (byte)(bitNum & 0x1F);    // get the lowest 5 bits

            uint mask = (uint)1 << bitPos;
            return ((this.data[bytePos] | mask) == this.data[bytePos]);
        }


        public void SetBit(uint bitNum)
        {
            SetBit(bitNum, true);
        }


        public void ClearBit(uint bitNum)
        {
            SetBit(bitNum, false);
        }


        public void SetBit(uint bitNum, bool value)
        {
            uint bytePos = bitNum >> 5;             // divide by 32

            if (bytePos < this.length)
            {
                uint mask = (uint)1 << (int)(bitNum & 0x1F);
                if (value)
                    this.data[bytePos] |= mask;
                else
                    this.data[bytePos] &= ~mask;
            }
        }

        public int LowestSetBit()
        {
            if (this == 0)
                return -1;
            int i = 0;
            while (!TestBit(i))
                i++;
            return i;
        }

        public byte[] GetBytes()
        {
            if (this == 0)
                return new byte[1];

            int numBits = BitCount();
            int numBytes = numBits >> 3;
            if ((numBits & 0x7) != 0)
                numBytes++;

            byte[] result = new byte[numBytes];

            int numBytesInWord = numBytes & 0x3;
            if (numBytesInWord == 0)
                numBytesInWord = 4;

            int pos = 0;
            for (int i = (int)length - 1; i >= 0; i--)
            {
                uint val = data[i];
                for (int j = numBytesInWord - 1; j >= 0; j--)
                {
                    result[pos + j] = (byte)(val & 0xFF);
                    val >>= 8;
                }
                pos += numBytesInWord;
                numBytesInWord = 4;
            }
            return result;
        }

        #endregion

        #region Compare


        public static bool operator ==(BigInteger bi1, uint ui)
        {
            if (bi1.length != 1)
                bi1.Normalize();
            return bi1.length == 1 && bi1.data[0] == ui;
        }


        public static bool operator !=(BigInteger bi1, uint ui)
        {
            if (bi1.length != 1)
                bi1.Normalize();
            return !(bi1.length == 1 && bi1.data[0] == ui);
        }

        public static bool operator ==(BigInteger bi1, BigInteger bi2)
        {
            // we need to compare with null
            if ((bi1 as object) == (bi2 as object))
                return true;
            if (null == bi1 || null == bi2)
                return false;
            return Kernel.Compare(bi1, bi2) == 0;
        }

        public static bool operator !=(BigInteger bi1, BigInteger bi2)
        {
            // we need to compare with null
            if ((bi1 as object) == (bi2 as object))
                return false;
            if (null == bi1 || null == bi2)
                return true;
            return Kernel.Compare(bi1, bi2) != 0;
        }

        public static bool operator >(BigInteger bi1, BigInteger bi2)
        {
            return Kernel.Compare(bi1, bi2) > 0;
        }

        public static bool operator <(BigInteger bi1, BigInteger bi2)
        {
            return Kernel.Compare(bi1, bi2) < 0;
        }

        public static bool operator >=(BigInteger bi1, BigInteger bi2)
        {
            return Kernel.Compare(bi1, bi2) >= 0;
        }

        public static bool operator <=(BigInteger bi1, BigInteger bi2)
        {
            return Kernel.Compare(bi1, bi2) <= 0;
        }

        public Sign Compare(BigInteger bi)
        {
            return Kernel.Compare(this, bi);
        }

        #endregion

        #region Formatting


        public string ToString(uint radix)
        {
            return ToString(radix, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }


        public string ToString(uint radix, string characterSet)
        {
            if (characterSet.Length < radix)
                throw new ArgumentException("charSet length less than radix", "characterSet");
            if (radix == 1)
                throw new ArgumentException("There is no such thing as radix one notation", "radix");

            if (this == 0)
                return "0";
            if (this == 1)
                return "1";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            BigInteger a = new BigInteger(this);

            while (a != 0)
            {
                sb.Remove(0, sb.Length - 1);
                uint rem = Kernel.SingleByteDivideInPlace(a, radix);
                sb.Append(characterSet[(int)rem] + sb.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #region Misc

        /// <summary>
        ///     Normalizes this by setting the capacity to the actual number of
        ///     uints used in data and by setting the sign to Sign.Zero if the
        ///     value of this is 0.
        /// </summary>
        private void Normalize()
        {
            // Normalize capacity
            while (length > 0 && data[length - 1] == 0)
                length--;

            // Check for zero
            if (length == 0)
                length++;
        }

        public void Clear()
        {
            for (int i = 0; i < length; i++)
                data[i] = 0x00;
        }

        #endregion

        #region Object Impl

        public override int GetHashCode()
        {
            uint val = 0;

            for (uint i = 0; i < this.length; i++)
                val ^= this.data[i];

            return (int)val;
        }

        public override string ToString()
        {
            return ToString(10);
        }

        public override bool Equals(object o)
        {
            if (o == null)
                return false;
            if (o is int)
                return (int)o >= 0 && this == (uint)o;

            return Kernel.Compare(this, (BigInteger)o) == 0;
        }

        #endregion

        #region Number Theory

        public BigInteger GCD(BigInteger bi)
        {
            return Kernel.gcd(this, bi);
        }

        public BigInteger ModInverse(BigInteger modulus)
        {
            return Kernel.modInverse(this, modulus);
        }

        public BigInteger ModPow(BigInteger exp, BigInteger n)
        {
            ModulusRing mr = new ModulusRing(n);
            return mr.Pow(this, exp);
        }

        #endregion

        #region Prime Testing

        public bool IsProbablePrime()
        {
            if (this < smallPrimes[smallPrimes.Length - 1])
            {
                for (int p = 0; p < smallPrimes.Length; p++)
                {
                    if (this == smallPrimes[p])
                        return true;
                }
            }
            else
            {
                for (int p = 0; p < smallPrimes.Length; p++)
                {
                    if (this % smallPrimes[p] == 0)
                        return false;
                }
            }
            return PrimalityTests.RabinMillerTest(this, ConfidenceFactor.Medium);
        }

        #endregion

        #region Prime Number Generation

        /// <summary>
        /// Generates the smallest prime >= bi
        /// </summary>
        /// <param name="bi">A BigInteger</param>
        /// <returns>The smallest prime >= bi. More mathematically, if bi is prime: bi, else Prime [PrimePi [bi] + 1].</returns>
        public static BigInteger NextHighestPrime(BigInteger bi)
        {
            NextPrimeFinder npf = new NextPrimeFinder();
            return npf.GenerateNewPrime(0, bi);
        }

        public static BigInteger GeneratePseudoPrime(int bits)
        {
            SequentialSearchPrimeGeneratorBase sspg = new SequentialSearchPrimeGeneratorBase();
            return sspg.GenerateNewPrime(bits);
        }

        /// <summary>
        /// Increments this by two
        /// </summary>
        public void Incr2()
        {
            int i = 0;

            data[0] += 2;

            // If there was no carry, nothing to do
            if (data[0] < 2)
            {

                // Account for the first carry
                data[++i]++;

                // Keep adding until no carry
                while (data[i++] == 0x0)
                    data[i]++;

                // See if we increased the data capacity
                if (length == (uint)i)
                    length++;
            }
        }

        #endregion

#if INSIDE_CORLIB
        internal
#else
        public
#endif
 sealed class ModulusRing
        {

            BigInteger mod, constant;

            public ModulusRing(BigInteger modulus)
            {
                this.mod = modulus;

                // calculate constant = m^ (2k) / m
                uint i = mod.length << 1;

                constant = new BigInteger(Sign.Positive, i + 1);
                constant.data[i] = 0x00000001;

                constant = constant / mod;
            }

            public void BarrettReduction(BigInteger x)
            {
                BigInteger n = mod;
                uint k = n.length,
                    kPlusOne = k + 1,
                    kMinusOne = k - 1;

                // x < mod, so nothing to do.
                if (x.length < k)
                    return;

                BigInteger q3;

                //
                // Validate pointers
                //
                if (x.data.Length < x.length)
                    throw new IndexOutOfRangeException("x out of range");

                // q1 = x / m^ (j-1)
                // q2 = q1 * constant
                // q3 = q2 / m^ (j+1), Needs to be accessed with an offset of kPlusOne

                // TODO: We should the method in HAC p 604 to do this (14.45)
                q3 = new BigInteger(Sign.Positive, x.length - kMinusOne + constant.length);
                Kernel.Multiply(x.data, kMinusOne, x.length - kMinusOne, constant.data, 0, constant.length, q3.data, 0);

                // r1 = x mod m^ (j+1)
                // i.e. keep the lowest (j+1) words

                uint lengthToCopy = (x.length > kPlusOne) ? kPlusOne : x.length;

                x.length = lengthToCopy;
                x.Normalize();

                // r2 = (q3 * n) mod m^ (j+1)
                // partial multiplication of q3 and n

                BigInteger r2 = new BigInteger(Sign.Positive, kPlusOne);
                Kernel.MultiplyMod2p32pmod(q3.data, (int)kPlusOne, (int)q3.length - (int)kPlusOne, n.data, 0, (int)n.length, r2.data, 0, (int)kPlusOne);

                r2.Normalize();

                if (r2 <= x)
                {
                    Kernel.MinusEq(x, r2);
                }
                else
                {
                    BigInteger val = new BigInteger(Sign.Positive, kPlusOne + 1);
                    val.data[kPlusOne] = 0x00000001;

                    Kernel.MinusEq(val, r2);
                    Kernel.PlusEq(x, val);
                }

                while (x >= n)
                    Kernel.MinusEq(x, n);
            }

            public BigInteger Multiply(BigInteger a, BigInteger b)
            {
                if (a == 0 || b == 0)
                    return 0;

                if (a.length >= mod.length << 1)
                    a %= mod;

                if (b.length >= mod.length << 1)
                    b %= mod;

                if (a.length >= mod.length)
                    BarrettReduction(a);

                if (b.length >= mod.length)
                    BarrettReduction(b);

                BigInteger ret = new BigInteger(a * b);
                BarrettReduction(ret);

                return ret;
            }

            public BigInteger Difference(BigInteger a, BigInteger b)
            {
                Sign cmp = Kernel.Compare(a, b);
                BigInteger diff;

                switch (cmp)
                {
                    case Sign.Zero:
                        return 0;
                    case Sign.Positive:
                        diff = a - b;
                        break;
                    case Sign.Negative:
                        diff = b - a;
                        break;
                    default:
                        throw new Exception();
                }

                if (diff >= mod)
                {
                    if (diff.length >= mod.length << 1)
                        diff %= mod;
                    else
                        BarrettReduction(diff);
                }
                if (cmp == Sign.Negative)
                    diff = mod - diff;
                return diff;
            }

            public BigInteger Pow(BigInteger b, BigInteger exp)
            {
                if ((mod.data[0] & 1) == 1)
                    return OddPow(b, exp);
                else
                    return EvenPow(b, exp);
            }

            public BigInteger EvenPow(BigInteger b, BigInteger exp)
            {
                BigInteger resultNum = new BigInteger((BigInteger)1, mod.length << 1);
                BigInteger tempNum = new BigInteger(b % mod, mod.length << 1);  // ensures (tempNum * tempNum) < m^ (2k)

                uint totalBits = (uint)exp.BitCount();

                uint[] wkspace = new uint[mod.length << 1];

                // perform squaring and multiply exponentiation
                for (uint pos = 0; pos < totalBits; pos++)
                {
                    if (exp.TestBit(pos))
                    {

                        Array.Clear(wkspace, 0, wkspace.Length);
                        Kernel.Multiply(resultNum.data, 0, resultNum.length, tempNum.data, 0, tempNum.length, wkspace, 0);
                        resultNum.length += tempNum.length;
                        uint[] t = wkspace;
                        wkspace = resultNum.data;
                        resultNum.data = t;

                        BarrettReduction(resultNum);
                    }

                    Kernel.SquarePositive(tempNum, ref wkspace);
                    BarrettReduction(tempNum);

                    if (tempNum == 1)
                    {
                        return resultNum;
                    }
                }

                return resultNum;
            }

            private BigInteger OddPow(BigInteger b, BigInteger exp)
            {
                BigInteger resultNum = new BigInteger(Montgomery.ToMont(1, mod), mod.length << 1);
                BigInteger tempNum = new BigInteger(Montgomery.ToMont(b, mod), mod.length << 1);  // ensures (tempNum * tempNum) < m^ (2k)
                uint mPrime = Montgomery.Inverse(mod.data[0]);
                uint totalBits = (uint)exp.BitCount();

                uint[] wkspace = new uint[mod.length << 1];

                // perform squaring and multiply exponentiation
                for (uint pos = 0; pos < totalBits; pos++)
                {
                    if (exp.TestBit(pos))
                    {

                        Array.Clear(wkspace, 0, wkspace.Length);
                        Kernel.Multiply(resultNum.data, 0, resultNum.length, tempNum.data, 0, tempNum.length, wkspace, 0);
                        resultNum.length += tempNum.length;
                        uint[] t = wkspace;
                        wkspace = resultNum.data;
                        resultNum.data = t;

                        Montgomery.Reduce(resultNum, mod, mPrime);
                    }

                    Kernel.SquarePositive(tempNum, ref wkspace);
                    Montgomery.Reduce(tempNum, mod, mPrime);
                }

                Montgomery.Reduce(resultNum, mod, mPrime);
                return resultNum;
            }

            #region Pow Small Base

            // TODO: Make tests for this, not really needed m/c prime stuff
            // checks it, but still would be nice


            public BigInteger Pow(uint b, BigInteger exp)
            {
                //                if (m != 2) {
                if ((mod.data[0] & 1) == 1)
                    return OddPow(b, exp);
                else
                    return EvenPow(b, exp);
                /* buggy in some cases (like the well tested primes)
                                } else {
                                    if ((mod.data [0] & 1) == 1)
                                        return OddModTwoPow (exp);
                                    else
                                        return EvenModTwoPow (exp);
                                }*/
            }

            private unsafe BigInteger OddPow(uint b, BigInteger exp)
            {
                exp.Normalize();
                uint[] wkspace = new uint[mod.length << 1 + 1];

                BigInteger resultNum = Montgomery.ToMont((BigInteger)b, this.mod);
                resultNum = new BigInteger(resultNum, mod.length << 1 + 1);

                uint mPrime = Montgomery.Inverse(mod.data[0]);

                uint pos = (uint)exp.BitCount() - 2;

                //
                // We know that the first itr will make the val m
                //

                do
                {
                    //
                    // r = r ^ 2 % m
                    //
                    Kernel.SquarePositive(resultNum, ref wkspace);
                    resultNum = Montgomery.Reduce(resultNum, mod, mPrime);

                    if (exp.TestBit(pos))
                    {

                        //
                        // r = r * m % m
                        //

                        // TODO: Is Unsafe really speeding things up?
                        fixed (uint* u = resultNum.data)
                        {

                            uint i = 0;
                            ulong mc = 0;

                            do
                            {
                                mc += (ulong)u[i] * (ulong)b;
                                u[i] = (uint)mc;
                                mc >>= 32;
                            } while (++i < resultNum.length);

                            if (resultNum.length < mod.length)
                            {
                                if (mc != 0)
                                {
                                    u[i] = (uint)mc;
                                    resultNum.length++;
                                    while (resultNum >= mod)
                                        Kernel.MinusEq(resultNum, mod);
                                }
                            }
                            else if (mc != 0)
                            {

                                //
                                // First, we estimate the quotient by dividing
                                // the first part of each of the numbers. Then
                                // we correct this, if necessary, with a subtraction.
                                //

                                uint cc = (uint)mc;

                                // We would rather have this estimate overshoot,
                                // so we add one to the divisor
                                uint divEstimate;
                                if (mod.data[mod.length - 1] < UInt32.MaxValue)
                                {
                                    divEstimate = (uint)((((ulong)cc << 32) | (ulong)u[i - 1]) /
                                        (mod.data[mod.length - 1] + 1));
                                }
                                else
                                {
                                    // guess but don'thread divide by 0
                                    divEstimate = (uint)((((ulong)cc << 32) | (ulong)u[i - 1]) /
                                        (mod.data[mod.length - 1]));
                                }

                                uint t;

                                i = 0;
                                mc = 0;
                                do
                                {
                                    mc += (ulong)mod.data[i] * (ulong)divEstimate;
                                    t = u[i];
                                    u[i] -= (uint)mc;
                                    mc >>= 32;
                                    if (u[i] > t)
                                        mc++;
                                    i++;
                                } while (i < resultNum.length);
                                cc -= (uint)mc;

                                if (cc != 0)
                                {

                                    uint sc = 0, j = 0;
                                    uint[] s = mod.data;
                                    do
                                    {
                                        uint a = s[j];
                                        if (((a += sc) < sc) | ((u[j] -= a) > ~a))
                                            sc = 1;
                                        else
                                            sc = 0;
                                        j++;
                                    } while (j < resultNum.length);
                                    cc -= sc;
                                }
                                while (resultNum >= mod)
                                    Kernel.MinusEq(resultNum, mod);
                            }
                            else
                            {
                                while (resultNum >= mod)
                                    Kernel.MinusEq(resultNum, mod);
                            }
                        }
                    }
                } while (pos-- > 0);

                resultNum = Montgomery.Reduce(resultNum, mod, mPrime);
                return resultNum;

            }

            private unsafe BigInteger EvenPow(uint b, BigInteger exp)
            {
                exp.Normalize();
                uint[] wkspace = new uint[mod.length << 1 + 1];
                BigInteger resultNum = new BigInteger((BigInteger)b, mod.length << 1 + 1);

                uint pos = (uint)exp.BitCount() - 2;

                //
                // We know that the first itr will make the val m
                //

                do
                {
                    //
                    // r = r ^ 2 % m
                    //
                    Kernel.SquarePositive(resultNum, ref wkspace);
                    if (!(resultNum.length < mod.length))
                        BarrettReduction(resultNum);

                    if (exp.TestBit(pos))
                    {

                        //
                        // r = r * m % m
                        //

                        // TODO: Is Unsafe really speeding things up?
                        fixed (uint* u = resultNum.data)
                        {

                            uint i = 0;
                            ulong mc = 0;

                            do
                            {
                                mc += (ulong)u[i] * (ulong)b;
                                u[i] = (uint)mc;
                                mc >>= 32;
                            } while (++i < resultNum.length);

                            if (resultNum.length < mod.length)
                            {
                                if (mc != 0)
                                {
                                    u[i] = (uint)mc;
                                    resultNum.length++;
                                    while (resultNum >= mod)
                                        Kernel.MinusEq(resultNum, mod);
                                }
                            }
                            else if (mc != 0)
                            {

                                //
                                // First, we estimate the quotient by dividing
                                // the first part of each of the numbers. Then
                                // we correct this, if necessary, with a subtraction.
                                //

                                uint cc = (uint)mc;

                                // We would rather have this estimate overshoot,
                                // so we add one to the divisor
                                uint divEstimate = (uint)((((ulong)cc << 32) | (ulong)u[i - 1]) /
                                    (mod.data[mod.length - 1] + 1));

                                uint t;

                                i = 0;
                                mc = 0;
                                do
                                {
                                    mc += (ulong)mod.data[i] * (ulong)divEstimate;
                                    t = u[i];
                                    u[i] -= (uint)mc;
                                    mc >>= 32;
                                    if (u[i] > t)
                                        mc++;
                                    i++;
                                } while (i < resultNum.length);
                                cc -= (uint)mc;

                                if (cc != 0)
                                {

                                    uint sc = 0, j = 0;
                                    uint[] s = mod.data;
                                    do
                                    {
                                        uint a = s[j];
                                        if (((a += sc) < sc) | ((u[j] -= a) > ~a))
                                            sc = 1;
                                        else
                                            sc = 0;
                                        j++;
                                    } while (j < resultNum.length);
                                    cc -= sc;
                                }
                                while (resultNum >= mod)
                                    Kernel.MinusEq(resultNum, mod);
                            }
                            else
                            {
                                while (resultNum >= mod)
                                    Kernel.MinusEq(resultNum, mod);
                            }
                        }
                    }
                } while (pos-- > 0);

                return resultNum;
            }

            /* known to be buggy in some cases
                        private unsafe BigInteger EvenModTwoPow (BigInteger exp)
                        {
                            exp.Normalize ();
                            uint [] wkspace = new uint [mod.capacity << 1 + 1];
                            BigInteger resultNum = new BigInteger (2, mod.capacity << 1 +1);
                            uint value = exp.data [exp.capacity - 1];
                            uint mask = 0x80000000;
                            // Find the first bit of the exponent
                            while ((value & mask) == 0)
                                mask >>= 1;
                            //
                            // We know that the first itr will make the val 2,
                            // so eat one bit of the exponent
                            //
                            mask >>= 1;
                            uint wPos = exp.capacity - 1;
                            do {
                                value = exp.data [wPos];
                                do {
                                    Kernel.SquarePositive (resultNum, ref wkspace);
                                    if (resultNum.capacity >= mod.capacity)
                                        BarrettReduction (resultNum);
                                    if ((value & mask) != 0) {
                                        //
                                        // resultNum = (resultNum * 2) % mod
                                        //
                                        fixed (uint* u = resultNum.data) {
                                            //
                                            // Double
                                            //
                                            uint* uu = u;
                                            uint* uuE = u + resultNum.capacity;
                                            uint x, carry = 0;
                                            while (uu < uuE) {
                                                x = *uu;
                                                *uu = (x << 1) | carry;
                                                carry = x >> (32 - 1);
                                                uu++;
                                            }
                                            // subtraction inlined because we know it is square
                                            if (carry != 0 || resultNum >= mod) {
                                                uu = u;
                                                uint c = 0;
                                                uint [] s = mod.data;
                                                uint i = 0;
                                                do {
                                                    uint a = s [i];
                                                    if (((a += c) < c) | ((* (uu++) -= a) > ~a))
                                                        c = 1;
                                                    else
                                                        c = 0;
                                                    i++;
                                                } while (uu < uuE);
                                            }
                                        }
                                    }
                                } while ((mask >>= 1) > 0);
                                mask = 0x80000000;
                            } while (wPos-- > 0);
                            return resultNum;
                        }
                        private unsafe BigInteger OddModTwoPow (BigInteger exp)
                        {
                            uint [] wkspace = new uint [mod.capacity << 1 + 1];
                            BigInteger resultNum = Montgomery.ToMont ((BigInteger)2, this.mod);
                            resultNum = new BigInteger (resultNum, mod.capacity << 1 +1);
                            uint mPrime = Montgomery.Inverse (mod.data [0]);
                            //
                            // TODO: eat small bits, the ones we can do with no modular reduction
                            //
                            uint center = (uint)exp.BitCount () - 2;
                            do {
                                Kernel.SquarePositive (resultNum, ref wkspace);
                                resultNum = Montgomery.Reduce (resultNum, mod, mPrime);
                                if (exp.TestBit (center)) {
                                    //
                                    // resultNum = (resultNum * 2) % mod
                                    //
                                    fixed (uint* u = resultNum.data) {
                                        //
                                        // Double
                                        //
                                        uint* uu = u;
                                        uint* uuE = u + resultNum.capacity;
                                        uint x, carry = 0;
                                        while (uu < uuE) {
                                            x = *uu;
                                            *uu = (x << 1) | carry;
                                            carry = x >> (32 - 1);
                                            uu++;
                                        }
                                        // subtraction inlined because we know it is square
                                        if (carry != 0 || resultNum >= mod) {
                                            fixed (uint* s = mod.data) {
                                                uu = u;
                                                uint c = 0;
                                                uint* ss = s;
                                                do {
                                                    uint a = *ss++;
                                                    if (((a += c) < c) | ((* (uu++) -= a) > ~a))
                                                        c = 1;
                                                    else
                                                        c = 0;
                                                } while (uu < uuE);
                                            }
                                        }
                                    }
                                }
                            } while (center-- > 0);
                            resultNum = Montgomery.Reduce (resultNum, mod, mPrime);
                            return resultNum;
                        }
            */
            #endregion
        }

        internal sealed class Montgomery
        {

            private Montgomery()
            {
            }

            public static uint Inverse(uint n)
            {
                uint y = n, z;

                while ((z = n * y) != 1)
                    y *= 2 - z;

                return (uint)-y;
            }

            public static BigInteger ToMont(BigInteger n, BigInteger m)
            {
                n.Normalize();
                m.Normalize();

                n <<= (int)m.length * 32;
                n %= m;
                return n;
            }

            public static unsafe BigInteger Reduce(BigInteger n, BigInteger m, uint mPrime)
            {
                BigInteger A = n;
                fixed (uint* a = A.data, mm = m.data)
                {
                    for (uint i = 0; i < m.length; i++)
                    {
                        // The mod here is taken care of by the CPU,
                        // since the multiply will overflow.
                        uint u_i = a[0] * mPrime /* % 2^32 */;

                        //
                        // A += u_i * m;
                        // A >>= 32
                        //

                        // mP = Point3D in mod
                        // aSP = the source of bits from a
                        // aDP = destination for bits
                        uint* mP = mm, aSP = a, aDP = a;

                        ulong c = (ulong)u_i * ((ulong)*(mP++)) + *(aSP++);
                        c >>= 32;
                        uint j = 1;

                        // Multiply and add
                        for (; j < m.length; j++)
                        {
                            c += (ulong)u_i * (ulong)*(mP++) + *(aSP++);
                            *(aDP++) = (uint)c;
                            c >>= 32;
                        }

                        // Account for carry
                        // TODO: use a better loop here, we dont need the ulong stuff
                        for (; j < A.length; j++)
                        {
                            c += *(aSP++);
                            *(aDP++) = (uint)c;
                            c >>= 32;
                            if (c == 0)
                            {
                                j++;
                                break;
                            }
                        }
                        // SpecialCopy the rest
                        for (; j < A.length; j++)
                        {
                            *(aDP++) = *(aSP++);
                        }

                        *(aDP++) = (uint)c;
                    }

                    while (A.length > 1 && a[A.length - 1] == 0)
                        A.length--;

                }
                if (A >= m)
                    Kernel.MinusEq(A, m);

                return A;
            }
#if _NOT_USED_
            public static BigInteger Reduce (BigInteger n, BigInteger m)
            {
                return Reduce (n, m, Inverse (m.data [0]));
            }
#endif
        }

        /// <summary>
        /// Low level functions for the BigInteger
        /// </summary>
        private sealed class Kernel
        {

            #region Addition/Subtraction

            /// <summary>
            /// Adds two numbers with the same sign.
            /// </summary>
            /// <param name="bi1">A BigInteger</param>
            /// <param name="bi2">A BigInteger</param>
            /// <returns>bi1 + bi2</returns>
            public static BigInteger AddSameSign(BigInteger bi1, BigInteger bi2)
            {
                uint[] x, y;
                uint yMax, xMax, i = 0;

                // x should be bigger
                if (bi1.length < bi2.length)
                {
                    x = bi2.data;
                    xMax = bi2.length;
                    y = bi1.data;
                    yMax = bi1.length;
                }
                else
                {
                    x = bi1.data;
                    xMax = bi1.length;
                    y = bi2.data;
                    yMax = bi2.length;
                }

                BigInteger result = new BigInteger(Sign.Positive, xMax + 1);

                uint[] r = result.data;

                ulong sum = 0;

                // Add common parts of both numbers
                do
                {
                    sum = ((ulong)x[i]) + ((ulong)y[i]) + sum;
                    r[i] = (uint)sum;
                    sum >>= 32;
                } while (++i < yMax);

                // SpecialCopy remainder of longer number while carry propagation is required
                bool carry = (sum != 0);

                if (carry)
                {

                    if (i < xMax)
                    {
                        do
                            carry = ((r[i] = x[i] + 1) == 0);
                        while (++i < xMax && carry);
                    }

                    if (carry)
                    {
                        r[i] = 1;
                        result.length = ++i;
                        return result;
                    }
                }

                // SpecialCopy the rest
                if (i < xMax)
                {
                    do
                        r[i] = x[i];
                    while (++i < xMax);
                }

                result.Normalize();
                return result;
            }

            public static BigInteger Subtract(BigInteger big, BigInteger small)
            {
                BigInteger result = new BigInteger(Sign.Positive, big.length);

                uint[] r = result.data, b = big.data, s = small.data;
                uint i = 0, c = 0;

                do
                {

                    uint x = s[i];
                    if (((x += c) < c) | ((r[i] = b[i] - x) > ~x))
                        c = 1;
                    else
                        c = 0;

                } while (++i < small.length);

                if (i == big.length)
                    goto fixup;

                if (c == 1)
                {
                    do
                        r[i] = b[i] - 1;
                    while (b[i++] == 0 && i < big.length);

                    if (i == big.length)
                        goto fixup;
                }

                do
                    r[i] = b[i];
                while (++i < big.length);

            fixup:

                result.Normalize();
                return result;
            }

            public static void MinusEq(BigInteger big, BigInteger small)
            {
                uint[] b = big.data, s = small.data;
                uint i = 0, c = 0;

                do
                {
                    uint x = s[i];
                    if (((x += c) < c) | ((b[i] -= x) > ~x))
                        c = 1;
                    else
                        c = 0;
                } while (++i < small.length);

                if (i == big.length)
                    goto fixup;

                if (c == 1)
                {
                    do
                        b[i]--;
                    while (b[i++] == 0 && i < big.length);
                }

            fixup:

                // Normalize capacity
                while (big.length > 0 && big.data[big.length - 1] == 0)
                    big.length--;

                // Check for zero
                if (big.length == 0)
                    big.length++;

            }

            public static void PlusEq(BigInteger bi1, BigInteger bi2)
            {
                uint[] x, y;
                uint yMax, xMax, i = 0;
                bool flag = false;

                // x should be bigger
                if (bi1.length < bi2.length)
                {
                    flag = true;
                    x = bi2.data;
                    xMax = bi2.length;
                    y = bi1.data;
                    yMax = bi1.length;
                }
                else
                {
                    x = bi1.data;
                    xMax = bi1.length;
                    y = bi2.data;
                    yMax = bi2.length;
                }

                uint[] r = bi1.data;

                ulong sum = 0;

                // Add common parts of both numbers
                do
                {
                    sum += ((ulong)x[i]) + ((ulong)y[i]);
                    r[i] = (uint)sum;
                    sum >>= 32;
                } while (++i < yMax);

                // SpecialCopy remainder of longer number while carry propagation is required
                bool carry = (sum != 0);

                if (carry)
                {

                    if (i < xMax)
                    {
                        do
                            carry = ((r[i] = x[i] + 1) == 0);
                        while (++i < xMax && carry);
                    }

                    if (carry)
                    {
                        r[i] = 1;
                        bi1.length = ++i;
                        return;
                    }
                }

                // SpecialCopy the rest
                if (flag && i < xMax - 1)
                {
                    do
                        r[i] = x[i];
                    while (++i < xMax);
                }

                bi1.length = xMax + 1;
                bi1.Normalize();
            }

            #endregion

            #region Compare

            /// <summary>
            /// Compares two BigInteger
            /// </summary>
            /// <param name="bi1">A BigInteger</param>
            /// <param name="bi2">A BigInteger</param>
            /// <returns>The sign of bi1 - bi2</returns>
            public static Sign Compare(BigInteger bi1, BigInteger bi2)
            {
                //
                // Step 1. Compare the lengths
                //
                uint l1 = bi1.length, l2 = bi2.length;

                while (l1 > 0 && bi1.data[l1 - 1] == 0)
                    l1--;
                while (l2 > 0 && bi2.data[l2 - 1] == 0)
                    l2--;

                if (l1 == 0 && l2 == 0)
                    return Sign.Zero;

                // bi1 len < bi2 len
                if (l1 < l2)
                    return Sign.Negative;
                // bi1 len > bi2 len
                else if (l1 > l2)
                    return Sign.Positive;

                //
                // Step 2. Compare the bits
                //

                uint pos = l1 - 1;

                while (pos != 0 && bi1.data[pos] == bi2.data[pos])
                    pos--;

                if (bi1.data[pos] < bi2.data[pos])
                    return Sign.Negative;
                else if (bi1.data[pos] > bi2.data[pos])
                    return Sign.Positive;
                else
                    return Sign.Zero;
            }

            #endregion

            #region Division

            #region Dword

            /// <summary>
            /// Performs n / i and n % i in one operation.
            /// </summary>
            /// <param name="n">A BigInteger, upon exit this will hold n / i</param>
            /// <param name="i">The divisor</param>
            /// <returns>n % i</returns>
            public static uint SingleByteDivideInPlace(BigInteger n, uint d)
            {
                ulong r = 0;
                uint i = n.length;

                while (i-- > 0)
                {
                    r <<= 32;
                    r |= n.data[i];
                    n.data[i] = (uint)(r / d);
                    r %= d;
                }
                n.Normalize();

                return (uint)r;
            }

            public static uint DwordMod(BigInteger n, uint d)
            {
                ulong r = 0;
                uint i = n.length;

                while (i-- > 0)
                {
                    r <<= 32;
                    r |= n.data[i];
                    r %= d;
                }

                return (uint)r;
            }

            public static BigInteger DwordDiv(BigInteger n, uint d)
            {
                BigInteger ret = new BigInteger(Sign.Positive, n.length);

                ulong r = 0;
                uint i = n.length;

                while (i-- > 0)
                {
                    r <<= 32;
                    r |= n.data[i];
                    ret.data[i] = (uint)(r / d);
                    r %= d;
                }
                ret.Normalize();

                return ret;
            }

            public static BigInteger[] DwordDivMod(BigInteger n, uint d)
            {
                BigInteger ret = new BigInteger(Sign.Positive, n.length);

                ulong r = 0;
                uint i = n.length;

                while (i-- > 0)
                {
                    r <<= 32;
                    r |= n.data[i];
                    ret.data[i] = (uint)(r / d);
                    r %= d;
                }
                ret.Normalize();

                BigInteger rem = (uint)r;

                return new BigInteger[] { ret, rem };
            }

            #endregion

            #region BigNum

            public static BigInteger[] multiByteDivide(BigInteger bi1, BigInteger bi2)
            {
                if (Kernel.Compare(bi1, bi2) == Sign.Negative)
                    return new BigInteger[2] { 0, new BigInteger(bi1) };

                bi1.Normalize();
                bi2.Normalize();

                if (bi2.length == 1)
                    return DwordDivMod(bi1, bi2.data[0]);

                uint remainderLen = bi1.length + 1;
                int divisorLen = (int)bi2.length + 1;

                uint mask = 0x80000000;
                uint val = bi2.data[bi2.length - 1];
                int shift = 0;
                int resultPos = (int)bi1.length - (int)bi2.length;

                while (mask != 0 && (val & mask) == 0)
                {
                    shift++;
                    mask >>= 1;
                }

                BigInteger quot = new BigInteger(Sign.Positive, bi1.length - bi2.length + 1);
                BigInteger rem = (bi1 << shift);

                uint[] remainder = rem.data;

                bi2 = bi2 << shift;

                int j = (int)(remainderLen - bi2.length);
                int pos = (int)remainderLen - 1;

                uint firstDivisorByte = bi2.data[bi2.length - 1];
                ulong secondDivisorByte = bi2.data[bi2.length - 2];

                while (j > 0)
                {
                    ulong dividend = ((ulong)remainder[pos] << 32) + (ulong)remainder[pos - 1];

                    ulong q_hat = dividend / (ulong)firstDivisorByte;
                    ulong r_hat = dividend % (ulong)firstDivisorByte;

                    do
                    {

                        if (q_hat == 0x100000000 ||
                            (q_hat * secondDivisorByte) > ((r_hat << 32) + remainder[pos - 2]))
                        {
                            q_hat--;
                            r_hat += (ulong)firstDivisorByte;

                            if (r_hat < 0x100000000)
                                continue;
                        }
                        break;
                    } while (true);

                    //
                    // At this point, q_hat is either exact, or one too large
                    // (more likely to be exact) so, we attempt to multiply the
                    // divisor by q_hat, if we get a borrow, we just subtract
                    // one from q_hat and add the divisor back.
                    //

                    uint t;
                    uint dPos = 0;
                    int nPos = pos - divisorLen + 1;
                    ulong mc = 0;
                    uint uint_q_hat = (uint)q_hat;
                    do
                    {
                        mc += (ulong)bi2.data[dPos] * (ulong)uint_q_hat;
                        t = remainder[nPos];
                        remainder[nPos] -= (uint)mc;
                        mc >>= 32;
                        if (remainder[nPos] > t)
                            mc++;
                        dPos++;
                        nPos++;
                    } while (dPos < divisorLen);

                    nPos = pos - divisorLen + 1;
                    dPos = 0;

                    // Overestimate
                    if (mc != 0)
                    {
                        uint_q_hat--;
                        ulong sum = 0;

                        do
                        {
                            sum = ((ulong)remainder[nPos]) + ((ulong)bi2.data[dPos]) + sum;
                            remainder[nPos] = (uint)sum;
                            sum >>= 32;
                            dPos++;
                            nPos++;
                        } while (dPos < divisorLen);

                    }

                    quot.data[resultPos--] = (uint)uint_q_hat;

                    pos--;
                    j--;
                }

                quot.Normalize();
                rem.Normalize();
                BigInteger[] ret = new BigInteger[2] { quot, rem };

                if (shift != 0)
                    ret[1] >>= shift;

                return ret;
            }

            #endregion

            #endregion

            #region Shift
            public static BigInteger LeftShift(BigInteger bi, int n)
            {
                if (n == 0)
                    return new BigInteger(bi, bi.length + 1);

                int w = n >> 5;
                n &= ((1 << 5) - 1);

                BigInteger ret = new BigInteger(Sign.Positive, bi.length + 1 + (uint)w);

                uint i = 0, l = bi.length;
                if (n != 0)
                {
                    uint x, carry = 0;
                    while (i < l)
                    {
                        x = bi.data[i];
                        ret.data[i + w] = (x << n) | carry;
                        carry = x >> (32 - n);
                        i++;
                    }
                    ret.data[i + w] = carry;
                }
                else
                {
                    while (i < l)
                    {
                        ret.data[i + w] = bi.data[i];
                        i++;
                    }
                }

                ret.Normalize();
                return ret;
            }

            public static BigInteger RightShift(BigInteger bi, int n)
            {
                if (n == 0)
                    return new BigInteger(bi);

                int w = n >> 5;
                int s = n & ((1 << 5) - 1);

                BigInteger ret = new BigInteger(Sign.Positive, bi.length - (uint)w + 1);
                uint l = (uint)ret.data.Length - 1;

                if (s != 0)
                {

                    uint x, carry = 0;

                    while (l-- > 0)
                    {
                        x = bi.data[l + w];
                        ret.data[l] = (x >> n) | carry;
                        carry = x << (32 - n);
                    }
                }
                else
                {
                    while (l-- > 0)
                        ret.data[l] = bi.data[l + w];

                }
                ret.Normalize();
                return ret;
            }

            #endregion

            #region Multiply

            public static BigInteger MultiplyByDword(BigInteger n, uint f)
            {
                BigInteger ret = new BigInteger(Sign.Positive, n.length + 1);

                uint i = 0;
                ulong c = 0;

                do
                {
                    c += (ulong)n.data[i] * (ulong)f;
                    ret.data[i] = (uint)c;
                    c >>= 32;
                } while (++i < n.length);
                ret.data[i] = (uint)c;
                ret.Normalize();
                return ret;

            }

            /// <summary>
            /// Multiplies the data in x [xOffset:xOffset+xLen] by
            /// y [yOffset:yOffset+yLen] and puts it into
            /// i [dOffset:dOffset+xLen+yLen].
            /// </summary>
            /// <remarks>
            /// This code is unsafe! It is the caller's responsibility to make
            /// sure that it is safe to access x [xOffset:xOffset+xLen],
            /// y [yOffset:yOffset+yLen], and i [dOffset:dOffset+xLen+yLen].
            /// </remarks>
            public static unsafe void Multiply(uint[] x, uint xOffset, uint xLen, uint[] y, uint yOffset, uint yLen, uint[] d, uint dOffset)
            {
                fixed (uint* xx = x, yy = y, dd = d)
                {
                    uint* xP = xx + xOffset,
                        xE = xP + xLen,
                        yB = yy + yOffset,
                        yE = yB + yLen,
                        dB = dd + dOffset;

                    for (; xP < xE; xP++, dB++)
                    {

                        if (*xP == 0)
                            continue;

                        ulong mcarry = 0;

                        uint* dP = dB;
                        for (uint* yP = yB; yP < yE; yP++, dP++)
                        {
                            mcarry += ((ulong)*xP * (ulong)*yP) + (ulong)*dP;

                            *dP = (uint)mcarry;
                            mcarry >>= 32;
                        }

                        if (mcarry != 0)
                            *dP = (uint)mcarry;
                    }
                }
            }

            /// <summary>
            /// Multiplies the data in x [xOffset:xOffset+xLen] by
            /// y [yOffset:yOffset+yLen] and puts the low mod words into
            /// i [dOffset:dOffset+mod].
            /// </summary>
            /// <remarks>
            /// This code is unsafe! It is the caller's responsibility to make
            /// sure that it is safe to access x [xOffset:xOffset+xLen],
            /// y [yOffset:yOffset+yLen], and i [dOffset:dOffset+mod].
            /// </remarks>
            public static unsafe void MultiplyMod2p32pmod(uint[] x, int xOffset, int xLen, uint[] y, int yOffest, int yLen, uint[] d, int dOffset, int mod)
            {
                fixed (uint* xx = x, yy = y, dd = d)
                {
                    uint* xP = xx + xOffset,
                        xE = xP + xLen,
                        yB = yy + yOffest,
                        yE = yB + yLen,
                        dB = dd + dOffset,
                        dE = dB + mod;

                    for (; xP < xE; xP++, dB++)
                    {

                        if (*xP == 0)
                            continue;

                        ulong mcarry = 0;
                        uint* dP = dB;
                        for (uint* yP = yB; yP < yE && dP < dE; yP++, dP++)
                        {
                            mcarry += ((ulong)*xP * (ulong)*yP) + (ulong)*dP;

                            *dP = (uint)mcarry;
                            mcarry >>= 32;
                        }

                        if (mcarry != 0 && dP < dE)
                            *dP = (uint)mcarry;
                    }
                }
            }

            public static unsafe void SquarePositive(BigInteger bi, ref uint[] wkSpace)
            {
                uint[] t = wkSpace;
                wkSpace = bi.data;
                uint[] d = bi.data;
                uint dl = bi.length;
                bi.data = t;

                fixed (uint* dd = d, tt = t)
                {

                    uint* ttE = tt + t.Length;
                    // Clear the dest
                    for (uint* ttt = tt; ttt < ttE; ttt++)
                        *ttt = 0;

                    uint* dP = dd, tP = tt;

                    for (uint i = 0; i < dl; i++, dP++)
                    {
                        if (*dP == 0)
                            continue;

                        ulong mcarry = 0;
                        uint bi1val = *dP;

                        uint* dP2 = dP + 1, tP2 = tP + 2 * i + 1;

                        for (uint j = i + 1; j < dl; j++, tP2++, dP2++)
                        {
                            // j = i + k
                            mcarry += ((ulong)bi1val * (ulong)*dP2) + *tP2;

                            *tP2 = (uint)mcarry;
                            mcarry >>= 32;
                        }

                        if (mcarry != 0)
                            *tP2 = (uint)mcarry;
                    }

                    // Double thread. Inlined for speed.

                    tP = tt;

                    uint x, carry = 0;
                    while (tP < ttE)
                    {
                        x = *tP;
                        *tP = (x << 1) | carry;
                        carry = x >> (32 - 1);
                        tP++;
                    }
                    if (carry != 0)
                        *tP = carry;

                    // Add in the diagnals

                    dP = dd;
                    tP = tt;
                    for (uint* dE = dP + dl; (dP < dE); dP++, tP++)
                    {
                        ulong val = (ulong)*dP * (ulong)*dP + *tP;
                        *tP = (uint)val;
                        val >>= 32;
                        *(++tP) += (uint)val;
                        if (*tP < (uint)val)
                        {
                            uint* tP3 = tP;
                            // Account for the first carry
                            (*++tP3)++;

                            // Keep adding until no carry
                            while ((*tP3++) == 0)
                                (*tP3)++;
                        }

                    }

                    bi.length <<= 1;

                    // Normalize capacity
                    while (tt[bi.length - 1] == 0 && bi.length > 1)
                        bi.length--;

                }
            }

            /*
             * Never called in BigInteger (and part of a private class)
             *             public static bool Double (uint [] u, int k)
                        {
                            uint x, carry = 0;
                            uint i = 0;
                            while (i < k) {
                                x = u [i];
                                u [i] = (x << 1) | carry;
                                carry = x >> (32 - 1);
                                i++;
                            }
                            if (carry != 0) u [k] = carry;
                            return carry != 0;
                        }*/

            #endregion

            #region Number Theory

            public static BigInteger gcd(BigInteger a, BigInteger b)
            {
                BigInteger x = a;
                BigInteger y = b;

                BigInteger g = y;

                while (x.length > 1)
                {
                    g = x;
                    x = y % x;
                    y = g;

                }
                if (x == 0)
                    return g;

                // TODO: should we have something here if we can convert to long?

                //
                // Now we can just do it with single precision. I am using the binary gcd method,
                // as it should be faster.
                //

                uint yy = x.data[0];
                uint xx = y % yy;

                int t = 0;

                while (((xx | yy) & 1) == 0)
                {
                    xx >>= 1;
                    yy >>= 1;
                    t++;
                }
                while (xx != 0)
                {
                    while ((xx & 1) == 0)
                        xx >>= 1;
                    while ((yy & 1) == 0)
                        yy >>= 1;
                    if (xx >= yy)
                        xx = (xx - yy) >> 1;
                    else
                        yy = (yy - xx) >> 1;
                }

                return yy << t;
            }

            public static uint modInverse(BigInteger bi, uint modulus)
            {
                uint a = modulus, b = bi % modulus;
                uint p0 = 0, p1 = 1;

                while (b != 0)
                {
                    if (b == 1)
                        return p1;
                    p0 += (a / b) * p1;
                    a %= b;

                    if (a == 0)
                        break;
                    if (a == 1)
                        return modulus - p0;

                    p1 += (b / a) * p0;
                    b %= a;

                }
                return 0;
            }

            public static BigInteger modInverse(BigInteger bi, BigInteger modulus)
            {
                if (modulus.length == 1)
                    return modInverse(bi, modulus.data[0]);

                BigInteger[] p = { 0, 1 };
                BigInteger[] q = new BigInteger[2];    // quotients
                BigInteger[] r = { 0, 0 };             // remainders

                int step = 0;

                BigInteger a = modulus;
                BigInteger b = bi;

                ModulusRing mr = new ModulusRing(modulus);

                while (b != 0)
                {

                    if (step > 1)
                    {

                        BigInteger pval = mr.Difference(p[0], p[1] * q[0]);
                        p[0] = p[1];
                        p[1] = pval;
                    }

                    BigInteger[] divret = multiByteDivide(a, b);

                    q[0] = q[1];
                    q[1] = divret[0];
                    r[0] = r[1];
                    r[1] = divret[1];
                    a = b;
                    b = divret[1];

                    step++;
                }

                if (r[0] != 1)
                    throw (new ArithmeticException("No inverse!"));

                return mr.Difference(p[0], p[1] * q[0]);

            }
            #endregion
        }
    }

    /// <summary>
    /// A factor of confidence.
    /// </summary>
#if INSIDE_CORLIB
    internal
#else
    public
#endif
 enum ConfidenceFactor
    {
        /// <summary>
        /// Only suitable for development use, probability of failure may be greater than 1/2^20.
        /// </summary>
        ExtraLow,
        /// <summary>
        /// Suitable only for transactions which do not require forward secrecy.  Probability of failure about 1/2^40
        /// </summary>
        Low,
        /// <summary>
        /// Designed for production use. Probability of failure about 1/2^80.
        /// </summary>
        Medium,
        /// <summary>
        /// Suitable for sensitive data. Probability of failure about 1/2^160.
        /// </summary>
        High,
        /// <summary>
        /// Use only if you have lots of time! Probability of failure about 1/2^320.
        /// </summary>
        ExtraHigh,
        /// <summary>
        /// Only use methods which generate provable primes. Not yet implemented.
        /// </summary>
        Provable
    }

    /// <summary>
    /// Finds the next prime after a given number.
    /// </summary>
#if INSIDE_CORLIB
    internal
#else
    public
#endif
 class NextPrimeFinder : SequentialSearchPrimeGeneratorBase
    {

        protected override BigInteger GenerateSearchBase(int bits, object Context)
        {
            if (Context == null)
                throw new ArgumentNullException("Context");

            BigInteger ret = new BigInteger((BigInteger)Context);
            ret.SetBit(0);
            return ret;
        }
    }

#if INSIDE_CORLIB
    internal
#else
    public
#endif
 delegate bool PrimalityTest(BigInteger bi, ConfidenceFactor confidence);

#if INSIDE_CORLIB
    internal
#else
    public
#endif
 sealed class PrimalityTests
    {

        private PrimalityTests()
        {
        }

        #region SPP Test

        private static int GetSPPRounds(BigInteger bi, ConfidenceFactor confidence)
        {
            int bc = bi.BitCount();

            int Rounds;

            // Data from HAC, 4.49
            if (bc <= 100)
                Rounds = 27;
            else if (bc <= 150)
                Rounds = 18;
            else if (bc <= 200)
                Rounds = 15;
            else if (bc <= 250)
                Rounds = 12;
            else if (bc <= 300)
                Rounds = 9;
            else if (bc <= 350)
                Rounds = 8;
            else if (bc <= 400)
                Rounds = 7;
            else if (bc <= 500)
                Rounds = 6;
            else if (bc <= 600)
                Rounds = 5;
            else if (bc <= 800)
                Rounds = 4;
            else if (bc <= 1250)
                Rounds = 3;
            else
                Rounds = 2;

            switch (confidence)
            {
                case ConfidenceFactor.ExtraLow:
                    Rounds >>= 2;
                    return Rounds != 0 ? Rounds : 1;
                case ConfidenceFactor.Low:
                    Rounds >>= 1;
                    return Rounds != 0 ? Rounds : 1;
                case ConfidenceFactor.Medium:
                    return Rounds;
                case ConfidenceFactor.High:
                    return Rounds << 1;
                case ConfidenceFactor.ExtraHigh:
                    return Rounds << 2;
                case ConfidenceFactor.Provable:
                    throw new Exception("The Rabin-Miller test can not be executed in a way such that its results are provable");
                default:
                    throw new ArgumentOutOfRangeException("confidence");
            }
        }

        /// <summary>
        ///     Probabilistic prime test based on Rabin-Miller's test
        /// </summary>
        /// <param name="bi" type="BigInteger.BigInteger">
        ///     <para>
        ///         The number to test.
        ///     </para>
        /// </param>
        /// <param name="confidence" type="int">
        ///     <para>
        ///    The number of chosen bases. The test has at least a
        ///    1/4^confidence chance of falsely returning True.
        ///     </para>
        /// </param>
        /// <returns>
        ///    <para>
        ///        True if "this" is a strong pseudoprime to randomly chosen bases.
        ///    </para>
        ///    <para>
        ///        False if "this" is definitely NOT prime.
        ///    </para>
        /// </returns>
        public static bool RabinMillerTest(BigInteger bi, ConfidenceFactor confidence)
        {
            int Rounds = GetSPPRounds(bi, confidence);

            // calculate values of s and thread
            BigInteger p_sub1 = bi - 1;
            int s = p_sub1.LowestSetBit();

            BigInteger t = p_sub1 >> s;

            int bits = bi.BitCount();
            BigInteger a = null;
            BigInteger.ModulusRing mr = new BigInteger.ModulusRing(bi);

            // Applying optimization from HAC section 4.50 (base == 2)
            // not a really random base but an interesting (and speedy) one
            BigInteger b = mr.Pow(2, t);
            if (b != 1)
            {
                bool result = false;
                for (int j = 0; j < s; j++)
                {
                    if (b == p_sub1)
                    {         // a^((2^k)*thread) mod p = p-1 for some 0 <= k <= s-1
                        result = true;
                        break;
                    }

                    b = (b * b) % bi;
                }
                if (!result)
                    return false;
            }

            // still here ? start at round 1 (round 0 was a == 2)
            for (int round = 1; round < Rounds; round++)
            {
                while (true)
                {                   // generate a < n
                    a = BigInteger.GenerateRandom(bits);

                    // make sure "a" is not 0 (and not 2 as we have already tested that)
                    if (a > 2 && a < bi)
                        break;
                }

                if (a.GCD(bi) != 1)
                    return false;

                b = mr.Pow(a, t);

                if (b == 1)
                    continue;              // a^thread mod p = 1

                bool result = false;
                for (int j = 0; j < s; j++)
                {

                    if (b == p_sub1)
                    {         // a^((2^k)*thread) mod p = p-1 for some 0 <= k <= s-1
                        result = true;
                        break;
                    }

                    b = (b * b) % bi;
                }

                if (!result)
                    return false;
            }
            return true;
        }

        public static bool SmallPrimeSppTest(BigInteger bi, ConfidenceFactor confidence)
        {
            int Rounds = GetSPPRounds(bi, confidence);

            // calculate values of s and thread
            BigInteger p_sub1 = bi - 1;
            int s = p_sub1.LowestSetBit();

            BigInteger t = p_sub1 >> s;


            BigInteger.ModulusRing mr = new BigInteger.ModulusRing(bi);

            for (int round = 0; round < Rounds; round++)
            {

                BigInteger b = mr.Pow(BigInteger.smallPrimes[round], t);

                if (b == 1)
                    continue;              // a^thread mod p = 1

                bool result = false;
                for (int j = 0; j < s; j++)
                {

                    if (b == p_sub1)
                    {         // a^((2^k)*thread) mod p = p-1 for some 0 <= k <= s-1
                        result = true;
                        break;
                    }

                    b = (b * b) % bi;
                }

                if (result == false)
                    return false;
            }
            return true;
        }

        #endregion

        // TODO: Implement the Lucus test
        // TODO: Implement other new primality tests
        // TODO: Implement primality proving
    }


#if INSIDE_CORLIB
    internal
#else
    public
#endif
 abstract class PrimeGeneratorBase
    {

        public virtual ConfidenceFactor Confidence
        {
            get
            {
#if DEBUG
                return ConfidenceFactor.ExtraLow;
#else
				return ConfidenceFactor.Medium;
#endif
            }
        }

        public virtual PrimalityTest PrimalityTest
        {
            get
            {
                return new PrimalityTest(PrimalityTests.RabinMillerTest);
            }
        }

        public virtual int TrialDivisionBounds
        {
            get
            {
                return 4000;
            }
        }

        /// <summary>
        /// Performs primality tests on bi, assumes trial division has been done.
        /// </summary>
        /// <param name="bi">A BigInteger that has been subjected to and passed trial division</param>
        /// <returns>False if bi is composite, true if it may be prime.</returns>
        /// <remarks>The speed of this method is dependent on Confidence</remarks>
        protected bool PostTrialDivisionTests(BigInteger bi)
        {
            return PrimalityTest(bi, this.Confidence);
        }

        public abstract BigInteger GenerateNewPrime(int bits);
    }


#if INSIDE_CORLIB
    internal
#else
    public
#endif
 class RSAManaged : System.Security.Cryptography.RSA
    {

        private const int defaultKeySize = 1024;

        private bool isCRTpossible;
        private bool keyBlinding = true;
        private bool keypairGenerated;
        private bool m_disposed;

        private BigInteger d;
        private BigInteger p;
        private BigInteger q;
        private BigInteger dp;
        private BigInteger dq;
        private BigInteger qInv;
        private BigInteger n;        // modulus
        private BigInteger e;

        public RSAManaged()
            : this(defaultKeySize)
        {
        }

        public RSAManaged(int keySize)
        {
            LegalKeySizesValue = new KeySizes[1];
            LegalKeySizesValue[0] = new KeySizes(384, 16384, 8);
            base.KeySize = keySize;
        }

        ~RSAManaged()
        {
            // Zeroize private key
            Dispose(false);
        }

        private void GenerateKeyPair()
        {
            // p and q values should have a capacity of half the strength in bits
            int bitlength = ((KeySize + 1) >> 1);
            //            int qbitlength = (KeySize - pbitlength);
            const uint uint_e = 65537;
            e = uint_e; // fixed

            // generate p, prime and (p-1) relatively prime to e
            for (;;)
            {
                p = BigInteger.GeneratePseudoPrime(bitlength);
                if (p % uint_e != 1)
                    break;
            }
            // generate a modulus of the required capacity
            for (;;)
            {
                // generate q, prime and (q-1) relatively prime to e,
                // and not equal to p
                for (;;)
                {
                    q = BigInteger.GeneratePseudoPrime(bitlength);
                    if ((q % uint_e != 1) && (p != q))
                        break;
                }

                // calculate the modulus
                n = p * q;
                if (n.BitCount() == KeySize)
                    break;

                // if we get here our primes aren'thread big enough, make the largest
                // of the two p and try again
                if (p < q)
                    p = q;
            }

            BigInteger pSub1 = (p - 1);
            BigInteger qSub1 = (q - 1);
            BigInteger phi = pSub1 * qSub1;

            //while (phi.GCD(e) != 1) e+=2;

            // calculate the private exponent
            d = e.ModInverse(phi);

            // calculate the CRT factors
            dp = d % pSub1;
            dq = d % qSub1;
            qInv = q.ModInverse(p);

            keypairGenerated = true;
            isCRTpossible = true;

            if (KeyGenerated != null)
                KeyGenerated(this, null);
        }

        // overrides from RSA class

        public override int KeySize
        {
            get
            {
                // in case keypair hasn'thread been (yet) generated
                if (keypairGenerated)
                {
                    int ks = n.BitCount();
                    if ((ks & 7) != 0)
                        ks = ks + (8 - (ks & 7));
                    return ks;
                }
                else
                    return base.KeySize;
            }
        }
        public override string KeyExchangeAlgorithm
        {
            get
            {
                return "RSA-PKCS1-KeyEx";
            }
        }

        // note: this property will exist in RSACryptoServiceProvider in
        // version 2.0 of the framework
        public bool PublicOnly
        {
            get
            {
                return ((d == null) || (n == null));
            }
        }

        public override string SignatureAlgorithm
        {
            get
            {
                return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
            }
        }

        public override byte[] DecryptValue(byte[] rgb)
        {
            if (m_disposed)
                throw new ObjectDisposedException("private key");

            // decrypt operation is used for signature
            if (!keypairGenerated)
                GenerateKeyPair();

            BigInteger input = new BigInteger(rgb);
            BigInteger r = null;

            // we use key blinding (by default) against timing attacks
            if (keyBlinding)
            {
                // x = (r^e * g) mod n
                // *new* random number (so it's timing is also random)
                r = BigInteger.GenerateRandom(n.BitCount());
                input = r.ModPow(e, n) * input % n;
            }

            BigInteger output;
            // decrypt (which uses the private key) can be
            // optimized by using CRT (Chinese Remainder Theorem)
            if (isCRTpossible)
            {
                // m1 = c^dp mod p
                BigInteger m1 = input.ModPow(dp, p);
                // m2 = c^dq mod q
                BigInteger m2 = input.ModPow(dq, q);
                BigInteger h;
                if (m2 > m1)
                {
                    // thanks to benm!
                    h = p - ((m2 - m1) * qInv % p);
                    output = m2 + q * h;
                }
                else
                {
                    // h = (m1 - m2) * qInv mod p
                    h = (m1 - m2) * qInv % p;
                    // m = m2 + q * h;
                    output = m2 + q * h;
                }
            }
            else
            {
                // m = c^i mod n
                output = input.ModPow(d, n);
            }

            if (keyBlinding)
            {
                // Complete blinding
                // x^e / r mod n
                output = output * r.ModInverse(n) % n;
                r.Clear();
            }

            byte[] result = output.GetBytes();
            // zeroize values
            input.Clear();
            output.Clear();
            return result;
        }

        public override byte[] EncryptValue(byte[] rgb)
        {
            if (m_disposed)
                throw new ObjectDisposedException("public key");

            if (!keypairGenerated)
                GenerateKeyPair();

            BigInteger input = new BigInteger(rgb);
            BigInteger output = input.ModPow(e, n);
            byte[] result = output.GetBytes();
            // zeroize value
            input.Clear();
            output.Clear();
            return result;
        }

        public override RSAParameters ExportParameters(bool includePrivateParameters)
        {
            if (m_disposed)
                throw new ObjectDisposedException("");

            if (!keypairGenerated)
                GenerateKeyPair();

            RSAParameters param = new RSAParameters();
            param.Exponent = e.GetBytes();
            param.Modulus = n.GetBytes();
            if (includePrivateParameters)
            {
                // some parameters are required for exporting the private key
                if (d == null)
                    throw new CryptographicException("Missing private key");
                param.D = d.GetBytes();
                // hack for bugzilla #57941 where D wasn'thread provided
                if (param.D.Length != param.Modulus.Length)
                {
                    byte[] normalizedD = new byte[param.Modulus.Length];
                    Buffer.BlockCopy(param.D, 0, normalizedD, (normalizedD.Length - param.D.Length), param.D.Length);
                    param.D = normalizedD;
                }
                // but CRT parameters are optionals
                if ((p != null) && (q != null) && (dp != null) && (dq != null) && (qInv != null))
                {
                    // and we include them only if we have them all
                    param.P = p.GetBytes();
                    param.Q = q.GetBytes();
                    param.DP = dp.GetBytes();
                    param.DQ = dq.GetBytes();
                    param.InverseQ = qInv.GetBytes();
                }
            }
            return param;
        }

        public override void ImportParameters(RSAParameters parameters)
        {
            if (m_disposed)
                throw new ObjectDisposedException("");

            // if missing "mandatory" parameters
            if (parameters.Exponent == null)
                throw new CryptographicException("Missing Exponent");
            if (parameters.Modulus == null)
                throw new CryptographicException("Missing Modulus");

            e = new BigInteger(parameters.Exponent);
            n = new BigInteger(parameters.Modulus);
            // only if the private key is present
            if (parameters.D != null)
                d = new BigInteger(parameters.D);
            if (parameters.DP != null)
                dp = new BigInteger(parameters.DP);
            if (parameters.DQ != null)
                dq = new BigInteger(parameters.DQ);
            if (parameters.InverseQ != null)
                qInv = new BigInteger(parameters.InverseQ);
            if (parameters.P != null)
                p = new BigInteger(parameters.P);
            if (parameters.Q != null)
                q = new BigInteger(parameters.Q);

            // we now have a keypair
            keypairGenerated = true;
            isCRTpossible = ((p != null) && (q != null) && (dp != null) && (dq != null) && (qInv != null));
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                // Always zeroize private key
                if (d != null)
                {
                    d.Clear();
                    d = null;
                }
                if (p != null)
                {
                    p.Clear();
                    p = null;
                }
                if (q != null)
                {
                    q.Clear();
                    q = null;
                }
                if (dp != null)
                {
                    dp.Clear();
                    dp = null;
                }
                if (dq != null)
                {
                    dq.Clear();
                    dq = null;
                }
                if (qInv != null)
                {
                    qInv.Clear();
                    qInv = null;
                }

                if (disposing)
                {
                    // clear public key
                    if (e != null)
                    {
                        e.Clear();
                        e = null;
                    }
                    if (n != null)
                    {
                        n.Clear();
                        n = null;
                    }
                }
            }
            // call base class
            // no need as they all are abstract before us
            m_disposed = true;
        }

        public delegate void KeyGeneratedEventHandler(object sender, EventArgs e);

        public event KeyGeneratedEventHandler KeyGenerated;

        public override string ToXmlString(bool includePrivateParameters)
        {
            StringBuilder sb = new StringBuilder();
            RSAParameters rsaParams = ExportParameters(includePrivateParameters);
            try
            {
                sb.Append("<RSAKeyValue>");

                sb.Append("<Modulus>");
                sb.Append(Convert.ToBase64String(rsaParams.Modulus));
                sb.Append("</Modulus>");

                sb.Append("<Exponent>");
                sb.Append(Convert.ToBase64String(rsaParams.Exponent));
                sb.Append("</Exponent>");

                if (includePrivateParameters)
                {
                    if (rsaParams.P != null)
                    {
                        sb.Append("<P>");
                        sb.Append(Convert.ToBase64String(rsaParams.P));
                        sb.Append("</P>");
                    }
                    if (rsaParams.Q != null)
                    {
                        sb.Append("<Q>");
                        sb.Append(Convert.ToBase64String(rsaParams.Q));
                        sb.Append("</Q>");
                    }
                    if (rsaParams.DP != null)
                    {
                        sb.Append("<DP>");
                        sb.Append(Convert.ToBase64String(rsaParams.DP));
                        sb.Append("</DP>");
                    }
                    if (rsaParams.DQ != null)
                    {
                        sb.Append("<DQ>");
                        sb.Append(Convert.ToBase64String(rsaParams.DQ));
                        sb.Append("</DQ>");
                    }
                    if (rsaParams.InverseQ != null)
                    {
                        sb.Append("<InverseQ>");
                        sb.Append(Convert.ToBase64String(rsaParams.InverseQ));
                        sb.Append("</InverseQ>");
                    }
                    sb.Append("<D>");
                    sb.Append(Convert.ToBase64String(rsaParams.D));
                    sb.Append("</D>");
                }

                sb.Append("</RSAKeyValue>");
            }
            catch
            {
                if (rsaParams.P != null)
                    Array.Clear(rsaParams.P, 0, rsaParams.P.Length);
                if (rsaParams.Q != null)
                    Array.Clear(rsaParams.Q, 0, rsaParams.Q.Length);
                if (rsaParams.DP != null)
                    Array.Clear(rsaParams.DP, 0, rsaParams.DP.Length);
                if (rsaParams.DQ != null)
                    Array.Clear(rsaParams.DQ, 0, rsaParams.DQ.Length);
                if (rsaParams.InverseQ != null)
                    Array.Clear(rsaParams.InverseQ, 0, rsaParams.InverseQ.Length);
                if (rsaParams.D != null)
                    Array.Clear(rsaParams.D, 0, rsaParams.D.Length);
                throw;
            }

            return sb.ToString();
        }

        // internal for Mono 1.0.x in order to preserve public contract
        // they are public for Mono 1.1.x (for 1.2) as the API isn'thread froze ATM

#if NET_2_0
        public
#else
        internal
#endif
 bool UseKeyBlinding
        {
            get
            {
                return keyBlinding;
            }
            // you REALLY shoudn'thread touch this (true is fine ;-)
            set
            {
                keyBlinding = value;
            }
        }

#if NET_2_0
        public
#else
        internal
#endif
 bool IsCrtPossible
        {
            // either the key pair isn'thread generated (and will be
            // generated with CRT parameters) or CRT is (or isn'thread)
            // possible (in case the key was imported)
            get
            {
                return (!keypairGenerated || isCRTpossible);
            }
        }
    }


#if INSIDE_CORLIB
    internal
#else
    public
#endif
 class SequentialSearchPrimeGeneratorBase : PrimeGeneratorBase
    {

        protected virtual BigInteger GenerateSearchBase(int bits, object context)
        {
            BigInteger ret = BigInteger.GenerateRandom(bits);
            ret.SetBit(0);
            return ret;
        }


        public override BigInteger GenerateNewPrime(int bits)
        {
            return GenerateNewPrime(bits, null);
        }


        public virtual BigInteger GenerateNewPrime(int bits, object context)
        {
            //
            // STEP 1. Find a place to do a sequential search
            //
            BigInteger curVal = GenerateSearchBase(bits, context);

            const uint primeProd1 = 3u * 5u * 7u * 11u * 13u * 17u * 19u * 23u * 29u;

            uint pMod1 = curVal % primeProd1;

            int DivisionBound = TrialDivisionBounds;
            uint[] SmallPrimes = BigInteger.smallPrimes;
            PrimalityTest PostTrialDivisionTest = this.PrimalityTest;
            //
            // STEP 2. Search for primes
            //
            while (true)
            {

                //
                // STEP 2.1 Sieve out numbers divisible by the first 9 primes
                //
                if (pMod1 % 3 == 0)
                    goto biNotPrime;
                if (pMod1 % 5 == 0)
                    goto biNotPrime;
                if (pMod1 % 7 == 0)
                    goto biNotPrime;
                if (pMod1 % 11 == 0)
                    goto biNotPrime;
                if (pMod1 % 13 == 0)
                    goto biNotPrime;
                if (pMod1 % 17 == 0)
                    goto biNotPrime;
                if (pMod1 % 19 == 0)
                    goto biNotPrime;
                if (pMod1 % 23 == 0)
                    goto biNotPrime;
                if (pMod1 % 29 == 0)
                    goto biNotPrime;

                //
                // STEP 2.2 Sieve out all numbers divisible by the primes <= DivisionBound
                //
                for (int p = 10; p < SmallPrimes.Length && SmallPrimes[p] <= DivisionBound; p++)
                {
                    if (curVal % SmallPrimes[p] == 0)
                        goto biNotPrime;
                }

                //
                // STEP 2.3 Is the potential prime acceptable?
                //
                if (!IsPrimeAcceptable(curVal, context))
                    goto biNotPrime;

                //
                // STEP 2.4 Filter out all primes that pass this step with a primality test
                //
                if (PrimalityTest(curVal, Confidence))
                    return curVal;

                //
                // STEP 2.4
                //
                biNotPrime:
                pMod1 += 2;
                if (pMod1 >= primeProd1)
                    pMod1 -= primeProd1;
                curVal.Incr2();
            }
        }

        protected virtual bool IsPrimeAcceptable(BigInteger bi, object context)
        {
            return true;
        }
    }

    #endregion

    #region Blowfish

    public class BlowfishEngine
    {
        private static readonly uint[] KP = new uint[] {
            0x243f6a88, 0x85a308d3, 0x13198a2e, 0x03707344, 0xa4093822, 0x299f31d0,
            0x082efa98, 0xec4e6c89, 0x452821e6, 0x38d01377, 0xbe5466cf, 0x34e90c6c,
            0xc0ac29b7, 0xc97c50dd, 0x3f84d5b5, 0xb5470917, 0x9216d5d9, 0x8979fb1b
        };

        private static readonly uint[] KS0 = new uint[] {
            0xd1310ba6,   0x98dfb5ac,   0x2ffd72db,   0xd01adfb7,   0xb8e1afed,   0x6a267e96,
            0xba7c9045,   0xf12c7f99,   0x24a19947,   0xb3916cf7,   0x0801f2e2,   0x858efc16,
            0x636920d8,   0x71574e69,   0xa458fea3,   0xf4933d7e,   0x0d95748f,   0x728eb658,
            0x718bcd58,   0x82154aee,   0x7b54a41d,   0xc25a59b5,   0x9c30d539,   0x2af26013,
            0xc5d1b023,   0x286085f0,   0xca417918,   0xb8db38ef,   0x8e79dcb0,   0x603a180e,
            0x6c9e0e8b,   0xb01e8a3e,   0xd71577c1,   0xbd314b27,   0x78af2fda,   0x55605c60,
            0xe65525f3,   0xaa55ab94,   0x57489862,   0x63e81440,   0x55ca396a,   0x2aab10b6,
            0xb4cc5c34,   0x1141e8ce,   0xa15486af,   0x7c72e993,   0xb3ee1411,   0x636fbc2a,
            0x2ba9c55d,   0x741831f6,   0xce5c3e16,   0x9b87931e,   0xafd6ba33,   0x6c24cf5c,
            0x7a325381,   0x28958677,   0x3b8f4898,   0x6b4bb9af,   0xc4bfe81b,   0x66282193,
            0x61d809cc,   0xfb21a991,   0x487cac60,   0x5dec8032,   0xef845d5d,   0xe98575b1,
            0xdc262302,   0xeb651b88,   0x23893e81,   0xd396acc5,   0x0f6d6ff3,   0x83f44239,
            0x2e0b4482,   0xa4842004,   0x69c8f04a,   0x9e1f9b5e,   0x21c66842,   0xf6e96c9a,
            0x670c9c61,   0xabd388f0,   0x6a51a0d2,   0xd8542f68,   0x960fa728,   0xab5133a3,
            0x6eef0b6c,   0x137a3be4,   0xba3bf050,   0x7efb2a98,   0xa1f1651d,   0x39af0176,
            0x66ca593e,   0x82430e88,   0x8cee8619,   0x456f9fb4,   0x7d84a5c3,   0x3b8b5ebe,
            0xe06f75d8,   0x85c12073,   0x401a449f,   0x56c16aa6,   0x4ed3aa62,   0x363f7706,
            0x1bfedf72,   0x429b023d,   0x37d0d724,   0xd00a1248,   0xdb0fead3,   0x49f1c09b,
            0x075372c9,   0x80991b7b,   0x25d479d8,   0xf6e8def7,   0xe3fe501a,   0xb6794c3b,
            0x976ce0bd,   0x04c006ba,   0xc1a94fb6,   0x409f60c4,   0x5e5c9ec2,   0x196a2463,
            0x68fb6faf,   0x3e6c53b5,   0x1339b2eb,   0x3b52ec6f,   0x6dfc511f,   0x9b30952c,
            0xcc814544,   0xaf5ebd09,   0xbee3d004,   0xde334afd,   0x660f2807,   0x192e4bb3,
            0xc0cba857,   0x45c8740f,   0xd20b5f39,   0xb9d3fbdb,   0x5579c0bd,   0x1a60320a,
            0xd6a100c6,   0x402c7279,   0x679f25fe,   0xfb1fa3cc,   0x8ea5e9f8,   0xdb3222f8,
            0x3c7516df,   0xfd616b15,   0x2f501ec8,   0xad0552ab,   0x323db5fa,   0xfd238760,
            0x53317b48,   0x3e00df82,   0x9e5c57bb,   0xca6f8ca0,   0x1a87562e,   0xdf1769db,
            0xd542a8f6,   0x287effc3,   0xac6732c6,   0x8c4f5573,   0x695b27b0,   0xbbca58c8,
            0xe1ffa35d,   0xb8f011a0,   0x10fa3d98,   0xfd2183b8,   0x4afcb56c,   0x2dd1d35b,
            0x9a53e479,   0xb6f84565,   0xd28e49bc,   0x4bfb9790,   0xe1ddf2da,   0xa4cb7e33,
            0x62fb1341,   0xcee4c6e8,   0xef20cada,   0x36774c01,   0xd07e9efe,   0x2bf11fb4,
            0x95dbda4d,   0xae909198,   0xeaad8e71,   0x6b93d5a0,   0xd08ed1d0,   0xafc725e0,
            0x8e3c5b2f,   0x8e7594b7,   0x8ff6e2fb,   0xf2122b64,   0x8888b812,   0x900df01c,
            0x4fad5ea0,   0x688fc31c,   0xd1cff191,   0xb3a8c1ad,   0x2f2f2218,   0xbe0e1777,
            0xea752dfe,   0x8b021fa1,   0xe5a0cc0f,   0xb56f74e8,   0x18acf3d6,   0xce89e299,
            0xb4a84fe0,   0xfd13e0b7,   0x7cc43b81,   0xd2ada8d9,   0x165fa266,   0x80957705,
            0x93cc7314,   0x211a1477,   0xe6ad2065,   0x77b5fa86,   0xc75442f5,   0xfb9d35cf,
            0xebcdaf0c,   0x7b3e89a0,   0xd6411bd3,   0xae1e7e49,   0x00250e2d,   0x2071b35e,
            0x226800bb,   0x57b8e0af,   0x2464369b,   0xf009b91e,   0x5563911d,   0x59dfa6aa,
            0x78c14389,   0xd95a537f,   0x207d5ba2,   0x02e5b9c5,   0x83260376,   0x6295cfa9,
            0x11c81968,   0x4e734a41,   0xb3472dca,   0x7b14a94a,   0x1b510052,   0x9a532915,
            0xd60f573f,   0xbc9bc6e4,   0x2b60a476,   0x81e67400,   0x08ba6fb5,   0x571be91f,
            0xf296ec6b,   0x2a0dd915,   0xb6636521,   0xe7b9f9b6,   0xff34052e,   0xc5855664,
            0x53b02d5d,   0xa99f8fa1,   0x08ba4799,   0x6e85076a
        };

        private static readonly uint[] KS1 = new uint[] {
            0x4b7a70e9,   0xb5b32944,
            0xdb75092e,   0xc4192623,   0xad6ea6b0,   0x49a7df7d,   0x9cee60b8,   0x8fedb266,
            0xecaa8c71,   0x699a17ff,   0x5664526c,   0xc2b19ee1,   0x193602a5,   0x75094c29,
            0xa0591340,   0xe4183a3e,   0x3f54989a,   0x5b429d65,   0x6b8fe4d6,   0x99f73fd6,
            0xa1d29c07,   0xefe830f5,   0x4d2d38e6,   0xf0255dc1,   0x4cdd2086,   0x8470eb26,
            0x6382e9c6,   0x021ecc5e,   0x09686b3f,   0x3ebaefc9,   0x3c971814,   0x6b6a70a1,
            0x687f3584,   0x52a0e286,   0xb79c5305,   0xaa500737,   0x3e07841c,   0x7fdeae5c,
            0x8e7d44ec,   0x5716f2b8,   0xb03ada37,   0xf0500c0d,   0xf01c1f04,   0x0200b3ff,
            0xae0cf51a,   0x3cb574b2,   0x25837a58,   0xdc0921bd,   0xd19113f9,   0x7ca92ff6,
            0x94324773,   0x22f54701,   0x3ae5e581,   0x37c2dadc,   0xc8b57634,   0x9af3dda7,
            0xa9446146,   0x0fd0030e,   0xecc8c73e,   0xa4751e41,   0xe238cd99,   0x3bea0e2f,
            0x3280bba1,   0x183eb331,   0x4e548b38,   0x4f6db908,   0x6f420d03,   0xf60a04bf,
            0x2cb81290,   0x24977c79,   0x5679b072,   0xbcaf89af,   0xde9a771f,   0xd9930810,
            0xb38bae12,   0xdccf3f2e,   0x5512721f,   0x2e6b7124,   0x501adde6,   0x9f84cd87,
            0x7a584718,   0x7408da17,   0xbc9f9abc,   0xe94b7d8c,   0xec7aec3a,   0xdb851dfa,
            0x63094366,   0xc464c3d2,   0xef1c1847,   0x3215d908,   0xdd433b37,   0x24c2ba16,
            0x12a14d43,   0x2a65c451,   0x50940002,   0x133ae4dd,   0x71dff89e,   0x10314e55,
            0x81ac77d6,   0x5f11199b,   0x043556f1,   0xd7a3c76b,   0x3c11183b,   0x5924a509,
            0xf28fe6ed,   0x97f1fbfa,   0x9ebabf2c,   0x1e153c6e,   0x86e34570,   0xeae96fb1,
            0x860e5e0a,   0x5a3e2ab3,   0x771fe71c,   0x4e3d06fa,   0x2965dcb9,   0x99e71d0f,
            0x803e89d6,   0x5266c825,   0x2e4cc978,   0x9c10b36a,   0xc6150eba,   0x94e2ea78,
            0xa5fc3c53,   0x1e0a2df4,   0xf2f74ea7,   0x361d2b3d,   0x1939260f,   0x19c27960,
            0x5223a708,   0xf71312b6,   0xebadfe6e,   0xeac31f66,   0xe3bc4595,   0xa67bc883,
            0xb17f37d1,   0x018cff28,   0xc332ddef,   0xbe6c5aa5,   0x65582185,   0x68ab9802,
            0xeecea50f,   0xdb2f953b,   0x2aef7dad,   0x5b6e2f84,   0x1521b628,   0x29076170,
            0xecdd4775,   0x619f1510,   0x13cca830,   0xeb61bd96,   0x0334fe1e,   0xaa0363cf,
            0xb5735c90,   0x4c70a239,   0xd59e9e0b,   0xcbaade14,   0xeecc86bc,   0x60622ca7,
            0x9cab5cab,   0xb2f3846e,   0x648b1eaf,   0x19bdf0ca,   0xa02369b9,   0x655abb50,
            0x40685a32,   0x3c2ab4b3,   0x319ee9d5,   0xc021b8f7,   0x9b540b19,   0x875fa099,
            0x95f7997e,   0x623d7da8,   0xf837889a,   0x97e32d77,   0x11ed935f,   0x16681281,
            0x0e358829,   0xc7e61fd6,   0x96dedfa1,   0x7858ba99,   0x57f584a5,   0x1b227263,
            0x9b83c3ff,   0x1ac24696,   0xcdb30aeb,   0x532e3054,   0x8fd948e4,   0x6dbc3128,
            0x58ebf2ef,   0x34c6ffea,   0xfe28ed61,   0xee7c3c73,   0x5d4a14d9,   0xe864b7e3,
            0x42105d14,   0x203e13e0,   0x45eee2b6,   0xa3aaabea,   0xdb6c4f15,   0xfacb4fd0,
            0xc742f442,   0xef6abbb5,   0x654f3b1d,   0x41cd2105,   0xd81e799e,   0x86854dc7,
            0xe44b476a,   0x3d816250,   0xcf62a1f2,   0x5b8d2646,   0xfc8883a0,   0xc1c7b6a3,
            0x7f1524c3,   0x69cb7492,   0x47848a0b,   0x5692b285,   0x095bbf00,   0xad19489d,
            0x1462b174,   0x23820e00,   0x58428d2a,   0x0c55f5ea,   0x1dadf43e,   0x233f7061,
            0x3372f092,   0x8d937e41,   0xd65fecf1,   0x6c223bdb,   0x7cde3759,   0xcbee7460,
            0x4085f2a7,   0xce77326e,   0xa6078084,   0x19f8509e,   0xe8efd855,   0x61d99735,
            0xa969a7aa,   0xc50c06c2,   0x5a04abfc,   0x800bcadc,   0x9e447a2e,   0xc3453484,
            0xfdd56705,   0x0e1e9ec9,   0xdb73dbd3,   0x105588cd,   0x675fda79,   0xe3674340,
            0xc5c43465,   0x713e38d8,   0x3d28f89e,   0xf16dff20,   0x153e21e7,   0x8fb03d4a,
            0xe6e39f2b,   0xdb83adf7
        };

        private static readonly uint[] KS2 = new uint[] {
            0xe93d5a68,   0x948140f7,   0xf64c261c,   0x94692934,
            0x411520f7,   0x7602d4f7,   0xbcf46b2e,   0xd4a20068,   0xd4082471,   0x3320f46a,
            0x43b7d4b7,   0x500061af,   0x1e39f62e,   0x97244546,   0x14214f74,   0xbf8b8840,
            0x4d95fc1d,   0x96b591af,   0x70f4ddd3,   0x66a02f45,   0xbfbc09ec,   0x03bd9785,
            0x7fac6dd0,   0x31cb8504,   0x96eb27b3,   0x55fd3941,   0xda2547e6,   0xabca0a9a,
            0x28507825,   0x530429f4,   0x0a2c86da,   0xe9b66dfb,   0x68dc1462,   0xd7486900,
            0x680ec0a4,   0x27a18dee,   0x4f3ffea2,   0xe887ad8c,   0xb58ce006,   0x7af4d6b6,
            0xaace1e7c,   0xd3375fec,   0xce78a399,   0x406b2a42,   0x20fe9e35,   0xd9f385b9,
            0xee39d7ab,   0x3b124e8b,   0x1dc9faf7,   0x4b6d1856,   0x26a36631,   0xeae397b2,
            0x3a6efa74,   0xdd5b4332,   0x6841e7f7,   0xca7820fb,   0xfb0af54e,   0xd8feb397,
            0x454056ac,   0xba489527,   0x55533a3a,   0x20838d87,   0xfe6ba9b7,   0xd096954b,
            0x55a867bc,   0xa1159a58,   0xcca92963,   0x99e1db33,   0xa62a4a56,   0x3f3125f9,
            0x5ef47e1c,   0x9029317c,   0xfdf8e802,   0x04272f70,   0x80bb155c,   0x05282ce3,
            0x95c11548,   0xe4c66d22,   0x48c1133f,   0xc70f86dc,   0x07f9c9ee,   0x41041f0f,
            0x404779a4,   0x5d886e17,   0x325f51eb,   0xd59bc0d1,   0xf2bcc18f,   0x41113564,
            0x257b7834,   0x602a9c60,   0xdff8e8a3,   0x1f636c1b,   0x0e12b4c2,   0x02e1329e,
            0xaf664fd1,   0xcad18115,   0x6b2395e0,   0x333e92e1,   0x3b240b62,   0xeebeb922,
            0x85b2a20e,   0xe6ba0d99,   0xde720c8c,   0x2da2f728,   0xd0127845,   0x95b794fd,
            0x647d0862,   0xe7ccf5f0,   0x5449a36f,   0x877d48fa,   0xc39dfd27,   0xf33e8d1e,
            0x0a476341,   0x992eff74,   0x3a6f6eab,   0xf4f8fd37,   0xa812dc60,   0xa1ebddf8,
            0x991be14c,   0xdb6e6b0d,   0xc67b5510,   0x6d672c37,   0x2765d43b,   0xdcd0e804,
            0xf1290dc7,   0xcc00ffa3,   0xb5390f92,   0x690fed0b,   0x667b9ffb,   0xcedb7d9c,
            0xa091cf0b,   0xd9155ea3,   0xbb132f88,   0x515bad24,   0x7b9479bf,   0x763bd6eb,
            0x37392eb3,   0xcc115979,   0x8026e297,   0xf42e312d,   0x6842ada7,   0xc66a2b3b,
            0x12754ccc,   0x782ef11c,   0x6a124237,   0xb79251e7,   0x06a1bbe6,   0x4bfb6350,
            0x1a6b1018,   0x11caedfa,   0x3d25bdd8,   0xe2e1c3c9,   0x44421659,   0x0a121386,
            0xd90cec6e,   0xd5abea2a,   0x64af674e,   0xda86a85f,   0xbebfe988,   0x64e4c3fe,
            0x9dbc8057,   0xf0f7c086,   0x60787bf8,   0x6003604d,   0xd1fd8346,   0xf6381fb0,
            0x7745ae04,   0xd736fccc,   0x83426b33,   0xf01eab71,   0xb0804187,   0x3c005e5f,
            0x77a057be,   0xbde8ae24,   0x55464299,   0xbf582e61,   0x4e58f48f,   0xf2ddfda2,
            0xf474ef38,   0x8789bdc2,   0x5366f9c3,   0xc8b38e74,   0xb475f255,   0x46fcd9b9,
            0x7aeb2661,   0x8b1ddf84,   0x846a0e79,   0x915f95e2,   0x466e598e,   0x20b45770,
            0x8cd55591,   0xc902de4c,   0xb90bace1,   0xbb8205d0,   0x11a86248,   0x7574a99e,
            0xb77f19b6,   0xe0a9dc09,   0x662d09a1,   0xc4324633,   0xe85a1f02,   0x09f0be8c,
            0x4a99a025,   0x1d6efe10,   0x1ab93d1d,   0x0ba5a4df,   0xa186f20f,   0x2868f169,
            0xdcb7da83,   0x573906fe,   0xa1e2ce9b,   0x4fcd7f52,   0x50115e01,   0xa70683fa,
            0xa002b5c4,   0x0de6d027,   0x9af88c27,   0x773f8641,   0xc3604c06,   0x61a806b5,
            0xf0177a28,   0xc0f586e0,   0x006058aa,   0x30dc7d62,   0x11e69ed7,   0x2338ea63,
            0x53c2dd94,   0xc2c21634,   0xbbcbee56,   0x90bcb6de,   0xebfc7da1,   0xce591d76,
            0x6f05e409,   0x4b7c0188,   0x39720a3d,   0x7c927c24,   0x86e3725f,   0x724d9db9,
            0x1ac15bb4,   0xd39eb8fc,   0xed545578,   0x08fca5b5,   0xd83d7cd3,   0x4dad0fc4,
            0x1e50ef5e,   0xb161e6f8,   0xa28514d9,   0x6c51133c,   0x6fd5c7e7,   0x56e14ec4,
            0x362abfce,   0xddc6c837,   0xd79a3234,   0x92638212,   0x670efa8e,   0x406000e0
        };

        private static readonly uint[] KS3 = new uint[] {
            0x3a39ce37,   0xd3faf5cf,   0xabc27737,   0x5ac52d1b,   0x5cb0679e,   0x4fa33742,
            0xd3822740,   0x99bc9bbe,   0xd5118e9d,   0xbf0f7315,   0xd62d1c7e,   0xc700c47b,
            0xb78c1b6b,   0x21a19045,   0xb26eb1be,   0x6a366eb4,   0x5748ab2f,   0xbc946e79,
            0xc6a376d2,   0x6549c2c8,   0x530ff8ee,   0x468dde7d,   0xd5730a1d,   0x4cd04dc6,
            0x2939bbdb,   0xa9ba4650,   0xac9526e8,   0xbe5ee304,   0xa1fad5f0,   0x6a2d519a,
            0x63ef8ce2,   0x9a86ee22,   0xc089c2b8,   0x43242ef6,   0xa51e03aa,   0x9cf2d0a4,
            0x83c061ba,   0x9be96a4d,   0x8fe51550,   0xba645bd6,   0x2826a2f9,   0xa73a3ae1,
            0x4ba99586,   0xef5562e9,   0xc72fefd3,   0xf752f7da,   0x3f046f69,   0x77fa0a59,
            0x80e4a915,   0x87b08601,   0x9b09e6ad,   0x3b3ee593,   0xe990fd5a,   0x9e34d797,
            0x2cf0b7d9,   0x022b8b51,   0x96d5ac3a,   0x017da67d,   0xd1cf3ed6,   0x7c7d2d28,
            0x1f9f25cf,   0xadf2b89b,   0x5ad6b472,   0x5a88f54c,   0xe029ac71,   0xe019a5e6,
            0x47b0acfd,   0xed93fa9b,   0xe8d3c48d,   0x283b57cc,   0xf8d56629,   0x79132e28,
            0x785f0191,   0xed756055,   0xf7960e44,   0xe3d35e8c,   0x15056dd4,   0x88f46dba,
            0x03a16125,   0x0564f0bd,   0xc3eb9e15,   0x3c9057a2,   0x97271aec,   0xa93a072a,
            0x1b3f6d9b,   0x1e6321f5,   0xf59c66fb,   0x26dcf319,   0x7533d928,   0xb155fdf5,
            0x03563482,   0x8aba3cbb,   0x28517711,   0xc20ad9f8,   0xabcc5167,   0xccad925f,
            0x4de81751,   0x3830dc8e,   0x379d5862,   0x9320f991,   0xea7a90c2,   0xfb3e7bce,
            0x5121ce64,   0x774fbe32,   0xa8b6e37e,   0xc3293d46,   0x48de5369,   0x6413e680,
            0xa2ae0810,   0xdd6db224,   0x69852dfd,   0x09072166,   0xb39a460a,   0x6445c0dd,
            0x586cdecf,   0x1c20c8ae,   0x5bbef7dd,   0x1b588d40,   0xccd2017f,   0x6bb4e3bb,
            0xdda26a7e,   0x3a59ff45,   0x3e350a44,   0xbcb4cdd5,   0x72eacea8,   0xfa6484bb,
            0x8d6612ae,   0xbf3c6f47,   0xd29be463,   0x542f5d9e,   0xaec2771b,   0xf64e6370,
            0x740e0d8d,   0xe75b1357,   0xf8721671,   0xaf537d5d,   0x4040cb08,   0x4eb4e2cc,
            0x34d2466a,   0x0115af84,   0xe1b00428,   0x95983a1d,   0x06b89fb4,   0xce6ea048,
            0x6f3f3b82,   0x3520ab82,   0x011a1d4b,   0x277227f8,   0x611560b1,   0xe7933fdc,
            0xbb3a792b,   0x344525bd,   0xa08839e1,   0x51ce794b,   0x2f32c9b7,   0xa01fbac9,
            0xe01cc87e,   0xbcc7d1f6,   0xcf0111c3,   0xa1e8aac7,   0x1a908749,   0xd44fbd9a,
            0xd0dadecb,   0xd50ada38,   0x0339c32a,   0xc6913667,   0x8df9317c,   0xe0b12b4f,
            0xf79e59b7,   0x43f5bb3a,   0xf2d519ff,   0x27d9459c,   0xbf97222c,   0x15e6fc2a,
            0x0f91fc71,   0x9b941525,   0xfae59361,   0xceb69ceb,   0xc2a86459,   0x12baa8d1,
            0xb6c1075e,   0xe3056a0c,   0x10d25065,   0xcb03a442,   0xe0ec6e0e,   0x1698db3b,
            0x4c98a0be,   0x3278e964,   0x9f1f9532,   0xe0d392df,   0xd3a0342b,   0x8971f21e,
            0x1b0a7441,   0x4ba3348c,   0xc5be7120,   0xc37632d8,   0xdf359f8d,   0x9b992f2e,
            0xe60b6f47,   0x0fe3f11d,   0xe54cda54,   0x1edad891,   0xce6279cf,   0xcd3e7e6f,
            0x1618b166,   0xfd2c1d05,   0x848fd2c5,   0xf6fb2299,   0xf523f357,   0xa6327623,
            0x93a83531,   0x56cccd02,   0xacf08162,   0x5a75ebb5,   0x6e163697,   0x88d273cc,
            0xde966292,   0x81b949d0,   0x4c50901b,   0x71c65614,   0xe6c6c7bd,   0x327a140a,
            0x45e1d006,   0xc3f27b9a,   0xc9aa53fd,   0x62a80f00,   0xbb25bfe2,   0x35bdd2f6,
            0x71126905,   0xb2040222,   0xb6cbcf7c,   0xcd769c2b,   0x53113ec0,   0x1640e3d3,
            0x38abbd60,   0x2547adf0,   0xba38209c,   0xf746ce76,   0x77afa1c5,   0x20756060,
            0x85cbfe4e,   0x8ae88dd8,   0x7aaaf9b0,   0x4cf9aa7e,   0x1948c25c,   0x02fb8a8c,
            0x01c36ae4,   0xd6ebe1f9,   0x90d4f869,   0xa65cdea0,   0x3f09252d,   0xc208e69f,
            0xb74e6132,   0xce77e25b,   0x578fdfe3,   0x3ac372e6
        };

        //====================================
        // Useful constants
        //====================================
        private const int ROUNDS = 16;
        private const int BLOCK_SIZE = 8; // bytes = 64 bits
        private const int SBOX_SK = 256;
        private const int P_SZ = ROUNDS + 2;

        // the s-boxes
        private uint[] S0;
        private uint[] S1;
        private uint[] S2;
        private uint[] S3;

        // the p-array
        private uint[] P;

        private bool encrypting;
        private byte[] workingKey;

        //Constructor
        public BlowfishEngine()
        {
            S0 = new uint[SBOX_SK];
            S1 = new uint[SBOX_SK];
            S2 = new uint[SBOX_SK];
            S3 = new uint[SBOX_SK];
            P = new uint[P_SZ];
        }

        /// <summary> initialise a Blowfish cipher.
        ///
        /// </summary>
        /// <param name="">encryption
        /// whether or not we are for encryption.
        /// </param>
        /// <param name="">key
        /// the key used to set up the cipher.
        /// </param>
        /// <exception cref="IllegalArgumentException"> 
        /// if the params argument is inappropriate.
        ///
        /// </exception>
        public virtual void init(bool Encrypting, byte[] key)
        {
            this.encrypting = Encrypting;
            this.workingKey = key;
            Key = this.workingKey;
            //return ;
        }


        virtual public System.String AlgorithmName
        {
            get
            {
                return "Blowfish";
            }
        }


        public int processBlock(byte[] in_Block, int inOff, byte[] out_Block, int outOff)
        {
            if (workingKey == null)
                throw new System.SystemException("Blowfish not initialised");

            if ((inOff + BLOCK_SIZE) > in_Block.Length)
                throw new System.IO.IOException("input buffer too short");

            if ((outOff + BLOCK_SIZE) > out_Block.Length)
                throw new System.IO.IOException("output buffer too short");

            if (encrypting)
                EncryptBlock(in_Block, inOff, out_Block, outOff);
            else
                DecryptBlock(in_Block, inOff, out_Block, outOff);

            return BLOCK_SIZE;
        }


        public virtual void Reset()
        {
        }


        virtual public int BlockSize
        {
            get
            {
                return BLOCK_SIZE;
            }
        }


        //==================================
        // Private Implementation
        //==================================
        /*
                public static int URShift(int number, int bits) {
                    if (number >= 0)
                        return number >> bits;
                    else
                        return (number >> bits) + (2 << ~bits);
                }
        */
        public static long URShift(long number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2L << ~bits);
        }

        /*
                public static int URShift(int number, long bits) {
                    return URShift(number, (int)bits);
                }
                public static long URShift(long number, long bits) {
                    return URShift(number, (int)bits);
                }
        */
        private uint F(uint x)
        {
            return (((S0[(URShift(x, 24))] + S1[(URShift(x, 16)) & 0xff]) ^ S2[(URShift(x, 8)) & 0xff]) + S3[x & 0xff]);
        }

        /// <summary> apply the encryption cycle to each value pair in the table.
        /// </summary>
        private void ProcessTable(uint xl, uint xr, uint[] table)
        {
            int size = table.Length;
            for (int s = 0; s < size; s += 2)
            {
                xl ^= P[0];
                for (int i = 1; i < ROUNDS; i += 2)
                {
                    xr ^= F(xl) ^ P[i];
                    xl ^= F(xr) ^ P[i + 1];
                }

                xr ^= P[ROUNDS + 1];
                table[s] = xr;
                table[s + 1] = xl;
                xr = xl; // end of cycle swap
                xl = table[s];
            }
        }


        /// <summary> Encrypt the given input starting at the given offset and place the
        /// result in the provided buffer starting at the given offset. The input
        /// will be an exact multiple of our blocksize.
        /// </summary>
        private void EncryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
        {
            uint xl = (uint)BytesTo32bits(src, srcIndex);
            uint xr = (uint)BytesTo32bits(src, srcIndex + 4);
            xl ^= P[0];
            for (uint i = 1; i < ROUNDS; i += 2)
            {
                xr ^= F(xl) ^ P[i];
                xl ^= F(xr) ^ P[i + 1];
            }
            xr ^= P[ROUNDS + 1];
            Bits32ToBytes(xr, dst, dstIndex);
            Bits32ToBytes(xl, dst, dstIndex + 4);
        }


        /// <summary> Decrypt the given input starting at the given offset and place the
        /// result in the provided buffer starting at the given offset. The input
        /// will be an exact multiple of our blocksize.
        /// </summary>
        private void DecryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
        {
            uint xl = (uint)BytesTo32bits(src, srcIndex);
            uint xr = (uint)BytesTo32bits(src, srcIndex + 4);
            xl ^= P[ROUNDS + 1];
            for (int i = ROUNDS; i > 0; i -= 2)
            {
                xr ^= F(xl) ^ P[i];
                xl ^= F(xr) ^ P[i - 1];
            }
            xr ^= P[0];
            Bits32ToBytes(xr, dst, dstIndex);
            Bits32ToBytes(xl, dst, dstIndex + 4);
        }


        private int BytesTo32bits(byte[] b, int i)
        {
            return ((b[i + 3] & 0xff) << 24) | ((b[i + 2] & 0xff) << 16) | ((b[i + 1] & 0xff) << 8) | ((b[i] & 0xff));
        }


        private void Bits32ToBytes(uint in_Renamed, byte[] b, int offset)
        {
            b[offset] = (byte)in_Renamed;
            b[offset + 1] = (byte)(in_Renamed >> 8);
            b[offset + 2] = (byte)(in_Renamed >> 16);
            b[offset + 3] = (byte)(in_Renamed >> 24);
        }


        private byte[] Key
        {
            set
            {
                /*
                * - comments are from _Applied Crypto_, Schneier, p338 please be
                * careful comparing the two, AC numbers the arrays from 1, the
                * enclosed code from 0.
                *
                * (1) Initialise the S-boxes and the P-array, with a fixed string This
                * string contains the hexadecimal digits of pi (3.141...)
                */
                Array.Copy(KS0, 0, S0, 0, SBOX_SK);
                Array.Copy(KS1, 0, S1, 0, SBOX_SK);
                Array.Copy(KS2, 0, S2, 0, SBOX_SK);
                Array.Copy(KS3, 0, S3, 0, SBOX_SK);
                Array.Copy(KP, 0, P, 0, P_SZ);
                /*
                * (2) Now, XOR P[0] with the first 32 bits of the key, XOR P[1] with
                * the second 32-bits of the key, and so on for all bits of the key (up
                * to P[17]). Repeatedly cycle through the key bits until the entire
                * P-array has been XOR-ed with the key bits
                */
                int keyLength = value.Length;
                int keyIndex = 0;
                for (int i = 0; i < P_SZ; i++)
                {
                    // get the 32 bits of the key, in 4 * 8 bit chunks
                    int data = 0x0000000;
                    for (int j = 0; j < 4; j++)
                    {
                        // create a 32 bit block
                        data = (data << 8) | (value[keyIndex++] & 0xff);
                        // wrap when we get to the end of the key
                        if (keyIndex >= keyLength)
                        {
                            keyIndex = 0;
                        }
                    }
                    // XOR the newly created 32 bit chunk onto the P-array
                    P[i] ^= (uint)data;
                }
                /*
                * (3) Encrypt the all-zero string with the Blowfish algorithm, using
                * the subkeys described in (1) and (2)
                *
                * (4) Replace P1 and P2 with the output of step (3)
                *
                * (5) Encrypt the output of step(3) using the Blowfish algorithm, with
                * the modified subkeys.
                *
                * (6) Replace P3 and P4 with the output of step (5)
                *
                * (7) Continue the process, replacing all elements of the P-array and
                * then all four S-boxes in order, with the output of the continuously
                * changing Blowfish algorithm
                */
                ProcessTable(0, 0, P);
                ProcessTable(P[P_SZ - 2], P[P_SZ - 1], S0);
                ProcessTable(S0[SBOX_SK - 2], S0[SBOX_SK - 1], S1);
                ProcessTable(S1[SBOX_SK - 2], S1[SBOX_SK - 1], S2);
                ProcessTable(S2[SBOX_SK - 2], S2[SBOX_SK - 1], S3);
            }
        }

    }

    #endregion
}
