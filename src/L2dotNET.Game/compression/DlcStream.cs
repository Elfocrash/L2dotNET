using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace L2dotNET.GameService.compression
{
    class DlcStream
    {
        public GZipStream stream;
        public FileStream stream2;
        private readonly byte[] dlcStr = Encoding.UTF8.GetBytes("DLC");

        public DlcStream(FileStream fstream, CompressionMode cm)
        {
            this.stream = new GZipStream(fstream, cm);
            this.stream2 = fstream;
            if (cm == CompressionMode.Compress)
                stream2.Write(dlcStr, 0, dlcStr.Length);
        }

        public void close()
        {
            stream.Close();
            stream.Dispose();

            stream2.Close();
            stream2.Dispose();
        }

        public int readD()
        {
            byte[] buff = new byte[4];
            stream.Read(buff, 0, buff.Length);
            return BitConverter.ToInt32(buff, 0);
        }

        public long readQ()
        {
            byte[] buff = new byte[8];
            stream.Read(buff, 0, buff.Length);
            return BitConverter.ToInt64(buff, 0);
        }

        public double readF()
        {
            byte[] buff = new byte[8];
            stream.Read(buff, 0, buff.Length);
            return BitConverter.ToDouble(buff, 0);
        }

        public byte readC()
        {
            byte[] buff = new byte[1];
            stream.Read(buff, 0, buff.Length);
            return buff[0];
        }

        public string readS(int len)
        {
            byte[] buff = new byte[len];
            stream.Read(buff, 0, buff.Length);
            return Encoding.UTF8.GetString(buff);
        }

        public void writeD(int d)
        {
            byte[] buff = BitConverter.GetBytes(d);
            stream.Write(buff, 0, buff.Length);
        }

        public void writeQ(long q)
        {
            byte[] buff = BitConverter.GetBytes(q);
            stream.Write(buff, 0, buff.Length);
        }

        public void writeC(byte c)
        {
            stream.WriteByte(c);
        }

        public void writeS(string str)
        {
            byte[] buff = Encoding.UTF8.GetBytes(str);
            stream.Write(buff, 0, buff.Length);
        }

        public void writeF(double f)
        {
            byte[] buff = BitConverter.GetBytes(f);
            stream.Write(buff, 0, buff.Length);
        }
    }
}