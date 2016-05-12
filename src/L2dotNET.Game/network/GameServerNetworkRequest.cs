using log4net;
using System;
using System.Runtime.Remoting.Contexts;

namespace L2dotNET.GameService.network
{
    [Synchronization]
    public abstract class GameServerNetworkRequest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GameServerNetworkRequest));

        private byte[] buffer;
        private int position;
        public GameClient Client;

        public GameClient getClient()
        {
            return Client;
        }

        public void makeme(GameClient Client, byte[] packet)
        {
            this.Client = Client;
            buffer = packet;
            position = 1;
            read();
        }

        public void makeme(GameClient Client, byte[] packet, byte plus)
        {
            this.Client = Client;
            buffer = packet;
            position = 1 + plus;
            read();
        }

        public byte readC()
        {
            byte result = buffer[position];
            position += 1;
            return result;
        }

        public short readH()
        {
            short result = BitConverter.ToInt16(buffer, position);
            position += 2;
            return result;
        }

        public int readD()
        {
            int result = BitConverter.ToInt32(buffer, position);
            position += 4;
            return result;
        }

        public long readQ()
        {
            long result = BitConverter.ToInt64(buffer, position);
            position += 8;
            return result;
        }

        public byte[] readB(int len)
        {
            byte[] result = new byte[len];
            Array.Copy(buffer, position, result, 0, len);
            position += len;
            return result;
        }

        public double readF()
        {
            double result = BitConverter.ToDouble(buffer, position);
            position += 8;
            return result;
        }

        public string readS()
        {
            string result = "";
            try
            {
                result = System.Text.Encoding.Unicode.GetString(buffer, position, buffer.Length - position);
                int idx = result.IndexOf((char)0x00);
                if (idx != -1)
                {
                    result = result.Substring(0, idx);
                }
                position += (result.Length * 2) + 2;
            }
            catch (Exception ex)
            {
                log.Error($"while reading string from packet, { ex.Message } { ex.StackTrace }");
            }
            return result;
        }

        public abstract void read();
        public abstract void run();
    }
}
