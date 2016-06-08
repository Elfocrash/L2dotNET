using System;

namespace L2dotNET.Utility
{
    public class BlowFishKeygen
    {
        private const int CRYPT_KEYS_SIZE = 20;
        private static readonly byte[][] CRYPT_KEYS = new byte[CRYPT_KEYS_SIZE][];
        private static readonly Random Random = new Random();

        public static void GenerateKeys()
        {
            for (int i = 0; i < CRYPT_KEYS_SIZE; i++)
            {
                CRYPT_KEYS[i] = new byte[16];

                for (int j = 0; j < CRYPT_KEYS[i].Length; j++)
                    CRYPT_KEYS[i][j] = (byte)Random.Next(255);

                CRYPT_KEYS[i][8] = 0xc8;
                CRYPT_KEYS[i][9] = 0x27;
                CRYPT_KEYS[i][10] = 0x93;
                CRYPT_KEYS[i][11] = 0x01;
                CRYPT_KEYS[i][12] = 0xa1;
                CRYPT_KEYS[i][13] = 0x6c;
                CRYPT_KEYS[i][14] = 0x31;
                CRYPT_KEYS[i][15] = 0x97;
            }
        }

        public static byte[] getRandomKey()
        {
            return CRYPT_KEYS[Random.Next(CRYPT_KEYS_SIZE)];
        }
    }
}