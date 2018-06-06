using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using L2dotNET.Logging.Abstraction;
using L2dotNET.LoginService.Network;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.LoginService.GSCommunication
{
    public class ServerThread
    {
        private readonly ILog _log = LogProvider.GetCurrentClassLogger();

        private NetworkStream _nstream;
        private TcpClient _client;
        private byte[] _buffer;
        private readonly PacketHandler _packetHandler;
        public string Wan { get; set; }
        public short Port { get; set; }
        public short Curp { get; set; }
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
                _nstream.BeginRead(_buffer, 0, 2, OnReceiveCallbackStatic, null);
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
                    return;

                short length = BitConverter.ToInt16(_buffer, 0);
                _buffer = new byte[length];
                _nstream.BeginRead(_buffer, 0, length, OnReceiveCallback, result.AsyncState);
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
            _packetHandler.Handle(new Packet(1, buff), this);
            Read();
        }

        private void Termination()
        {
            LoginServer.ServiceProvider.GetService<ServerThreadPool>().Shutdown(Id);
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
            LoginServer.ServiceProvider.GetService<ServerThreadPool>().Shutdown(Id);
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

        public ServerThread(PacketHandler packetHandler)
        {
            _packetHandler = packetHandler;
        }

        public void AccountInGame(string account, byte status)
        {
            if (status == 1)
            {
                if (!_activeInGame.Contains(account))
                    _activeInGame.Add(account);
            }
            else
            {
                if (_activeInGame.Contains(account))
                    _activeInGame.Remove(account);
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