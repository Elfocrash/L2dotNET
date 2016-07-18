using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace L2dotNET.Encryption
{
    public class ScrambledKeyPair
    {
        private AsymmetricCipherKeyPair _pair;
        private AsymmetricKeyParameter _publicKey;
        public byte[] ScrambledModulus;
        public AsymmetricKeyParameter PrivateKey;

        public ScrambledKeyPair(AsymmetricCipherKeyPair pPair)
        {
            _pair = pPair;
            _publicKey = pPair.getPublic();
            ScrambledModulus = ScrambleModulus((_publicKey as RSAKeyParameters).getModulus());
            PrivateKey = pPair.getPrivate();
        }

        public static AsymmetricCipherKeyPair GenKeyPair()
        {
            RSAKeyGenerationParameters generationParameters = new RSAKeyGenerationParameters(BigInteger.valueOf(65537L), new SecureRandom(), 1024, 10);
            RSAKeyPairGenerator keyPairGenerator = new RSAKeyPairGenerator();
            keyPairGenerator.init(generationParameters);
            return keyPairGenerator.generateKeyPair();
        }

        public byte[] ScrambleModulus(BigInteger modulus)
        {
            byte[] numArray1 = modulus.toByteArray();
            if (numArray1.Length == 129 && numArray1[0] == 0)
            {
                byte[] numArray2 = new byte[128];
                Array.Copy(numArray1, 1, numArray2, 0, 128);
                numArray1 = numArray2;
            }
            for (int index = 0; index < 4; ++index)
            {
                byte num = numArray1[index];
                numArray1[index] = numArray1[77 + index];
                numArray1[77 + index] = num;
            }
            for (int index = 0; index < 64; ++index)
                numArray1[index] = (byte)(numArray1[index] ^ (uint)numArray1[64 + index]);
            for (int index = 0; index < 4; ++index)
                numArray1[13 + index] = (byte)(numArray1[13 + index] ^ (uint)numArray1[52 + index]);
            for (int index = 0; index < 64; ++index)
                numArray1[64 + index] = (byte)(numArray1[64 + index] ^ (uint)numArray1[index]);
            return numArray1;
        }
    }
}