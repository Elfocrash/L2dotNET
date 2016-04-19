using System;

namespace L2dotNET.Game.crypt
{
    class BlowFishKeygen
    {
        private static int CRYPT_KEYS_SIZE = 20;
	    private static byte[][] CRYPT_KEYS = new byte[CRYPT_KEYS_SIZE][];
        private static Random rn = new Random();

        public static void genKey()
        {
            for (int i = 0; i < CRYPT_KEYS_SIZE; i++)
            {
                CRYPT_KEYS[i] = new byte[16];

                for (int j = 0; j < CRYPT_KEYS[i].Length; j++)
                {
                    CRYPT_KEYS[i][j] = (byte)rn.Next(255);
                }

                CRYPT_KEYS[i][8] = (byte)0xc8;
                CRYPT_KEYS[i][9] = (byte)0x27;
                CRYPT_KEYS[i][10] = (byte)0x93;
                CRYPT_KEYS[i][11] = (byte)0x01;
                CRYPT_KEYS[i][12] = (byte)0xa1;
                CRYPT_KEYS[i][13] = (byte)0x6c;
                CRYPT_KEYS[i][14] = (byte)0x31;
                CRYPT_KEYS[i][15] = (byte)0x97;
            }
        }

        public static byte[] getRandomKey()
        {
            return CRYPT_KEYS[rn.Next(CRYPT_KEYS_SIZE)];
        }
    }
}
