using System;
using System.Runtime.Remoting.Contexts;
using log4net;

namespace L2dotNET.GameService.Network
{
    [Synchronization]
    public abstract class GameServerNetworkRequest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameServerNetworkRequest));

        private byte[] _buffer;
        private int _position;
        public GameClient Client;

        public GameClient GetClient()
        {
            return Client;
        }

        public void Makeme(GameClient client, byte[] packet)
        {
            Client = client;
            _buffer = packet;
            _position = 1;
            Read();
        }

        public void Makeme(GameClient client, byte[] packet, byte plus)
        {
            Client = client;
            _buffer = packet;
            _position = 1 + plus;
            Read();
        }

        public byte ReadC()
        {
            byte result = _buffer[_position];
            _position += 1;
            return result;
        }

        public short ReadH()
        {
            short result = BitConverter.ToInt16(_buffer, _position);
            _position += 2;
            return result;
        }

        public int ReadD()
        {
            int result = BitConverter.ToInt32(_buffer, _position);
            _position += 4;
            return result;
        }

        public long ReadQ()
        {
            long result = BitConverter.ToInt64(_buffer, _position);
            _position += 8;
            return result;
        }

        public byte[] ReadB(int len)
        {
            byte[] result = new byte[len];
            Array.Copy(_buffer, _position, result, 0, len);
            _position += len;
            return result;
        }

        public double ReadF()
        {
            double result = BitConverter.ToDouble(_buffer, _position);
            _position += 8;
            return result;
        }

        public string ReadS()
        {
            string result = "";
            try
            {
                result = System.Text.Encoding.Unicode.GetString(_buffer, _position, _buffer.Length - _position);
                int idx = result.IndexOf((char)0x00);
                if (idx != -1)
                {
                    result = result.Substring(0, idx);
                }
                _position += (result.Length * 2) + 2;
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