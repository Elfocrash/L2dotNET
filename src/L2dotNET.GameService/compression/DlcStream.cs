using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace L2dotNET.GameService.Compression
{
    class DlcStream
    {
        public GZipStream Stream;
        public FileStream Stream2;
        private readonly byte[] _dlcStr = Encoding.UTF8.GetBytes("DLC");

        public DlcStream(FileStream fstream, CompressionMode cm)
        {
            Stream = new GZipStream(fstream, cm);
            Stream2 = fstream;
            if (cm == CompressionMode.Compress)
                Stream2.Write(_dlcStr, 0, _dlcStr.Length);
        }

        public void Close()
        {
            Stream.Close();
            Stream.Dispose();

            Stream2.Close();
            Stream2.Dispose();
        }

        public int ReadD()
        {
            byte[] buff = new byte[4];
            Stream.Read(buff, 0, buff.Length);
            return BitConverter.ToInt32(buff, 0);
        }

        public long ReadQ()
        {
            byte[] buff = new byte[8];
            Stream.Read(buff, 0, buff.Length);
            return BitConverter.ToInt64(buff, 0);
        }

        public double ReadF()
        {
            byte[] buff = new byte[8];
            Stream.Read(buff, 0, buff.Length);
            return BitConverter.ToDouble(buff, 0);
        }

        public byte ReadC()
        {
            byte[] buff = new byte[1];
            Stream.Read(buff, 0, buff.Length);
            return buff[0];
        }

        public string ReadS(int len)
        {
            byte[] buff = new byte[len];
            Stream.Read(buff, 0, buff.Length);
            return Encoding.UTF8.GetString(buff);
        }

        public void WriteD(int d)
        {
            byte[] buff = BitConverter.GetBytes(d);
            Stream.Write(buff, 0, buff.Length);
        }

        public void WriteQ(long q)
        {
            byte[] buff = BitConverter.GetBytes(q);
            Stream.Write(buff, 0, buff.Length);
        }

        public void WriteC(byte c)
        {
            Stream.WriteByte(c);
        }

        public void WriteS(string str)
        {
            byte[] buff = Encoding.UTF8.GetBytes(str);
            Stream.Write(buff, 0, buff.Length);
        }

        public void WriteF(double f)
        {
            byte[] buff = BitConverter.GetBytes(f);
            Stream.Write(buff, 0, buff.Length);
        }
    }
}