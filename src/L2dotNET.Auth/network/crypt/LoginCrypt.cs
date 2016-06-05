using System;
using L2Crypt;

namespace L2dotNET.LoginService
{
    class LoginCrypt
    {
        private byte[] key = new byte[] { 
            (byte) 0x6b, (byte) 0x60, (byte) 0xcb, (byte) 0x5b, (byte) 0x82, (byte) 0xce, (byte) 0x90, (byte) 0xb1,
            (byte) 0xcc, (byte) 0x2b, (byte) 0x6c, (byte) 0x55, (byte) 0x6c, (byte) 0x6c, (byte) 0x6c, (byte) 0x6c };

        private bool updatedKey = false;
        private readonly Random rnd = new Random();
        private readonly BlowfishCipher cipher;

        public LoginCrypt()
        {
            cipher = new BlowfishCipher(key);
        }

        internal void updateKey(byte[] _blowfishKey)
        {
            key = _blowfishKey;
        }

        public bool decrypt(ref byte[] data, int offset, int size)
        {
            cipher.decipher(data, offset, size);

            return veryfyChecksum(data, offset, size);
        }

        public byte[] encrypt(byte[] data, int offset, int size)
        {
            size += 4;

            if (!updatedKey)
            {
                size += 4;
                size += 8 - size % 8;
                Array.Resize(ref data, size);
                encXORPass(data, offset, size, rnd.Next());
                cipher.cipher(data, offset, size);
                cipher.updateKey(key);
                updatedKey = true;
            }
            else
            {
                size += 8 - size % 8;
                Array.Resize(ref data, size);
                appendChecksum(data, offset, size);
                cipher.cipher(data, offset, size);
            }

            return data;
        }

        private bool veryfyChecksum(byte[] data, int offset, int size)
        {
            if ((size & 3) != 0 || size <= 4)
            {
                return false;
            }

            long chksum = 0;
            int count = size - 4;
            long check = -1;
            int i;

            for (i = offset; i < count; i += 4)
            {
                check = data[i] & 255;
                check |= data[i + 1] << 8 & 65280L;
                check |= data[i + 2] << 0x10 & 16711680L;
                check |= data[i + 3] << 0x18 & 4278190080L;

                chksum ^= check;
            }

            check = data[i] & 255;
            check |= data[i + 1] << 8 & 65280L;
            check |= data[i + 2] << 0x10 & 16711680L;
            check |= data[i + 3] << 0x18 & 4278190080L;

            return chksum == 0;
        }

        public static void appendChecksum(byte[] raw, int offset, int size)
        {
            long chksum = 0;
            int count = size - 4;
            long ecx;
            int i;

            for (i = offset; i < count; i += 4)
            {
                ecx = raw[i] & 0xff;
                ecx |= raw[i + 1] << 0x08 & 0xff00L;
                ecx |= raw[i + 2] << 0x10 & 0xff0000L;
                ecx |= raw[i + 3] << 0x18 & 0xff000000L;

                chksum ^= ecx;
            }

            ecx = raw[i] & 0xff;
            ecx |= raw[i + 1] << 0x08 & 0xff00L;
            ecx |= raw[i + 2] << 0x10 & 0xff0000L;
            ecx |= raw[i + 3] << 0x18 & 0xff000000L;

            raw[i] = (byte)(chksum & 0xff);
            raw[i + 1] = (byte)(chksum >> 0x08 & 0xff);
            raw[i + 2] = (byte)(chksum >> 0x10 & 0xff);
            raw[i + 3] = (byte)(chksum >> 0x18 & 0xff);
        }

        public static void encXORPass(byte[] raw, int offset, int size, int key)
        {
            int stop = size - 8;
            int pos = 4 + offset;
            int edx;
            int ecx = key; 

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
        }
    }
}
