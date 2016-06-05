using System;
using log4net;
using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService
{
    public abstract class ReceiveServerPacket
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReceiveServerPacket));

        private byte[] _packet;
        private int _offset;
        public ServerThread thread;

        public void CreatePacket(ServerThread thread, byte[] packet)
        {
            this.thread = thread;
            _packet = packet;
            _offset = 1;
            read();
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

        public long ReadLong()
        {
            long result = BitConverter.ToInt64(_packet, _offset);
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
                log.Error($"Error while reading string from packet, {ex.Message} {ex.StackTrace}");
            }
            return result;
        }

        public abstract void read();

        public abstract void run();
    }
}