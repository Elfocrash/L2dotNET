using System;
using L2dotNET.Auth.gscommunication;

namespace L2dotNET.Auth
{
    public abstract class ReceiveServerPacket
    {
        private byte[] _packet;
        private int _offset;
        public ServerThread thread;

        public void makeme(ServerThread thread, byte[] packet)
        {
            this.thread = thread;
            _packet = packet;
            _offset = 1;
            read();
        }

        public int readD()
        {
            int result = BitConverter.ToInt32(_packet, _offset);
            _offset += 4;
            return result;
        }

        public byte readC()
        {
            byte result = _packet[_offset];
            _offset += 1;
            return result;
        }

        public byte[] readB(int Length)
        {
            byte[] result = new byte[Length];
            Array.Copy(_packet, _offset, result, 0, Length);
            _offset += Length;
            return result;
        }

        public short readH()
        {
            short result = BitConverter.ToInt16(_packet, _offset);
            _offset += 2;
            return result;
        }

        public double readF()
        {
            double result = BitConverter.ToDouble(_packet, _offset);
            _offset += 8;
            return result;
        }

        public long readQ()
        {
            long result = BitConverter.ToInt64(_packet, _offset);
            _offset += 8;
            return result;
        }

        public string readS()
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
                CLogger.error("while reading string from packet, " + ex.Message + " " + ex.StackTrace);
            }
            return result;
        }

        public abstract void read();
        public abstract void run();
    }
}
