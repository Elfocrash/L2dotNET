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
        private readonly ILog _log = LogManager.GetLogger(typeof(ServerThread));

        private NetworkStream _nstream;
        private TcpClient _client;
        private byte[] _buffer;

        public string Wan { get; set; }
        public short Port { get; set; }
        public short Curp { get; set; } = 0;
        public short Maxp { get; set; } = 1000;
        public string Info { get; set; }
        public bool Connected { get; set; }
        public bool TestMode { get; set; }
        public bool GmOnly { get; set; }
        public byte Id { get; set; }

        public void ReadData(TcpClient tcpClient, ServerThreadPool cn)
        {
            _nstream = tcpClient.GetStream();
            _client = tcpClient;

            new Thread(Read).Start();
        }

        public void Read()
        {
            try
            {
                _buffer = new byte[2];
                _nstream.BeginRead(_buffer, 0, 2, new AsyncCallback(OnReceiveCallbackStatic), null);
            }
            catch (Exception e)
            {
                _log.Error($"ServerThread: {e.Message}");
                Termination();
            }
        }

        private void OnReceiveCallbackStatic(IAsyncResult result)
        {
            try
            {
                int rs = _nstream.EndRead(result);
                if (rs <= 0)
                {
                    return;
                }

                short length = BitConverter.ToInt16(_buffer, 0);
                _buffer = new byte[length];
                _nstream.BeginRead(_buffer, 0, length, new AsyncCallback(OnReceiveCallback), result.AsyncState);
            }
            catch (Exception e)
            {
                _log.Error($"ServerThread: {e.Message}");
                Termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            _nstream.EndRead(result);

            byte[] buff = new byte[_buffer.Length];
            _buffer.CopyTo(buff, 0);
            Handle(new Packet(1, buff));
            new Thread(Read).Start();
        }

        /// <summary>
        /// Handles incoming packet.
        /// </summary>
        /// <param name="packet">Incoming packet.</param>
        protected void Handle(Packet packet)
        {
            //string str = "header: " + packet.FirstOpcode + "\n";

            _log.Info($"{packet}");

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
            _nstream.Write(blist.ToArray(), 0, blist.Count);
            _nstream.Flush();
        }

        public void Close(Packet pk)
        {
            Send(pk);
            ServerThreadPool.Instance.Shutdown(Id);
        }

        public void Stop()
        {
            try
            {
                _nstream.Close();
                _client.Close();
            }
            catch (Exception e)
            {
                _log.Error($"ServerThread: {e.Message}");
            }

            _activeInGame.Clear();
        }

        private readonly List<string> _activeInGame = new List<string>();

        public void AccountInGame(string account, byte status)
        {
            if (status == 1)
            {
                if (!_activeInGame.Contains(account))
                {
                    _activeInGame.Add(account);
                }
            }
            else
            {
                if (_activeInGame.Contains(account))
                {
                    _activeInGame.Remove(account);
                }
            }
        }

        public bool LoggedAlready(string account)
        {
            return _activeInGame.Contains(account);
        }

        public void KickAccount(string account)
        {
            _activeInGame.Remove(account);
            Send(PleaseKickAccount.ToPacket(account));
        }

        public void SendPlayer(LoginClient loginClient, string time)
        {
            Send(PleaseAcceptPlayer.ToPacket(loginClient.ActiveAccount, time));
        }
    }
}