using log4net;
using System;

namespace L2dotNET.LoginService
{
    public abstract class ReceiveBasePacket
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReceiveBasePacket));

        private byte[] _packet;
        private int _offset;
        public LoginClient Client { get; set; }

        internal void CreatePacket(LoginClient client, byte[] packet)
        {
            Client = client;
            _packet = packet;
            _offset = 1;
            Read();
        }

        public int ReadInt()
        {
            int result = BitConverter.ToInt32(_packet, _offset);
            _offset += 4;
            return result;
        }

        public byte ReadByte()
        {
            byte result = _packet[_offset];
            _offset += 1;
            return result;
        }

        public byte[] ReadByteArray(int Length)
        {
            byte[] result = new byte[Length];
            Array.Copy(_packet, _offset, result, 0, Length);
            _offset += Length;
            return result;
        }

        public short ReadShort()
        {
            short result = BitConverter.ToInt16(_packet, _offset);
            _offset += 2;
            return result;
        }

        public double ReadDouble()
        {
            double result = BitConverter.ToDouble(_packet, _offset);
            _offset += 8;
            return result;
        }

        public string ReadString()
        {
            string result = "";
            try
            {
                result = System.Text.Encoding.Unicode.GetString(_packet, _offset, _packet.Length - _offset);
                int idx = result.IndexOf((char)0x00);
                if (idx != -1)
                {
                    result = result.Substring(0, idx);
                }
                _offset += (result.Length * 2) + 2;
            }
            catch (Exception ex)
            {
                log.Error($"While reading string from packet, { ex.Message } { ex.StackTrace }");
            }
            return result;
        }

        public abstract void Read();
        public abstract void Run();
    }
}
