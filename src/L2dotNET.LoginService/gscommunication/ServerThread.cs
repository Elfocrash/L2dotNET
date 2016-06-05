using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using log4net;
using L2dotNET.LoginService.Network;
using L2dotNET.LoginService.Network.InnerNetwork.ClientPackets;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.GSCommunication
{
    public class ServerThread
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ServerThread));

        private NetworkStream nstream;
        private TcpClient client;
        private byte[] buffer;

        public string Wan { get; set; }
        public short Port { get; set; }
        public short Curp { get; set; } = 0;
        public short Maxp { get; set; } = 1000;
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
                log.Error($"ServerThread: {e.Message}");
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
                log.Error($"ServerThread: {e.Message}");
                Termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            nstream.EndRead(result);

            byte[] buff = new byte[buffer.Length];
            buffer.CopyTo(buff, 0);
            Handle(new Packet(1, buff));
            new Thread(Read).Start();
        }

        /// <summary>
        /// Handles incoming packet.
        /// </summary>
        /// <param name="packet">Incoming packet.</param>
        protected void Handle(Packet packet)
        {
            string str = "header: " + packet.FirstOpcode + "\n";

            log.Info($"{packet.ToString()}");

            switch (packet.FirstOpcode)
            {
                case 0xA0:
                    new RequestLoginServPing(packet, this).RunImpl();
                    break;
                case 0xA1:
                    new RequestLoginAuth(packet, this).RunImpl();
                    break;
                case 0xA2:
                    new RequestPlayerInGame(packet, this).RunImpl();
                    break;
                case 0xA3:
                    new RequestPlayersOnline(packet, this).RunImpl();
                    break;
            }

            //if (msg == null)
            //    return;

            //new Thread(new ThreadStart(msg.run)).Start();
        }

        private void Termination()
        {
            ServerThreadPool.Instance.Shutdown(Id);
        }

        public void Send(Packet pk)
        {
            List<byte> blist = new List<byte>();
            byte[] db = pk.GetBuffer();
            short len = (short)db.Length;
            blist.AddRange(BitConverter.GetBytes(len));
            blist.AddRange(db);
            nstream.Write(blist.ToArray(), 0, blist.Count);
            nstream.Flush();
        }

        public void close(Packet pk)
        {
            Send(pk);
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

        private readonly List<string> activeInGame = new List<string>();

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

        public void KickAccount(string account)
        {
            activeInGame.Remove(account);
            Send(PleaseKickAccount.ToPacket(account));
        }

        public void SendPlayer(LoginClient client, string time)
        {
            Send(PleaseAcceptPlayer.ToPacket(client.ActiveAccount, time));
        }
    }
}