using System;
using L2dotNET.Utility;

namespace L2dotNET.Encryption
{
    public static class BlowfishKeygen
    {
        private const int _cryptKeysSize = 20;
        private static readonly byte[][] _cryptKeys = new byte[_cryptKeysSize][];

        public static void Init()
        {
            for (int index1 = 0; index1 < _cryptKeysSize; ++index1)
            {
                _cryptKeys[index1] = new byte[16];
                for (int index2 = 0; index2 < _cryptKeys[index1].Length; ++index2)
                    _cryptKeys[index1][index2] = (byte) RandomThreadSafe.Instance.Next(byte.MaxValue);

                _cryptKeys[index1][8] = 200;
                _cryptKeys[index1][9] = 39;
                _cryptKeys[index1][10] = 147;
                _cryptKeys[index1][11] = 1;
                _cryptKeys[index1][12] = 161;
                _cryptKeys[index1][13] = 108;
                _cryptKeys[index1][14] = 49;
                _cryptKeys[index1][15] = 151;
            }
        }

        public static byte[] GetRandomKey()
        {
            return _cryptKeys[RandomThreadSafe.Instance.Next(_cryptKeysSize)];
        }
    }
}