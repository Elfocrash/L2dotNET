using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using L2dotNET.Auth.network.rcv_gs;
using L2dotNET.Auth.network.serverpackets_gs;
using log4net;

namespace L2dotNET.Auth.gscommunication
{
    public class ServerThread
    {
        ILog log = LogManager.GetLogger(typeof(ServerThread));

        private NetworkStream nstream;
        private TcpClient client;
        private byte[] buffer;

        public string Wan { get; set; }
        public short Port { get; set; }
        private short curp = 0, maxp = 1000;
        public short Curp { get { return curp; } set { curp = value; } }
        public short Maxp { get { return maxp; } set { maxp = value; } }
        public string Info { get; set; }
        public bool Connected { get; set; }
        public bool TestMode { get; set; }
        public bool GmOnly { get; set; }
        public byte Id { get; set; }

        public void ReadData(TcpClient client, ServerThreadPool cn)
        {
            this.nstream = client.GetStream();
            this.client = client;

            new Thread(Read).Start();
        }

        public void Read()
        {
            try
            {
                buffer = new byte[2];
                nstream.BeginRead(buffer, 0, 2, new AsyncCallback(OnReceiveCallbackStatic), null);
            }
            catch (Exception e)
            {
                log.Error($"ServerThread: { e.Message }");
                Termination();
            }
        }

        private void OnReceiveCallbackStatic(IAsyncResult result)
        {
            int rs = 0;
            try
            {
                rs = nstream.EndRead(result);
                if (rs > 0)
                {
                    short length = BitConverter.ToInt16(buffer, 0);
                    buffer = new byte[length];
                    nstream.BeginRead(buffer, 0, length, new AsyncCallback(OnReceiveCallback), result.AsyncState);
                }
            }
            catch (Exception e)
            {
                log.Error($"ServerThread: { e.Message }");
                Termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            nstream.EndRead(result);

            byte[] buff = new byte[buffer.Length];
            buffer.CopyTo(buff, 0);
            handlePacket(buff);
            new Thread(Read).Start();
        }

        private void handlePacket(byte[] buff)
        {
            byte id = buff[0];

            string str = "header: " + buff[0] + "\n";
            foreach (byte b in buff)
                str += b.ToString("x2") + " ";

            log.Info(str);

            ReceiveServerPacket msg = null;
            switch (id)
            {
                case 0xA0:
                    msg = new RequestLoginServPing(this, buff);
                    break;
                case 0xA1:
                    msg = new RequestLoginAuth(this, buff);
                    break;
                case 0xA2:
                    msg = new RequestPlayerInGame(this, buff);
                    break;
                case 0xA3:
                    msg = new RequestPlayersOnline(this, buff);
                    break;
                case 0xA4:
                    msg = new RequestUpdatePremiumState(this, buff);
                    break;
            }

            if (msg == null)
                return;

            new Thread(new ThreadStart(msg.run)).Start();
        }

        private void Termination()
        {
            ServerThreadPool.Instance.Shutdown(Id);
        }

        public void SendPacket(SendServerPacket pk)
        {
            pk.write();
            List<byte> blist = new List<byte>();
            byte[] db = pk.ToByteArray();
            short len = (short)db.Length;
            blist.AddRange(BitConverter.GetBytes(len));
            blist.AddRange(db);
            nstream.Write(blist.ToArray(), 0, blist.Count);
            nstream.Flush();
        }

        public void close(SendServerPacket pk)
        {
            SendPacket(pk);
            ServerThreadPool.Instance.Shutdown(Id);
        }

        public void Stop()
        {
            try
            {
                nstream.Close();
                client.Close();
            }
            catch { }

            activeInGame.Clear();
        }

        private List<string> activeInGame = new List<string>();
        public void AccountInGame(string account, byte status)
        {
            if (status == 1)
            {
                if (!activeInGame.Contains(account))
                    activeInGame.Add(account);
            }
            else
            {
                if (activeInGame.Contains(account))
                    activeInGame.Remove(account);
            }
        }

        public bool LoggedAlready(string account)
        {
            return activeInGame.Contains(account);
        }

        public void SendPlayer(LoginClient client, string time)
        {
            SendPacket(new PleaseAcceptPlayer(client.ActiveAccount, time));
        }

        public void KickAccount(string account)
        {
            activeInGame.Remove(account);
            SendPacket(new PleaseKickAccount(account));
        }
    }
}
