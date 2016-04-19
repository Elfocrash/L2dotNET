using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using L2dotNET.Auth.network.rcv_gs;
using L2dotNET.Auth.network.serverpackets_gs;

namespace L2dotNET.Auth.gscommunication
{
    public class ServerThread
    {
        private NetworkStream nstream;
        private TcpClient client;
        private byte[] buffer;

        //dynamic
        public string wan;
        public short port;
        public short curp = 0, maxp = 1000;
        public string info;
        public bool connected = false;
        public bool testMode = false;
        public bool gmonly = false;
        public byte id;

        public void readData(TcpClient client, ServerThreadPool cn)
        {
            this.nstream = client.GetStream();
            this.client = client;

            new System.Threading.Thread(read).Start();
        }

        public void read()
        {
            try
            {
                buffer = new byte[2];
                nstream.BeginRead(buffer, 0, 2, new AsyncCallback(OnReceiveCallbackStatic), null);
            }
            catch (Exception e)
            {
                CLogger.error("ServerThread: " + e.Message);
                termination();
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
                CLogger.error("ServerThread: " + e.Message);
                termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            nstream.EndRead(result);

            byte[] buff = new byte[buffer.Length];
            buffer.CopyTo(buff, 0);
            handlePacket(buff);
            new System.Threading.Thread(read).Start();
        }

        private void handlePacket(byte[] buff)
        {
            byte id = buff[0];

            string str = "header: " + buff[0] + "\n";
            foreach (byte b in buff)
                str += b.ToString("x2") + " ";

            Console.WriteLine(str);

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

        private void termination()
        {
            ServerThreadPool.getInstance().shutdown(id);
        }

        public void sendPacket(SendServerPacket pk)
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
            sendPacket(pk);
            ServerThreadPool.getInstance().shutdown(id);
        }

        public void stop()
        {
            try
            {
                nstream.Close();
                client.Close();
            }
            catch  {}

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
            sendPacket(new PleaseAcceptPlayer(client.activeAccount, time));
        }

        public void KickAccount(string account)
        {
            activeInGame.Remove(account);
            sendPacket(new PleaseKickAccount(account));
        }
    }
}
