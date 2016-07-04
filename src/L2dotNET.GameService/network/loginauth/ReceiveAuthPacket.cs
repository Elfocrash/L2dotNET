using System;
using log4net;

namespace L2dotNET.GameService.Network.LoginAuth
{
    public abstract class ReceiveAuthPacket
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReceiveAuthPacket));

        private byte[] _packet;
        private int _offset;
        public AuthThread Login;

        public void Makeme(AuthThread authLogin, byte[] packet)
        {
            Login = authLogin;
            _packet = packet;
            _offset = 1;
            Read();
        }

        public byte ReadC()
        {
            byte result = _packet[_offset];
            _offset += 1;
            return result;
        }

        public short ReadH()
        {
            short result = BitConverter.ToInt16(_packet, _offset);
            _offset += 2;
            return result;
        }

        public int ReadD()
        {
            int result = BitConverter.ToInt32(_packet, _offset);
            _offset += 4;
            return result;
        }

        public long ReadQ()
        {
            long result = BitConverter.ToInt64(_packet, _offset);
            _offset += 8;
            return result;
        }

        public byte[] ReadB(int length)
        {
            byte[] result = new byte[length];
            Array.Copy(_packet, _offset, result, 0, length);
            _offset += length;
            return result;
        }

        public double ReadF()
        {
            double result = BitConverter.ToDouble(_packet, _offset);
            _offset += 8;
            return result;
        }

        public string ReadS()
        {
            string result = "";
            try
            {
                result = System.Text.Encoding.Unicode.GetString(_packet, _offset, _packet.Length - _offset);
                int idx = result.IndexOf((char)0x00);
                if (idx != -1)
                    result = result.Substring(0, idx);
                _offset += (result.Length * 2) + 2;
            }
            catch (Exception ex)
            {
                Log.Error($"while reading string from packet, {ex.Message} {ex.StackTrace}");
            }
            return result;
        }

        public abstract void Read();

        public abstract void Run();
    }
}